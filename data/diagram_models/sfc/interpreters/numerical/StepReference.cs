namespace Osls.SfcEditor.Interpreters.Numerical
{
    /// <summary>
    /// Used to link integer outputs of the step to the transition
    /// </summary>
    public class StepReference : NumericalExpression
    {
        #region ==================== Fields Properties ====================
        private readonly string _key;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Holds a reference to a step integer output
        /// </summary>
        public StepReference(string key)
        {
            _key = key;
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override int Result(IProcessingUnit pu)
        {
            return pu.LookupIntVariable(_key);
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