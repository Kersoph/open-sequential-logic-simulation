using System.Collections.Generic;
using System.Linq;

namespace SfcSandbox.Data.Model.SfcSimulation
{
    /// <summary>
    /// Represents the inputs to the plant.
    /// It is updated by the SFC.
    /// </summary>
    public class InputTable
    {
        #region ==================== Fields ====================
        private readonly Dictionary<string, bool> _booleans;
        private readonly Dictionary<string, int> _integers;
        #endregion
        
        
        #region ==================== Properties ====================
        public List<string> BooleanKeys { get { return _booleans.Keys.ToList(); } }
        public List<string> IntegerKeys { get { return _integers.Keys.ToList(); } }
        #endregion
        
        
        #region ==================== Constructor ====================
        public InputTable(Dictionary<string, bool> booleans, Dictionary<string, int> integers)
        {
            _booleans = booleans;
            _integers = integers;
        }
        #endregion
        
        
        #region ==================== Constructor ====================
        /// <summary>
        /// Looks up the key in the plant output description
        /// </summary>
        public bool ContainsBoolean(string key)
        {
            return _booleans.ContainsKey(key);
        }
        
        /// <summary>
        /// Looks up the key in the plant output description
        /// </summary>
        public bool ContainsInteger(string key)
        {
            return _integers.ContainsKey(key);
        }
        
        /// <summary>
        /// Tries to get the value of the input
        /// </summary>
        public bool PollBooleanInput(string key)
        {
            bool value = false;
            _booleans.TryGetValue(key, out value);
            return value;
        }
        
        /// <summary>
        /// Tries to get the value of the input
        /// </summary>
        public int PollIntegerInput(string key)
        {
            int value = 0;
            _integers.TryGetValue(key, out value);
            return value;
        }
        
        /// <summary>
        /// Copies the whole output table to our input table
        /// </summary>
        public void AssignValuesFrom(OutputTable outputs)
        {
            List<string> booleanKeys = BooleanKeys;
            List<string> integerKeys = IntegerKeys;
            foreach (var entry in booleanKeys)
            {
                _booleans[entry] = outputs.PollBooleanOutput(entry);
            }
            foreach (var entry in integerKeys)
            {
                _integers[entry] = outputs.PollIntegerOutput(entry);
            }
        }
        
        /// <summary>
        /// Used to synchronize the tables between the PLC and the plant
        /// </summary>
        internal OutputTable CloneToOutputTable()
        {
            return new OutputTable(new Dictionary<string, bool>(_booleans), new Dictionary<string, int>(_integers));
        }
        #endregion
    }
}