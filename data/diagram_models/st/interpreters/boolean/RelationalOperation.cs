using System.Collections.Generic;
using Osls.St.Numerical;


namespace Osls.St.Boolean
{
    /// <summary>
    /// Preforms an relational operation on the given data
    /// </summary>
    public class RelationalOperation : BooleanExpression
    {
        #region ==================== Fields Properties ====================
        public const string GreaterThan = ">";
        public const string SmallerThan = "<";
        public const string GreaterEqual = ">=";
        public const string SmallerEqual = "<=";
        public const string Equal = "==";
        public static HashSet<string> Values = new HashSet<string>() { GreaterThan, SmallerThan, GreaterEqual, SmallerEqual, Equal };
        
        private readonly string _operator;
        private readonly NumericalExpression _left;
        private readonly NumericalExpression _right;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Holds a true or false constant
        /// </summary>
        public RelationalOperation(string value, NumericalExpression left, NumericalExpression right)
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
                case GreaterThan:
                    return _left.Result(pu) > _right.Result(pu);
                case SmallerThan:
                    return _left.Result(pu) < _right.Result(pu);
                case GreaterEqual:
                    return _left.Result(pu) >= _right.Result(pu);
                case SmallerEqual:
                    return _left.Result(pu) <= _right.Result(pu);
                case Equal:
                    return _left.Result(pu) == _right.Result(pu);
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