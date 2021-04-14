namespace Osls.St.Boolean
{
    /// <summary>
    /// Used to link boolean outputs of the plant to the transition
    /// </summary>
    public class PlantReference : BooleanExpression
    {
        #region ==================== Fields Properties ====================
        private readonly string _key;
        private readonly bool _valid;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Holds a reference to a boolean plant output
        /// </summary>
        public PlantReference(string key, IProcessingData data)
        {
            _key = key;
            _valid = data.InputRegisters.ContainsBoolean(_key);
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override bool Result(IProcessingUnit pu)
        {
            return pu.InputRegisters.PollBoolean(_key);
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return _valid;
        }
        
        public override string ToString()
        {
            return _key + (IsValid() ? "" : "?");
        }
        #endregion
    }
}