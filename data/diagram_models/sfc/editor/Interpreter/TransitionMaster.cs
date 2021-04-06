using Godot;
using System.Collections.Generic;

namespace Osls.SfcEditor.Interpreter
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
        public static void UpdateColorKeys(TextEdit textEdit, StepMaster stepMaster)
        {
            textEdit.ClearColors();
            List<string> booleanKeys = PlantViewNode.LoadedSimulationNode.SimulationOutput.BooleanKeys;
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
            List<string> IntegerKeys = PlantViewNode.LoadedSimulationNode.SimulationOutput.IntegerKeys;
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
            foreach (string key in stepMaster.PatchNameMap.Keys)
            {
                textEdit.AddKeywordColor(key, StepInputColor);
            }
        }
        
        /// <summary>
        /// Converts the string to a logical model.
        /// </summary>
        public static Boolean.BooleanExpression InterpretTransitionText(string transition, StepMaster stepMaster)
        {
            string[] words = transition.Split(" ");
            Data data = new Data(words);
            Boolean.BooleanExpression mainExpression = InterpretBooleanExpression(data, stepMaster);
            return mainExpression;
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        /// <summary>
        /// Interprets the given words into a logical model.
        /// As there are different possible approaches, we choose a left to right packing method to provide
        /// a readable debug string for the user.
        /// </summary>
        private static Boolean.BooleanExpression InterpretBooleanExpression(Data data, StepMaster stepMaster)
        {
            if(data.IsEndRechaed) return null;
            string currentWord = data.GetNext();
            
            Boolean.BooleanExpression currentExpression = null;
            // B -> I B
            if (Boolean.LogicalInverter.Values.Contains(currentWord))
            {
                Boolean.BooleanExpression nextExpression = InterpretBooleanExpression(data, stepMaster);
                currentExpression = new Boolean.LogicalInverter(nextExpression);
                if(data.IsEndRechaed) return currentExpression;
                currentWord = data.GetNext();
            }
            // B -> b
            if (IsRepresentingBoolean(currentWord))
            {
                currentExpression = InterpretBoolean(currentWord);
                if(data.IsEndRechaed) return currentExpression;
                currentWord = data.GetNext();
            }
            // B -> N V N
            else if (IsRepresentingNumerical(currentWord, stepMaster))
            {
                // N -> n
                Numerical.NumericalExpression leftNumber = InterpretNumerical(currentWord, stepMaster);
                if (data.IsEndRechaed) return null;
                string relation = data.GetNext();
                // V -> v
                if (!Boolean.RelationalOperation.Values.Contains(relation)) return null;
                if (data.IsEndRechaed) return null;
                currentWord = data.GetNext();
                // N -> n
                if (!IsRepresentingNumerical(currentWord, stepMaster)) return null;
                Numerical.NumericalExpression rightNumber = InterpretNumerical(currentWord, stepMaster);
                currentExpression = new Boolean.RelationalOperation(relation, leftNumber, rightNumber);
                if (data.IsEndRechaed) return currentExpression;
                currentWord = data.GetNext();
            }
            // B -> B E B
            if (Boolean.LogicalCombination.Values.Contains(currentWord))
            {
                Boolean.BooleanExpression nextExpression = InterpretBooleanExpression(data, stepMaster);
                return new Boolean.LogicalCombination(currentWord, currentExpression, nextExpression);
            }
            return currentExpression; // Partial failure
        }
        
        private static bool IsRepresentingBoolean(string word)
        {
            return Boolean.Constant.Values.Contains(word)
            || PlantViewNode.LoadedSimulationNode.SimulationOutput.ContainsBoolean(word);
        }
        
        private static Boolean.BooleanExpression InterpretBoolean(string word)
        {
            if (Boolean.Constant.Values.Contains(word)) return new Boolean.Constant(word);
            return new Boolean.PlantReference(word);
        }
        
        private static bool IsRepresentingNumerical(string word, StepMaster stepMaster)
        {
            int number;
            return int.TryParse(word, out number)
            || PlantViewNode.LoadedSimulationNode.SimulationOutput.ContainsInteger(word)
            || stepMaster.ContainsStepTime(word);
        }
        
        private static Numerical.NumericalExpression InterpretNumerical(string word, StepMaster stepMaster)
        {
            int number = 0;
            if (int.TryParse(word, out number)) return new Numerical.Constant(number);
            if (stepMaster.ContainsStepTime(word)) return new Numerical.StepReference(word);
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