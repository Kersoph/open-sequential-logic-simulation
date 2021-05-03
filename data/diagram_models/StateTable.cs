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
        protected readonly Dictionary<string, StateEntry<bool>> Booleans;
        protected readonly Dictionary<string, StateEntry<int>> Integers;
        #endregion
        
        
        #region ==================== Properties ====================
        /// <summary>
        /// The keys of the boolean entries of this table
        /// </summary>
        public List<string> BooleanKeys { get; private set; }
        
        /// <summary>
        /// The keys of the integer entries of this table
        /// </summary>
        public List<string> IntegerKeys { get; private set; }
        
        /// <summary>
        /// The boolean entries of the this table
        /// </summary>
        public IReadOnlyCollection<StateEntry<bool>> BooleanEntries { get { return Booleans.Values; } }
        
        /// <summary>
        /// The boolean entries of the this table
        /// </summary>
        public IReadOnlyCollection<StateEntry<int>> IntegerEntries { get { return Integers.Values; } }
        #endregion
        
        
        #region ==================== Constructor ====================
        public StateTable(List<StateEntry<bool>> booleans, List<StateEntry<int>> integers)
        {
            Booleans = new Dictionary<string, StateEntry<bool>>(booleans.Count);
            foreach (var entry in booleans)
            {
                Booleans.Add(entry.Key, entry);
            }
            Integers = new Dictionary<string, StateEntry<int>>(integers.Count);
            foreach (var entry in integers)
            {
                Integers.Add(entry.Key, entry);
            }
            BooleanKeys = Booleans.Keys.ToList();
            IntegerKeys = Integers.Keys.ToList();
        }
        
        /// <summary>
        /// Initialises a state table from the booleans and integers of the given one
        /// </summary>
        public StateTable(StateTable other)
        {
            Booleans = new Dictionary<string, StateEntry<bool>>(other.Booleans);
            Integers = new Dictionary<string, StateEntry<int>>(other.Integers);
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
            Booleans.TryGetValue(key, out StateEntry<bool> entry);
            return entry.Value;
        }
        
        /// <summary>
        /// Tries to get the value of the key
        /// </summary>
        public int PollInteger(string key)
        {
            Integers.TryGetValue(key, out StateEntry<int> entry);
            return entry.Value;
        }
        
        /// <summary>
        /// Sets a value by the given key.
        /// </summary>
        public void SetValue(string key, bool value)
        {
            Booleans[key].Value = value;
        }
        
        /// <summary>
        /// Sets a value by the given key.
        /// </summary>
        public void SetValue(string key, int value)
        {
            Integers[key].Value = value;
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
                Booleans[entry].Value = outputs.PollBoolean(entry);
            }
            foreach (var entry in integerKeys)
            {
                Integers[entry].Value = outputs.PollInteger(entry);
            }
        }
        #endregion
    }
}