namespace Osls.Plants.MassTestChamber
{
    public class EmergencySystemTestStages
    {
        #region ==================== Fields / Properties ====================
        private const int StepTime = 10;
        private EmergencySystemTest _master;
        private int _currentStageTime;
        private bool _resetControllerDone;
        
        public enum Stages { Startup, BuildUp, Leakage, WaitCharges, Reset, Redo, Blackout, DirectReset, Wait, CoolDown, Done };
        public Stages Stage { get; private set; } = Stages.Startup;
        
        public BatteryTest BatteryTest { get; private set; }
        public TankTest TankTest { get; private set; }
        public SafeguardTest SafeguardTest { get; private set; }
        public AlarmLightTest AlarmLightTest { get; private set; }
        public bool StageRechargeTimedOut { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates the test scenario
        /// </summary>
        public void InitialiseWith(EmergencySystemTest master)
        {
            _master = master;
            BatteryTest = new BatteryTest();
            TankTest = new TankTest();
            SafeguardTest = new SafeguardTest();
            AlarmLightTest = new AlarmLightTest();
        }
        
        /// <summary>
        /// Called by the test page to update this stage
        /// </summary>
        public void TestStage()
        {
            _master.SimulationMaster.UpdateSimulation(StepTime);
            _currentStageTime += StepTime;
            switch (Stage)
            {
                case Stages.Startup:
                    StageStartup();
                    break;
                case Stages.BuildUp:
                    StageBuildUp();
                    break;
                case Stages.Leakage:
                    StageLeakage();
                    break;
                case Stages.WaitCharges:
                    StageWaitCharges();
                    break;
                case Stages.Reset:
                    StageReset();
                    break;
                case Stages.Redo:
                    StageRedo();
                    break;
                case Stages.Blackout:
                    StageBlackout();
                    break;
                case Stages.DirectReset:
                    StageDirectReset();
                    break;
                case Stages.Wait:
                    StageWait();
                    break;
                case Stages.CoolDown:
                    StageCoolDown();
                    break;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void StageStartup()
        {
            _master.PaperLog.Append("[u][b][center]Automated Initial Emergency Test Protocoll[/center][/b][/u]\n");
            _master.PaperLog.Append("[center]Author: HPK    Version: 1.0.17    Supervisor: Dr. Rosenberg[/center]\n\n");
            _master.PaperLog.Append("***Starting protocoll***\n\n");
            _currentStageTime = 0;
            Stage = Stages.BuildUp;
        }
        
        private void StageBuildUp()
        {
            BatteryTest.Check(_master, false);
            TankTest.Check(_master, false);
            AlarmLightTest.Check(_master, false);
            if (_master.Simulation.SafeguardSignal)
            {
                _currentStageTime = 0;
                _master.PaperLog.Append("-- Stage build up done\n");
                Stage = Stages.Leakage;
            }
            else if (_currentStageTime > 2500 && _currentStageTime < 2550)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
            }
            else if (_currentStageTime > 500 && _currentStageTime < 550)
            {
                _master.Simulation.OnAcknowledgeButtonChange(true);
            }
        }
        
        private void StageLeakage()
        {
            BatteryTest.Check(_master);
            TankTest.Check(_master);
            if (_currentStageTime > 100)
            {
                AlarmLightTest.Check(_master, true);
                SafeguardTest.Check(_master, true);
                _currentStageTime = 0;
                _master.PaperLog.Append("-- Stage leakage done\n");
                Stage = Stages.WaitCharges;
            }
        }
        
        private void StageWaitCharges()
        {
            BatteryTest.Check(_master);
            TankTest.Check(_master);
            SafeguardTest.Check(_master, true);
            AlarmLightTest.Check(_master, true);
            if ((_master.Simulation.PressureSignal >= 2000 && _master.Simulation.BatterySignal >= 45f)
            || _currentStageTime > 10000)
            {
                if (_currentStageTime < 5000)
                {
                    _master.PaperLog.Append("Time needed to charge: " + _currentStageTime + "\n");
                    _master.PaperLog.Append("-- Stage wait for battery and tank done\n");
                }
                else
                {
                    StageRechargeTimedOut = true;
                    _master.PaperLog.AppendWarning("-- Stage wait for battery and tank timed out!\n");
                }
                _currentStageTime = 0;
                Stage = Stages.Reset;
            }
        }
        
        private void StageReset()
        {
            if (_currentStageTime > 300)
            {
                AlarmLightTest.Check(_master, false);
                SafeguardTest.Check(_master, false);
                _currentStageTime = 0;
                _master.PaperLog.Append("-- Stage reset alarm done\n");
                _master.PaperLog.Append("***Reset Simulation***\n");
                _master.Simulation.ResetSimulation();
                Stage = Stages.Redo;
            }
            else if (_currentStageTime > 200)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
            }
            else if (_currentStageTime > 100)
            {
                BatteryTest.Check(_master, false);
                TankTest.Check(_master, false);
                _master.Simulation.OnAcknowledgeButtonChange(true);
            }
        }
        
        private void StageRedo()
        {
            BatteryTest.Check(_master, false);
            TankTest.Check(_master, false);
            AlarmLightTest.Check(_master, false);
            if (_master.Simulation.SafeguardSignal)
            {
                _currentStageTime = 0;
                _master.PaperLog.Append("-- Stage wait for second error done \n");
                Stage = Stages.Blackout;
            }
        }
        
        private void StageBlackout()
        {
            if (_currentStageTime > 300)
            {
                SafeguardTest.Check(_master, true);
                AlarmLightTest.Check(_master, true);
                _master.PaperLog.Append("-- Stage controller reset done \n");
                _currentStageTime = 0;
                Stage = Stages.DirectReset;
            }
            else if (_currentStageTime > 200)
            {
            }
            else if (_currentStageTime > 100)
            {
                if (!_resetControllerDone)
                {
                    _master.SimulationMaster.Reset();
                    _master.PaperLog.Append("Recorded controller blackout - restarting... \n");
                    _resetControllerDone = true;
                }
            }
            else if (_currentStageTime > 50)
            {
                AlarmLightTest.Check(_master, true);
            }
        }
        
        private void StageDirectReset()
        {
            BatteryTest.Check(_master, true);
            TankTest.Check(_master, true);
            if (_currentStageTime > 450)
            {
                SafeguardTest.Check(_master, true);
                AlarmLightTest.Check(_master, false);
                _master.PaperLog.Append("-- Stage quickly acknowledge the alarm done \n");
                _currentStageTime = 0;
                Stage = Stages.Wait;
            }
            else if (_currentStageTime > 300)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
            }
            else if (_currentStageTime > 150)
            {
                _master.Simulation.OnAcknowledgeButtonChange(true);
            }
            else if (_currentStageTime > 50)
            {
                AlarmLightTest.Check(_master, true);
            }
        }
        
