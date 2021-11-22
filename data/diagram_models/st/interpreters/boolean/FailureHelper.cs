namespace Osls.St.Boolean
{
    /// <summary>
    /// Groups the result of an boolean expression
    /// </summary>
    public class FailureHelper : BooleanExpression
    {
        #region ==================== Fields / Properties ====================
        private readonly BooleanExpression _target;
        private readonly string _trail;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Accepts the part that could be interpreted and shows the trail as an error
        /// </summary>
        public FailureHelper(BooleanExpression target, string trail)
        {
            _target = target;
            _trail = trail;
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override bool Result(IProcessingUnit pu)
        {
            return _target.Result(pu);
        }
        
        /// <summary>
        /// Returns false
        /// </summary>
        public override bool IsValid()
        {
            return false;
        }
        
        public override string ToString()
        {
            string targetString = _target != null ? _target.ToString() : "???";
            return targetString + "_?" + _trail;
        }
        #endregion
    }
}
