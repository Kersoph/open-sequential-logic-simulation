using Godot;
using System.Collections.Generic;


namespace Osls.SfcEditor.Interpreters
{
    public static class TransitionMaster
    {
        #region ==================== Fields Properties ====================
        private static Color BooleanInputColor   = Color.Color8(0, 150, 0, 255);
        private static Color BooleanCommandColor = Color.Color8(50, 180, 0, 255);
        private static Color IntegerInputColor   = Color.Color8(50, 50, 255, 255);
        private static Color IntegerCommandColor = Color.Color8(50, 150, 255, 255);
        private static Color StepInputColor      = Color.Color8(150, 100, 0, 255);
        #endregion
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Fills in the defined color keys of the opened simulation
        /// </summary>
        public static void UpdateColorKeys(TextEdit textEdit, IProcessingData data)
        {
            textEdit.ClearColors();
            List<string> booleanKeys = data.InputRegisters.BooleanKeys;
            foreach (string key in booleanKeys)
            {
                textEdit.AddKeywordColor(key, BooleanInputColor);
            }
            foreach (string key in Boolean.Constant.Values)
            {
                textEdit.AddKeywordColor(key, BooleanInputColor);
            }
            foreach (string key in Boolean.LogicalCombination.Values)
            {
                textEdit.AddKeywordColor(key, BooleanCommandColor);
            }
            foreach (string key in Boolean.LogicalInverter.Values)
            {
                textEdit.AddKeywordColor(key, BooleanCommandColor);
            }
            List<string> IntegerKeys = data.InputRegisters.IntegerKeys;
            foreach (string key in IntegerKeys)
            {
                textEdit.AddKeywordColor(key, IntegerInputColor);
            }
            // Godot already forces integer static inputs and colors them. (They can not be deleted so far)
            textEdit.AddColorOverride("symbol_color", IntegerCommandColor);
            textEdit.AddColorOverride("number_color", IntegerInputColor);
            foreach (string key in Boolean.RelationalOperation.Values)
            {
                textEdit.AddKeywordColor(key, IntegerCommandColor);
            }
            
            // Godot forces member variable color according to member_variable_color
            // Godot also forces the "class" color and access without any known possibility to change or override them.
            textEdit.AddColorOverride("member_variable_color", IntegerInputColor);
            foreach (string key in data.IntVariables)
            {
                textEdit.AddKeywordColor(key, StepInputColor);
            }
        }
        
        /// <summary>
        /// Converts the string to a logical model.
        /// </summary>
        public static Boolean.BooleanExpression InterpretTransitionText(string transition, IProcessingData context)
        {
            return Boolean.Interpreter.AsBooleanExpression(transition, context);
        }
        #endregion
    }
}