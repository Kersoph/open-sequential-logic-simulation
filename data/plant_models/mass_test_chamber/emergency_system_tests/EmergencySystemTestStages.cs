namespace Osls.Plants.MassTestChamber
{
    public class EmergencySystemTestStages
    {
        #region ==================== Fields / Properties ====================
        private const int StepTime = 10;
        private EmergencySystemTest _master;
        private int _currentStageTime;
        private bool _secondExecution = false;
        
        public enum Stages { Startup, BuildUp, Leakage, DetectorGap, HornReset, CoolDown, Reset, Wait, Done };
        public Stages Stage { get; private set; } = Stages.Startup;
        
        public AlarmHornTest AlarmHornTest { get; private set; }
        public AlarmLightTest AlarmLightTest { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates the test scenario
        /// </summary>
        public void InitialiseWith(EmergencySystemTest master)
        {
            _master = master;
            AlarmHornTest = new AlarmHornTest();
            AlarmLightTest = new AlarmLightTest();
        }
        
        /// <summary>
        /// Called by the test page to update this stage
        /// </summary>
        public void TestStage()
        {
            _master.SimulationMaster.UpdateSimulation(StepTime);
            _master.SimulationMaster.UpdateSimulation(StepTime);
            _currentStageTime += StepTime * 2;
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
                case Stages.DetectorGap:
                    StageDetectorGap();
                    break;
                case Stages.HornReset:
                    StageHornReset();
                    break;
                case Stages.CoolDown:
                    StageCoolDown();
                    break;
                case Stages.Reset:
                    StageReset();
                    break;
                case Stages.Wait:
                    StageWait();
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
            _master.PaperLog.Append("Trying to reset the alarms in every step...\n");
            _currentStageTime = 0;
            Stage = Stages.BuildUp;
        }
        
        private void StageBuildUp()
        {
            AlarmHornTest.Check(_master, false);
            AlarmLightTest.Check(_master, false);
            if (_master.Simulation.ParticleSensorSignal)
            {
                _currentStageTime = 0;
                _master.PaperLog.Append("-- Stage build up done\n");
                Stage = Stages.Leakage;
            }
            else if (_currentStageTime > 2500 && _currentStageTime < 2550)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
                _master.Simulation.OnMuteButtonChange(false);
            }
            else if (_currentStageTime > 500 && _currentStageTime < 550)
            {
                _master.Simulation.OnAcknowledgeButtonChange(true);
                _master.Simulation.OnMuteButtonChange(true);
            }
        }
        
        private void StageLeakage()
        {
            AlarmHornTest.Check(_master, true);
            AlarmLightTest.Check(_master, true);
            if (_currentStageTime > 1000)
            {
                _currentStageTime = 0;
                _master.PaperLog.Append("-- Stage leakage done\n");
                Stage = Stages.DetectorGap;
            }
            else if (_currentStageTime > 600)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
            }
            else if (_currentStageTime > 500)
            {
                _master.Simulation.OnAcknowledgeButtonChange(true);
            }
        }
        
        private void StageDetectorGap()
        {
            AlarmHornTest.Check(_master, true);
            AlarmLightTest.Check(_master, true);
            _master.Simulation.SurpressParticleSensorSignal = true;
            if (_currentStageTime > 500)
            {
                _currentStageTime = 0;
                _master.Simulation.SurpressParticleSensorSignal = false;
                _master.PaperLog.Append("The particle detector had a gap of 500ms for a short time.\n");
                _master.PaperLog.Append("-- Stage detector gap done\n");
                Stage = Stages.HornReset;
            }
            else if (_currentStageTime > 300)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
            }
            else if (_currentStageTime > 200)
            {
                _master.Simulation.OnAcknowledgeButtonChange(true);
            }
        }
        
        private void StageHornReset()
        {
            if (_currentStageTime > 500)
            {
                _currentStageTime = 0;
                _master.PaperLog.Append("Operators tried to reset the horn and light\n");
                _master.PaperLog.Append("-- Stage horn reset done\n");
                Stage = Stages.CoolDown;
            }
            else if (_currentStageTime > 200)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
                _master.Simulation.OnMuteButtonChange(false);
                AlarmHornTest.Check(_master, false);
                AlarmLightTest.Check(_master, true);
            }
            else if (_currentStageTime > 100)
            {
                _master.Simulation.OnAcknowledgeButtonChange(true);
                _master.Simulation.OnMuteButtonChange(true);
            }
        }
        
        private void StageCoolDown()
        {
            AlarmHornTest.Check(_master, false);
            AlarmLightTest.Check(_master, true);
            if (!_master.Simulation.ParticleSensorSignal)
            {
                _currentStageTime = 0;
                _master.PaperLog.Append("-- Stage cool down done\n");
                Stage = Stages.Reset;
            }
            else if (_currentStageTime > 200)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
                _master.Simulation.OnMuteButtonChange(false);
            }
            else if (_currentStageTime > 100)
            {
                _master.Simulation.OnAcknowledgeButtonChange(true);
                _master.Simulation.OnMuteButtonChange(true);
            }
        }
        
        private void StageReset()
        {
            if (_currentStageTime > 1500)
            {
                _currentStageTime = 0;
                _master.PaperLog.Append("Operators tried to reset the horn and light at 100ms and 1100ms\n");
                _master.PaperLog.Append("-- Stage reset done\n");
                Stage = Stages.Wait;
            }
            else if (_currentStageTime > 1200)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
                _master.Simulation.OnMuteButtonChange(false);
                AlarmHornTest.Check(_master, false);
                AlarmLightTest.Check(_master, false);
            }
            else if (_currentStageTime > 1100)
            {
                _master.Simulation.OnAcknowledgeButtonChange(true);
                _master.Simulation.OnMuteButtonChange(true);
            }
            else if (_currentStageTime > 200)
            {
                _master.Simulation.OnAcknowledgeButtonChange(false);
                _master.Simulation.OnMuteButtonChange(false);
                AlarmHornTest.Check(_master, false);
                AlarmLightTest.Check(_master, true);
            }
            else if (_currentStageTime > 100)
            {
                _master.Simulation.OnAcknowledgeButtonChange(true);
                _master.Simulation.OnMuteButtonChange(true);
            }
        }
        
        private void StageWait()
        {
            AlarmHornTest.Check(_master, false);
            AlarmLightTest.Check(_master, false);
            if (_currentStageTime > 5000)
            {
                bool hadErrors = AlarmHornTest.ReportedEarlyActivation
                    || AlarmHornTest.ReportedEarlyDeactivation
                    || AlarmLightTest.ReportedEarlyActivation
                    || AlarmLightTest.ReportedEarlyDeactivation;
                if (hadErrors || _secondExecution)
                {
                    _master.PaperLog.Append("\n*** Stopped protocol ***\n\n");
                    _currentStageTime = 0;
                    Stage = Stages.Done;
                }
                else
                {
                    _secondExecution = true;
                    _master.PaperLog.Append("\n*** Trying to reset and execute again ***\n\n");
                    _master.Simulation.ResetSimulation();
                    _currentStageTime = 0;
                    Stage = Stages.BuildUp;
                }
            }
        }
        #endregion
    }
}
