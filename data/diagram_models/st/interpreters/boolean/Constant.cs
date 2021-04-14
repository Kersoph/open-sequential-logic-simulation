using System.Collections.Generic;


namespace Osls.St.Boolean
{
    /// <summary>
    /// Represents an boolean constant
    /// </summary>
    public class Constant : BooleanExpression
    {
        #region ==================== Fields Properties ====================
        public static string TRUE = "true";
        public static string FALSE = "false";
        public static HashSet<string> TrueSet = new HashSet<string>(){ "true", "True", "TRUE" };
        public static HashSet<string> FalseSet = new HashSet<string>(){ "false", "False", "FALSE" };
        public static HashSet<string> Values = Union(TrueSet, FalseSet);
        
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
        public override bool Result(IProcessingUnit pu)
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
            return _value ? TRUE : FALSE;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private static bool ConvertValue(string value)
        {
            if (TrueSet.Contains(value))
            {
                return true;
            }
            if (FalseSet.Contains(value))
            {
                return false;
            }
            Godot.GD.Print("Unknown value " + value);
            return false;
        }
        #endregion
    }
}