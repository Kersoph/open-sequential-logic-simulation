using System.Collections.Generic;


namespace Osls.St.Boolean
{
    /// <summary>
    /// Inverts the result of an boolean expression
    /// </summary>
    public class LogicalInverter : BooleanExpression
    {
        #region ==================== Fields Properties ====================
        public const string NOT = "not";
        public static HashSet<string> Values = new HashSet<string>(){ NOT, "Not", "NOT", "!" };
        
        private readonly BooleanExpression _target;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Holds a true or false constant
        /// </summary>
        public LogicalInverter(BooleanExpression target)
        {
            _target = target;
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override bool Result(IProcessingUnit pu)
        {
            return !_target.Result(pu);
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return _target != null && _target.IsValid();
        }
        
        public override string ToString()
        {
            string targetString = _target != null ? _target.ToString() : "???";
            return NOT + " " + targetString;
        }
        #endregion
    }
}