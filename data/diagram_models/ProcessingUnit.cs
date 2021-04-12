namespace Osls
{
    public interface IProcessingUnit
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// Holds the input registers of the processing unit
        /// </summary>
        StateTable InputRegisters { get; }
        
        /// <summary>
        /// Holds the output registers of the processing unit
        /// </summary>
        StateTable OutputRegisters { get; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called to initialize the PU
        /// </summary>
        void Startup();
        
        /// <summary>
        /// Is called every simulation step to update the PU.
        /// </summary>
        void Update();

        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        bool IsLogicValid();
        #endregion
    }
}