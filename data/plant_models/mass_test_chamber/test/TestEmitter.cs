namespace Osls.Plants.MassTestChamber
{
    public class TestEmitter
    {
        #region ==================== Fields / Properties ====================
        public bool ReportedLostParticles { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Observes the cart and reports damage
        /// </summary>
        public void Observe(MassTestChamberTest master)
        {
            if (master.Simulation.EmitterInput)
            {
                if (master.Simulation.FocusInput && master.Simulation.Chamber.EmitterCart.FrontPositionReached && !master.Simulation.FieldGeneratorInput)
                {
                }
                else
                {
                    if (!ReportedLostParticles)
                    {
                        master.PaperLog.AppendWarning("Detecting roaming particles in the test chamber\n");
                        ReportedLostParticles = true;
                    }
                }
            }
        }
        #endregion
    }
}