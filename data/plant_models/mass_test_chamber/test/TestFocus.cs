namespace Osls.Plants.MassTestChamber
{
    public class TestFocus
    {
        #region ==================== Fields / Properties ====================
        private int _unusualActivationTime;
        
        public int ExposedLanceTime { get; private set; }
        public bool ReportedUnusualActivation { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        #endregion
        
        
        #region ==================== Public Methods ====================
        public void Observe(Test master, int timeMs)
        {
            if (master.Simulation.FocusInput)
            {
                if (!master.Simulation.Chamber.FocusCart.FrontPositionReached || master.Simulation.FieldGeneratorInput)
                {
                    _unusualActivationTime += timeMs;
                    if (_unusualActivationTime > 100 && !ReportedUnusualActivation)
                    {
                        master.PaperLog.Append("Focus reports unusual long activation time\n");
                        ReportedUnusualActivation = true;
                    }
                }
            }
            if (master.Simulation.FieldGeneratorInput && master.Simulation.Chamber.Central.CollectedMass > 0.1f)
            {
                ExposedLanceTime += timeMs;
            }
        }
        #endregion
    }
}