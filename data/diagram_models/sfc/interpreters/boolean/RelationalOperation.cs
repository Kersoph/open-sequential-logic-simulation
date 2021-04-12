using System.Collections.Generic;
using Osls.SfcEditor.Interpreter.Numerical;


namespace Osls.SfcEditor.Interpreter.Boolean
{
    /// <summary>
    /// Preforms an relational operation on the given data
    /// </summary>
    public class RelationalOperation : BooleanExpression
    {
        #region ==================== Fields Properties ====================
        public const string GreaterThan = ">";
        public const string SmallerThan = "<";
        public static HashSet<string> Values = new HashSet<string>() { GreaterThan, SmallerThan };
        
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
        public override bool Result(SfcSimulation.Engine.SfcProgram sfcProgram)
        {
            switch (_operator)
            {
                case GreaterThan:
                    return _left.Result(sfcProgram) > _right.Result(sfcProgram);
                case SmallerThan:
                    return _left.Result(sfcProgram) < _right.Result(sfcProgram);
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