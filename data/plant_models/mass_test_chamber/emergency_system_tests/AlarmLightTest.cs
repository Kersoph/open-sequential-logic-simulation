namespace Osls.Plants.MassTestChamber
{
    public class AlarmLightTest
    {
        #region ==================== Fields / Properties ====================
        public bool ReportedEarlyActivation { get; private set; }
        public bool ReportedEarlyDeactivation { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called by the stage test to check the actor status
        /// </summary>
        public void Check(EmergencySystemTest master, bool exceptedState)
        {
            if (master.Simulation.AlarmLightSignal != exceptedState)
            {
                if (exceptedState && !ReportedEarlyDeactivation)
                {
                    master.PaperLog.AppendError("The alarm light should be active.\n");
                    ReportedEarlyDeactivation = true;
                }
                else if (!exceptedState && !ReportedEarlyActivation)
                {
                    master.PaperLog.AppendError("The alarm light is in the wrong state.\n");
                    ReportedEarlyActivation = true;
                }
            }
        }
        #endregion
    }
}
