using System.Collections.Generic;
using System.Linq;


namespace Osls
{
    /// <summary>
    /// Represents the inputs/outputs to the plant.
    /// It is updated by the processing unit.
    /// </summary>
    public class StateTable
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
        public StateTable(Dictionary<string, bool> booleans, Dictionary<string, int> integers)
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
        /// Tries to get the value of the key
        /// </summary>
        public bool PollBoolean(string key)
        {
            _booleans.TryGetValue(key, out bool value);
            return value;
        }
        
        /// <summary>
        /// Tries to get the value of the key
        /// </summary>
        public int PollInteger(string key)
        {
            _integers.TryGetValue(key, out int value);
            return value;
        }
        
        /// <summary>
        /// Copies the whole output table to our input table
        /// </summary>
        public void AssignValuesFrom(StateTable outputs)
        {
            List<string> booleanKeys = BooleanKeys;
            List<string> integerKeys = IntegerKeys;
            foreach (var entry in booleanKeys)
            {
                _booleans[entry] = outputs.PollBoolean(entry);
            }
            foreach (var entry in integerKeys)
            {
                _integers[entry] = outputs.PollInteger(entry);
            }
        }
        
        /// <summary>
        /// Used to synchronize the tables between the PLC and the plant
        /// </summary>
        internal StateTable CloneTable()
        {
            return new StateTable(new Dictionary<string, bool>(_booleans), new Dictionary<string, int>(_integers));
        }
        #endregion
    }
}