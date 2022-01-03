namespace Osls
{
    public interface IDiagramSimulationMaster
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Is called to calculate the next steps of the simulation with the given time in ms.
        /// We update first the simulation and then the controller.
        /// </summary>
        void UpdateSimulation(int deltaTimeMs);
        
        /// <summary>
        /// Returns true if the simulation can be executed.
        /// </summary>
        bool IsProgramSimulationValid();
        
        /// <summary>
        /// Resets the controller like there was a blackout.
        /// </summary>
        void Reset();
        #endregion
    }
}
