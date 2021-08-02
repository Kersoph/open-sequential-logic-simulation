namespace Osls.Plants.MassTestChamber
{
    public class TestLaser
    {
        #region ==================== Fields / Properties ====================
        public int ActiveLaserTime { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        #endregion
        
        
        #region ==================== Public Methods ====================
        public void Observe(MassTestChamberTest master, int timeMs)
        {
            if (master.Simulation.LaserInput)
            {
                ActiveLaserTime += timeMs;
            }
        }
        #endregion
    }
}