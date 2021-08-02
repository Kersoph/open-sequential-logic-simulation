namespace Osls.Plants.MassTestChamber
{
    public class TestTemperature
    {
        #region ==================== Fields / Properties ====================
        public bool ReportedExceededTemperature { get; private set; }
        public int ExceededTemperatureTime { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        #endregion
        
        
        #region ==================== Public Methods ====================
        public void ObserveUpperBound(MassTestChamberTest master, int timeMs)
        {
            int temperature = master.Simulation.Chamber.Central.CentralTemperature;
            float centralMass = master.Simulation.Chamber.Central.CollectedMass;
            if (temperature > CentralParticles.MaxTemperature && centralMass > 0.01)
            {
                if (!ReportedExceededTemperature)
                {
                    ReportedExceededTemperature = true;
                    master.PaperLog.AppendWarning("Detected high temperatures in the core\n");
                }
                ExceededTemperatureTime += timeMs;
            }
        }
        #endregion
    }
}