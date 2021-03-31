using Godot;
using System.Collections.Generic;

namespace SfcSandbox.Data.Model.SfcEditor.Interpreter
{
    public static class ActionMaster
    {
        #region ==================== Fields Properties ====================
        private static Color BooleanInputColor   = Color.Color8(0, 150, 0, 255);
        private static Color BooleanOutputdColor = Color.Color8(50, 180, 0, 255);
        private static Color IntegerInputColor   = Color.Color8(50, 50, 255, 255);
        
        private const string AssignmentSymbol = "=";
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Fills in the defined color keys of the opened simulation
        /// </summary>
        public static void UpdateColorKeys(TextEdit textEdit)
        {
            textEdit.ClearColors();
            List<string> booleanInputKeys = PlantViewNode.LoadedSimulationNode.SimulationOutput.BooleanKeys;
            foreach (string key in booleanInputKeys)
            {
                textEdit.AddKeywordColor(key, BooleanInputColor);
            }
            foreach (string key in Boolean.Constant.Values)
            {
                textEdit.AddKeywordColor(key, BooleanInputColor);
            }
            List<string> booleanOutputKeys = PlantViewNode.LoadedSimulationNode.SimulationInput.BooleanKeys;
            foreach (string key in booleanOutputKeys)
            {
                textEdit.AddKeywordColor(key, BooleanOutputdColor);
            }
            
            List<string> IntegerInputKeys = PlantViewNode.LoadedSimulationNode.SimulationOutput.IntegerKeys;
            foreach (string key in IntegerInputKeys)
            {
                textEdit.AddKeywordColor(key, IntegerInputColor);
            }
            // Godot already forces integer static inputs and colors them. (They can not be deleted so far)
            textEdit.AddColorOverride("number_color", IntegerInputColor);
            List<string> IntegerOutputKeys = PlantViewNode.LoadedSimulationNode.SimulationInput.IntegerKeys;
            foreach (string key in IntegerOutputKeys)
            {
                textEdit.AddKeywordColor(key, IntegerInputColor);
            }
        }
        
        /// <summary>
        /// Converts the string to a logical model.
        /// </summary>
        public static Assignment.AssignmentExpression InterpretTransitionText(string transition)
        {
            string[] words = transition.Split(" ");
            Data data = new Data(words);
            return InterpretAssignmentExpression(data);
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        /// <summary>
        /// Interprets the given words into a logical model.
        /// We follow a fixed y = x format according the requirements.
        /// </summary>
        private static Assignment.AssignmentExpression InterpretAssignmentExpression(Data data)
        {
            string targetWord = data.GetNext();
            if (data.IsEndRechaed) return null;
            string assignmentSymbol = data.GetNext();
            if (assignmentSymbol != AssignmentSymbol || data.IsEndRechaed) return null;
            string sourceName = data.GetNext();
            if (PlantViewNode.LoadedSimulationNode.SimulationInput.ContainsBoolean(targetWord))
            {
                Boolean.BooleanExpression sourceExpression = InterpretBoolean(sourceName);
                return new Assignment.Boolean(targetWord, sourceExpression);
            }
            else if (PlantViewNode.LoadedSimulationNode.SimulationInput.ContainsInteger(targetWord))
            {
                Numerical.NumericalExpression sourceExpression = InterpretNumerical(sourceName);
                return new Assignment.Numerical(targetWord, sourceExpression);
            }
            return null; // not valid
        }
        
        private static Boolean.BooleanExpression InterpretBoolean(string word)
        {
            if (Boolean.Constant.Values.Contains(word)) return new Boolean.Constant(word);
            return new Boolean.PlantReference(word);
        }
        
        private static Numerical.NumericalExpression InterpretNumerical(string word)
        {
            int number = 0;
            if (int.TryParse(word, out number)) return new Numerical.Constant(number);
            return new Numerical.PlantReference(word);
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
            public bool IsEndRechaed { get { return Position == Words.Length; } }
        }
        #endregion
    }
}