namespace Osls.Models
{
    public interface IProcessingUnit
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// Holds the input registers of the processing unit
        /// </summary>
        int InputRegisters { get; }
        
        /// <summary>
        /// Holds the output registers of the processing unit
        /// </summary>
        int OutputRegisters { get; }
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
        #endregion
    }
}