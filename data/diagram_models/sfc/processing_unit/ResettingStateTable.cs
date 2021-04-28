using System.Collections.Generic;


namespace Osls.SfcSimulation.Engine
{
    public class ResettingStateTable : StateTable
    {
        #region ==================== Constructor ====================
        public ResettingStateTable(List<StateEntry<bool>> booleans, List<StateEntry<int>> integers)
        : base(booleans, integers)
        {
        }
        
        /// <summary>
        /// Initialises a resetting state table from the booleans and integers of the given one
        /// </summary>
        public ResettingStateTable(StateTable other)
        : base(other)
        {
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Resets the registers to the default value
        /// </summary>
        public void ResetRegisters()
        {
            foreach (string entry in BooleanKeys)
            {
                Booleans[entry].Value = Booleans[entry].DefaultValue;
            }
            foreach (string entry in IntegerKeys)
            {
                Integers[entry].Value = Integers[entry].DefaultValue;
            }
        }
        #endregion
    }
}