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
        
        /// <summary>
        /// Links the integer variables
        /// </summary>
        public IEnumerable<string> IntVariables { get { return IntLookup.Keys; } }
        
        /// <summary>
        /// The mocked internal int variables
        /// </summary>
        public Dictionary<string, int> IntLookup { get; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public ProcessingUnitMock()
        {
            InputRegisters = new StateTable(new List<StateEntry<bool>>(), new List<StateEntry<int>>());
            OutputRegisters = new StateTable(new List<StateEntry<bool>>(), new List<StateEntry<int>>());
            IntLookup = new Dictionary<string, int>();
        }
        
        public ProcessingUnitMock(StateTable inputs, StateTable outputs)
        {
            InputRegisters = inputs;
            OutputRegisters = outputs;
            IntLookup = new Dictionary<string, int>();
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
        public bool HasIntVariable(string key) { return IntLookup.ContainsKey(key); }
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        public int LookupIntVariable(string key) { return IntLookup[key]; }
        
        /// <summary>
        /// Returns true if there is a internal variable with this key
        /// </summary>
        public bool HasBoolVariable(string key) { return false; }
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        public bool LookupBoolVariable(string key) { return false; }
        #endregion
    }
}