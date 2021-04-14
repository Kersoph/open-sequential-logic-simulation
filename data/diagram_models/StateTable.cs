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
        protected readonly Dictionary<string, bool> Booleans;
        protected readonly Dictionary<string, int> Integers;
        #endregion
        
        
        #region ==================== Properties ====================
        public List<string> BooleanKeys { get; private set; }
        public List<string> IntegerKeys { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public StateTable(Dictionary<string, bool> booleans, Dictionary<string, int> integers)
        {
            Booleans = booleans;
            Integers = integers;
            BooleanKeys = Booleans.Keys.ToList();
            IntegerKeys = Integers.Keys.ToList();
        }
        
        /// <summary>
        /// Initialises a state table from the booleans and integers of the given one
        /// </summary>
        public StateTable(StateTable other)
        {
            Booleans = new Dictionary<string, bool>(other.Booleans);
            Integers = new Dictionary<string, int>(other.Integers);
            BooleanKeys = Booleans.Keys.ToList();
            IntegerKeys = Integers.Keys.ToList();
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Looks up the key in the plant output description
        /// </summary>
        public bool ContainsBoolean(string key)
        {
            return Booleans.ContainsKey(key);
        }
        
        /// <summary>
        /// Looks up the key in the plant output description
        /// </summary>
        public bool ContainsInteger(string key)
        {
            return Integers.ContainsKey(key);
        }
        
        /// <summary>
        /// Tries to get the value of the key
        /// </summary>
        public bool PollBoolean(string key)
        {
            Booleans.TryGetValue(key, out bool value);
            return value;
        }
        
        /// <summary>
        /// Tries to get the value of the key
        /// </summary>
        public int PollInteger(string key)
        {
            Integers.TryGetValue(key, out int value);
            return value;
        }
        
        /// <summary>
        /// Sets a value by the given key.
        /// </summary>
        public void SetValue(string key, bool value)
        {
            Booleans[key] = value;
        }
        
        /// <summary>
        /// Sets a value by the given key.
        /// </summary>
        public void SetValue(string key, int value)
        {
            Integers[key] = value;
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
                Booleans[entry] = outputs.PollBoolean(entry);
            }
            foreach (var entry in integerKeys)
            {
                Integers[entry] = outputs.PollInteger(entry);
            }
        }
        #endregion
    }
}