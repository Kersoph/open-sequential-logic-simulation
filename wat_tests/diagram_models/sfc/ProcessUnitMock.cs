using Osls;
using System.Collections.Generic;


namespace Tests.SfcEditor.Interpreters
{
    public class ProcessingUnitMock : IProcessingUnit
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// Holds the input registers of the processing unit
        /// </summary>
        public StateTable InputRegisters { get; }
        
        /// <summary>
        /// Holds the output registers of the processing unit
        /// </summary>
        public StateTable OutputRegisters { get; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public ProcessingUnitMock()
        {
            InputRegisters = new StateTable(new Dictionary<string, bool>(), new Dictionary<string, int>());
            OutputRegisters = new StateTable(new Dictionary<string, bool>(), new Dictionary<string, int>());
        }
        
        /// <summary>
        /// Called to initialize the PU
        /// </summary>
        public void Startup() { }
        
        /// <summary>
        /// Is called every simulation step to update the PU.
        /// </summary>
        public void Update(int deltaTimeMs) { }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsLogicValid() { return true; }
        
        /// <summary>
        /// Returns true if there is a internal variable with this key
        /// </summary>
        public bool HasIntVariable(string key) { return false; }
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        public int LookupIntVariable(string key) { return 0; }
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        public bool LookupBoolVariable(string key) { return false; }
        #endregion
    }
}