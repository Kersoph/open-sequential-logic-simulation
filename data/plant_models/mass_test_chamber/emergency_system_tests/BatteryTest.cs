namespace Osls.Plants.MassTestChamber
{
    public class BatteryTest
    {
        #region ==================== Fields / Properties ====================
        public bool Overcharged { get; private set; }
        public bool WrongTime { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called by the stage test to check the actor status
        /// </summary>
        public void Check(EmergencySystemTest master, bool interferenceAllowed = true)
        {
            if (master.Simulation.BatterySignal > 49 && !Overcharged)
            {
                master.PaperLog.AppendError("The batteries were overcharged.\n");
                Overcharged = true;
            }
            if (master.Simulation.ChargingSignal && !interferenceAllowed && !WrongTime)
            {
                master.PaperLog.AppendWarning("Detecting illegal access to the battery charging system.\n");
                WrongTime = true;
            }
        }
        #endregion
    }
}
