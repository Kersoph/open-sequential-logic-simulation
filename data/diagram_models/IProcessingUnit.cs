namespace Osls
{
    public interface IProcessingUnit : IProcessingData
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called to initialize the PU
        /// </summary>
        void Startup();
        
        /// <summary>
        /// Is called every simulation step to update the PU.
        /// </summary>
        void Update(int deltaTimeMs);
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        bool IsLogicValid();
        #endregion
    }
}
