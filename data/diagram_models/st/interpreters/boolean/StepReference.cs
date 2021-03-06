namespace Osls.St.Boolean
{
    /// <summary>
    /// Used to link integer outputs of the step to the transition
    /// </summary>
    public class StepReference : BooleanExpression
    {
        #region ==================== Fields / Properties ====================
        private readonly string _key;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Holds a reference to a step boolean output
        /// </summary>
        public StepReference(string key)
        {
            _key = key;
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override bool Result(IProcessingUnit pu)
        {
            return pu.LookupBoolVariable(_key);
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return true;
        }
        
        public override string ToString()
        {
            return _key + (IsValid() ? "" : "?");
        }
        #endregion
    }
}
