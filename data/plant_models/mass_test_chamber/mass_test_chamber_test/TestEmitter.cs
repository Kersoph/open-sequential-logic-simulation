namespace Osls.Plants.MassTestChamber
{
    public class TestEmitter
    {
        #region ==================== Fields / Properties ====================
        public bool ReportedLostParticles { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Observes the cart and reports damage
        /// </summary>
        public void Observe(MassTestChamberTest master)
        {
            if (master.Simulation.Chamber.Emitter.IsLeakingParticles && !ReportedLostParticles)
            {
                master.PaperLog.AppendWarning("Detecting roaming particles in the test chamber\n");
                ReportedLostParticles = true;
            }
        }
        #endregion
    }
}
