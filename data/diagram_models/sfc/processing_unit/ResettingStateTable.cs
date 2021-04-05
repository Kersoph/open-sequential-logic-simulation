using System.Collections.Generic;


namespace Osls.SfcSimulation.Engine
{
    public class ResettingStateTable : StateTable
    {
        #region ==================== Constructor ====================
        public ResettingStateTable(Dictionary<string, bool> booleans, Dictionary<string, int> integers)
        : base(booleans, integers)
        {
        }
        
        /// <summary>
        /// Initialises a resettig state table from the booleans and integers of the given one
        /// </summary>
        public ResettingStateTable(StateTable other)
        : base(other)
        {
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
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
        /// Resets the registers to the default value
        /// </summary>
        public void ResetRegisters()
        {
            foreach (string entry in BooleanKeys)
            {
                Booleans[entry] = false;
            }
            foreach (string entry in IntegerKeys)
            {
                Integers[entry] = 0;
            }
        }
        #endregion
    }
}