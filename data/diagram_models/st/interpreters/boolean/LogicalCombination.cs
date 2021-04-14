using System.Collections.Generic;


namespace Osls.SfcEditor.Interpreters.Boolean
{
    /// <summary>
    /// Preforms an logical operation on the left and right expression
    /// </summary>
    public class LogicalCombination : BooleanExpression
    {
        #region ==================== Fields Properties ====================
        public const string AND = "and";
        public const string OR = "or";
        public static HashSet<string> Values = new HashSet<string>() { AND, OR };

        private readonly string _operator;
        private readonly BooleanExpression _left;
        private readonly BooleanExpression _right;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Holds a true or false constant
        /// </summary>
        public LogicalCombination(string value, BooleanExpression left, BooleanExpression right)
        {
            _left = left;
            _right = right;
            _operator = value;
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override bool Result(IProcessingUnit pu)
        {
            switch (_operator)
            {
                case AND:
                    return _left.Result(pu) && _right.Result(pu);
                case OR:
                    return _left.Result(pu) || _right.Result(pu);
            }
            Godot.GD.PushError("Unknown operator " + _operator);
            return false;
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return _left != null && _right != null && _left.IsValid() && _right.IsValid();
        }
        
        public override string ToString()
        {
            string leftString = _left != null ? _left.ToString() : "???";
            string rightString = _right != null ? _right.ToString() : "???";
            return leftString + " " + _operator + " " + rightString;
        }
        #endregion
    }
}