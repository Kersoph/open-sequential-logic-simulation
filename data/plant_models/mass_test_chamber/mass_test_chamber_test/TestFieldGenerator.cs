namespace Osls.Plants.MassTestChamber
{
    public class TestFieldGenerator
    {
        #region ==================== Fields / Properties ====================
        public int ActiveTime { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        #endregion
        
        
        #region ==================== Public Methods ====================
        public void Observe(MassTestChamberTest master, int timeMs)
        {
            if (master.Simulation.FieldGeneratorInput)
            {
                ActiveTime += timeMs;
            }
        }
        #endregion
    }
}