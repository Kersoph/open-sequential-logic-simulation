namespace Osls.Plants.MassTestChamber
{
    public class TankTest
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
            if (master.Simulation.PressureSignal > 2200 && !Overcharged)
            {
                master.PaperLog.AppendError("The pressure in the tank is too hig!\n");
                Overcharged = true;
            }
            if (master.Simulation.ChargingSignal && !interferenceAllowed && !WrongTime)
            {
                master.PaperLog.AppendWarning("Detecting illegal access to the compressor.\n");
                WrongTime = true;
            }
        }
        #endregion
    }
}