        private void StageWait()
        {
            BatteryTest.Check(_master, true);
            TankTest.Check(_master, true);
            AlarmLightTest.Check(_master, false);
            if ((_master.Simulation.PressureSignal >= 2000 && _master.Simulation.BatterySignal >= 45f)
            || _currentStageTime > 10000)
            {
                if (_currentStageTime < 4600)
                {
                    _master.PaperLog.Append("Time needed to charge: " + _currentStageTime + "\n");
                    _master.PaperLog.Append("-- Stage wait for battery and tank done\n");
                }
                else
                {
                    StageRechargeTimedOut = true;
                    _master.PaperLog.AppendWarning("-- Stage wait for battery and tank timed out!\n");
                }
                _currentStageTime = 0;
                Stage = Stages.CoolDown;
            }
        }
        
        private void StageCoolDown()
        {
            AlarmLightTest.Check(_master, false);
            if (_currentStageTime > 1000)
            {
                _master.PaperLog.Append("\n*** Stopped protocol ***\n\n");
                _currentStageTime = 0;
                Stage = Stages.Done;
            }
            if (_currentStageTime > 100)
            {
                BatteryTest.Check(_master, false);
                TankTest.Check(_master, false);
                SafeguardTest.Check(_master, false);
            }
        }
        #endregion
    }
}
