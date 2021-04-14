using Godot;
using System.Collections.Generic;


namespace Osls.SfcEditor.Interpreters
{
    public static class ActionMaster
    {
        #region ==================== Fields Properties ====================
        private static Color BooleanInputColor   = Color.Color8(0, 150, 0, 255);
        private static Color BooleanOutputColor = Color.Color8(50, 180, 0, 255);
        private static Color IntegerInputColor   = Color.Color8(50, 50, 255, 255);
        
        private const string AssignmentSymbol = "=";
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
            foreach (string key in Boolean.Constant.Values)
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
        public static Assignment.AssignmentExpression InterpretTransitionText(string transition, IProcessingData processingData)
        {
            string[] words = transition.Split(" ");
            Data data = new Data(words);
            return InterpretAssignmentExpression(data, processingData);
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        /// <summary>
        /// Interprets the given words into a logical model.
        /// We follow a fixed y = x format according the requirements.
        /// </summary>
        private static Assignment.AssignmentExpression InterpretAssignmentExpression(Data data, IProcessingData context)
        {
            string targetWord = data.GetNext();
            if (data.IsEndReached) return null;
            string assignmentSymbol = data.GetNext();
            if (assignmentSymbol != AssignmentSymbol || data.IsEndReached) return null;
            string sourceName = data.GetNext();
            if (context.OutputRegisters.ContainsBoolean(targetWord))
            {
                Boolean.BooleanExpression sourceExpression = InterpretBoolean(sourceName, context);
                return new Assignment.Boolean(targetWord, sourceExpression, context);
            }
            else if (context.OutputRegisters.ContainsInteger(targetWord))
            {
                Numerical.NumericalExpression sourceExpression = InterpretNumerical(sourceName, context);
                return new Assignment.Numerical(targetWord, sourceExpression, context);
            }
            return null; // not valid
        }
        
        private static Boolean.BooleanExpression InterpretBoolean(string word, IProcessingData context)
        {
            if (Boolean.Constant.Values.Contains(word)) return new Boolean.Constant(word);
            return new Boolean.PlantReference(word, context);
        }
        
        private static Numerical.NumericalExpression InterpretNumerical(string word, IProcessingData context)
        {
            if (int.TryParse(word, out int number)) return new Numerical.Constant(number);
            return new Numerical.PlantReference(word, context);
        }
        #endregion
        
        
        #region ==================== Nested Class ====================
        private class Data
        {
            public Data(string[] words)
            {
                Words = words;
                Position = 0;
            }
            public string[] Words { get; set; }
            public int Position { get; set; }
            public string GetNext()
            {
                string word = Words[Position];
                Position++;
                return word;
            }
            public bool IsEndReached { get { return Position == Words.Length; } }
        }
        #endregion
    }
}