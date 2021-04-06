using System.Collections.Generic;

namespace Osls.SfcEditor.Interpreter.Boolean
{
    /// <summary>
    /// Represents an boolean constant
    /// </summary>
    public class Constant : BooleanExpression
    {
        #region ==================== Fields Properties ====================
        public const string TRUE = "true";
        public const string FASLE = "false";
        public static HashSet<string> Values = new HashSet<string>(){ TRUE, FASLE };
        
        private readonly bool _value;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Holds a true or false constant
        /// </summary>
        public Constant(string value)
        {
            _value = ConvertValue(value);
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override bool Result(SfcSimulation.Engine.SfcProgramm sfcProgramm)
        {
            return _value;
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
            return _value ? TRUE : FASLE;
        }
        #endregion
        
        
        #region ==================== Private ====================
        private static bool ConvertValue(string value)
        {
            switch (value)
            {
                case TRUE:
                    return true;
                case FASLE:
                    return false;
            }
            Godot.GD.Print("Unknown value " + value);
            return false;
        }
        #endregion
    }
}