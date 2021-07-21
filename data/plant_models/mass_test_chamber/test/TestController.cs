namespace Osls.Plants.MassTestChamber
{
    public class TestController
    {
        #region ==================== Fields / Properties ====================
        private const int StepTime = 20;
        private readonly Test _master;
        
        public enum Stages { Setup, Rails, BuildMass, Cage, Discharge, Reset, Done };
        public Stages Stage { get; private set; } = Stages.Setup;
        
        public TestCart EmitterCart { get; private set; }
        public TestCart FocusCart { get; private set; }
        public TestEmitter TestEmitter { get; private set; }
        public TestFocus TestFocus { get; private set; }
        public TestLaser TestLaser { get; private set; }
        public TestFieldGenerator TestFieldGenerator { get; private set; }
        public TestTemperature TestTemperature { get; private set; }
        
        /// <summary>
        /// Time in ms used for every stage according to the Stages enum.
        /// -1 is set for stages which timed out.
        /// </summary>
        public int[] StageTime { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public TestController(Test master)
        {
            _master = master;
            StageTime = new int[7];
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates the test scenario
        /// </summary>
        public void Setup()
        {
            _master.Simulation.Chamber.EmitterCart.WarpToPosition(1f);
            EmitterCart = new TestCart(_master.Simulation.Chamber.EmitterCart, "Emitter");
            FocusCart = new TestCart(_master.Simulation.Chamber.FocusCart, "Focus");
            TestEmitter = new TestEmitter();
            TestFocus = new TestFocus();
            TestLaser = new TestLaser();
            TestFieldGenerator = new TestFieldGenerator();
            TestTemperature = new TestTemperature();
        }
        
        /// <summary>
        /// Executes the automated initial operation protocoll
        /// </summary>
        public void ExecuteProtocoll()
        {
            StageTime[(int)Stage] += StepTime;
            switch (Stage)
            {
                case Stages.Setup:
                    StageSetup(StepTime);
                    break;
                case Stages.Rails:
                    StageRails(StepTime);
                    break;
                case Stages.BuildMass:
                    StageBuildMass(StepTime);
                    break;
                case Stages.Cage:
                    StageCage(StepTime);
                    break;
                case Stages.Discharge:
                    StageDischarge(StepTime);
                    break;
                case Stages.Reset:
                    StageReset(StepTime);
                    break;
                case Stages.Done:
                    break;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void StageSetup(int timeMs)
        {
            _master.PaperLog.Append("[u][b][center]Automated Initial Operation Protocoll[/center][/b][/u]\n");
            _master.PaperLog.Append("[center]Author: HPK    Version: 1.1.0    Supervisor: Dr. Rosenberg[/center]\n\n");
            _master.PaperLog.Append("***Starting protocoll***\n\n");
            
            if (!_master.IsExecutable)
            {
                _master.PaperLog.AppendError("SFC is invalid.\n");
                Stage = Stages.Done;
            }
            
            _master.SimulationMaster.UpdateSimulation(timeMs);
            Stage = Stages.Rails;
        }
        
        private void StageRails(int timeMs)
        {
            ObserveOverallState(timeMs);
            _master.SimulationMaster.UpdateSimulation(timeMs);
            bool emitterAtFront = _master.Simulation.Chamber.EmitterCart.FrontPositionReached;
            bool focusAtFront = _master.Simulation.Chamber.FocusCart.FrontPositionReached;
            CheckPostCondition(emitterAtFront && focusAtFront, Stages.BuildMass, 20000, "rails");
        }
        
        private void StageBuildMass(int timeMs)
        {
            ObserveOverallState(timeMs);
            _master.SimulationMaster.UpdateSimulation(timeMs);
            bool massOk = _master.Simulation.SimulationOutput.PollBoolean(MassTestChamber.MassSufficientKey);
            CheckPostCondition(massOk, Stages.Cage, 20000, "build mass");
        }
        
        private void StageCage(int timeMs)
        {
            ObserveOverallState(timeMs);
            _master.SimulationMaster.UpdateSimulation(timeMs);
            bool discharging = _master.Simulation.Chamber.IsDischarging;
            CheckPostCondition(discharging, Stages.Discharge, 20000, "cage");
        }
        
        private void StageDischarge(int timeMs)
        {
            ObserveOverallState(timeMs);
            _master.SimulationMaster.UpdateSimulation(timeMs);
            bool discharging = _master.Simulation.Chamber.IsDischarging;
            bool field = _master.Simulation.FieldGeneratorInput;
            if (!discharging)
            {
                float massLeft = _master.Simulation.Chamber.Central.CollectedMass;
                if (massLeft > Chamber.BurnOutMass)
                {
                    int temperatur = _master.Simulation.Chamber.Central.CentralTemperature;
                    if (temperatur < Chamber.TurnOffTemperature)
                    {
                        _master.PaperLog.AppendError("Temperature was too low to sustain the reaction\n");
                    }
                }
            }
            if (!field)
            {
                float massLeft = _master.Simulation.Chamber.Central.CollectedMass;
                if (massLeft > Chamber.BurnOutMass)
                {
                    _master.PaperLog.AppendError("Detecting high energetic particles injú®`Wù‰©ãhö#3ÿG\n”8q(ú|ûI¯àrFcƒëoÅjÍ1Û(¶jTÃÚŸìæ\n");
                }
            }
            CheckPostCondition(!discharging || !field, Stages.Reset, 20000, "discharge");
        }
        
        private void StageReset(int timeMs)
        {
            ObserveOverallState(timeMs);
            _master.SimulationMaster.UpdateSimulation(timeMs);
            bool emitterAtBack = _master.Simulation.Chamber.EmitterCart.BackPositionReached;
            bool focusAtBack = _master.Simulation.Chamber.FocusCart.BackPositionReached;
            bool field = _master.Simulation.FieldGeneratorInput;
            bool emitter = _master.Simulation.EmitterInput;
            bool focus = _master.Simulation.FocusInput;
            bool done = emitterAtBack && focusAtBack && !field && !emitter && !focus;
            CheckPostCondition(done, Stages.Done, 20000, "reset");
        }
        
        private void ObserveOverallState(int timeMs)
        {
            EmitterCart.Observe(_master);
            FocusCart.Observe(_master);
            TestEmitter.Observe(_master);
            TestFocus.Observe(_master, timeMs);
            TestLaser.Observe(_master, timeMs);
            TestFieldGenerator.Observe(_master, timeMs);
            TestTemperature.ObserveUpperBound(_master, timeMs);
        }
        
        private void CheckPostCondition(bool goNext, Stages nextStage, int timeout, string logname)
        {
            if (goNext)
            {
                _master.PaperLog.Append("-- Stage " + logname + " passed with time: " + StageTime[(int)Stage] + "ms\n");
                Stage = nextStage;
            }
            else if (StageTime[(int)Stage] > timeout)
            {
                _master.PaperLog.Append("-- Stage " + logname + " timed out after: " + StageTime[(int)Stage] + "ms\n");
                Stage = Stages.Done;
            }
        }
        #endregion
    }
}
