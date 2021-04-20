namespace Osls.St.Boolean
{
    /// <summary>
    /// Groups the result of an boolean expression
    /// </summary>
    public class Grouping : BooleanExpression
    {
        #region ==================== Fields Properties ====================
        public const string Open = "(";
        public const string Close = ")";
        private readonly BooleanExpression _target;
        private bool _validClosing;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Groups the target expression. Set validClosing to false if there is no closing symbol
        /// </summary>
        public Grouping(BooleanExpression target, bool validClosing)
        {
            _target = target;
            _validClosing = validClosing;
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override bool Result(IProcessingUnit pu)
        {
            return _target.Result(pu);
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return _validClosing && _target != null && _target.IsValid();
        }
        
        public override string ToString()
        {
            string targetString = _target != null ? _target.ToString() : "???";
            string close = _validClosing ? Close : "?" + Close + "?";
            return Open + " " + targetString + " " + close;
        }
        #endregion
    }
}