namespace Osls
{
    public class StateEntry<T> where T : struct
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// The key used for the I/O Table
        /// </summary>
        public string Key { get; }
        
        /// <summary>
        /// The state value
        /// </summary>
        public T Value { get; set; }
        
        /// <summary>
        /// The state default value
        /// </summary>
        public T DefaultValue { get; }
        
        /// <summary>
        /// The short description of this entry provided by the boundary description
        /// </summary>
        public string Description { get; }
        
        /// <summary>
        /// The long description of this entry provided by the boundary description
        /// </summary>
        public string Hint { get; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public StateEntry(string key, T defaultValue, string description, string hint)
        {
            Key = key;
            DefaultValue = defaultValue;
            Value = defaultValue;
            Description = description;
            Hint = hint;
        }
        #endregion
    }
}