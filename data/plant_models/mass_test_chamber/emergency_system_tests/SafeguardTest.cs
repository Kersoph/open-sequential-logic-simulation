namespace Osls.Plants.MassTestChamber
{
    public class SafeguardTest
    {
        #region ==================== Fields / Properties ====================
        public bool WrongTime { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called by the stage test to check the actor status
        /// </summary>
        public void Check(EmergencySystemTest master, bool expected)
        {
            if (!WrongTime)
            {
                if (master.Simulation.SafeguardSignal && !expected)
                {
                    master.PaperLog.AppendWarning("The emergency state should be reset.\n");
                    WrongTime = true;
                }
                else if (!master.Simulation.SafeguardSignal && expected)
                {
                    master.PaperLog.AppendError("The emergency state was reset at the wrong time!\n");
                    WrongTime = true;
                }
            }
        }
        #endregion
    }
}
