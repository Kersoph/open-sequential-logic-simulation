using Godot;
using System.Collections.Generic;
using Osls.St.Assignment;


namespace Osls.SfcEditor.Interpreters
{
    public static class ActionMaster
    {
        #region ==================== Fields / Properties ====================
        private static Color BooleanInputColor   = Color.Color8(0, 150, 0, 255);
        private static Color BooleanOutputColor = Color.Color8(50, 180, 0, 255);
        private static Color IntegerInputColor   = Color.Color8(50, 50, 255, 255);
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Fills in the defined color keys of the opened simulation
        /// </summary>
        public static void UpdateColorKeys(TextEdit textEdit, IProcessingData processingData)
        {
            textEdit.ClearColors();
            List<string> booleanInputKeys = processingData.InputRegisters.BooleanKeys;
            foreach (string key in booleanInputKeys)
            {
                textEdit.AddKeywordColor(key, BooleanInputColor);
            }
            foreach (string key in St.Boolean.Constant.Values)
            {
                textEdit.AddKeywordColor(key, BooleanInputColor);
            }
            List<string> booleanOutputKeys = processingData.OutputRegisters.BooleanKeys;
            foreach (string key in booleanOutputKeys)
            {
                textEdit.AddKeywordColor(key, BooleanOutputColor);
            }
            
            List<string> IntegerInputKeys = processingData.InputRegisters.IntegerKeys;
            foreach (string key in IntegerInputKeys)
            {
                textEdit.AddKeywordColor(key, IntegerInputColor);
            }
            // Godot already forces integer static inputs and colors them. (They can not be deleted so far)
            textEdit.AddColorOverride("number_color", IntegerInputColor);
            List<string> IntegerOutputKeys = processingData.OutputRegisters.IntegerKeys;
            foreach (string key in IntegerOutputKeys)
            {
                textEdit.AddKeywordColor(key, IntegerInputColor);
            }
        }
        
        /// <summary>
        /// Converts the string to a logical model.
        /// </summary>
        public static AssignmentExpression InterpretTransitionText(string transition, IProcessingData processingData)
        {
            return Interpreter.AsAssignmentExpression(transition, processingData);
        }
        #endregion
    }
}
