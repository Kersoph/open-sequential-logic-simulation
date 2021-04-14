namespace Osls.St.Boolean
{
    public class Interpreter
    {
        #region ==================== Nested ====================
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
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Converts the string to a logical model.
        /// </summary>
        public static BooleanExpression AsBooleanExpression(string transition, IProcessingData context)
        {
            string[] words = transition.Split(' ');
            Data data = new Data(words);
            BooleanExpression mainExpression = InterpretBooleanExpression(data, context);
            return mainExpression;
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        /// <summary>
        /// Interprets the given words into a logical model.
        /// As there are different possible approaches, we choose a left to right packing method to provide
        /// a readable debug string for the user.
        /// </summary>
        private static BooleanExpression InterpretBooleanExpression(Data data, IProcessingData context)
        {
            if (data.IsEndReached) return null;
            string currentWord = data.GetNext();
            
            BooleanExpression currentExpression = null;
            // B -> I B
            if (LogicalInverter.Values.Contains(currentWord))
            {
                BooleanExpression nextExpression = InterpretBooleanExpression(data, context);
                currentExpression = new LogicalInverter(nextExpression);
                if (data.IsEndReached) return currentExpression;
                currentWord = data.GetNext();
            }
            // B -> b
            if (IsRepresentingBoolean(currentWord, context))
            {
                currentExpression = InterpretBoolean(currentWord, context);
                if (data.IsEndReached) return currentExpression;
                currentWord = data.GetNext();
            }
            // B -> N V N
            else if (IsRepresentingNumerical(currentWord, context))
            {
                // N -> n
                Numerical.NumericalExpression leftNumber = Numerical.Interpreter.AsNumericalExpression(currentWord, context);
                if (data.IsEndReached) return null;
                string relation = data.GetNext();
                // V -> v
                if (!RelationalOperation.Values.Contains(relation)) return null;
                if (data.IsEndReached) return null;
                currentWord = data.GetNext();
                // N -> n
                if (!IsRepresentingNumerical(currentWord, context)) return null;
                Numerical.NumericalExpression rightNumber = Numerical.Interpreter.AsNumericalExpression(currentWord, context);
                currentExpression = new RelationalOperation(relation, leftNumber, rightNumber);
                if (data.IsEndReached) return currentExpression;
                currentWord = data.GetNext();
            }
            // B -> B E B
            if (LogicalCombination.Values.Contains(currentWord))
            {
                BooleanExpression nextExpression = InterpretBooleanExpression(data, context);
                return new LogicalCombination(currentWord, currentExpression, nextExpression);
            }
            return currentExpression; // Partial failure
        }
        
        private static bool IsRepresentingBoolean(string word, IProcessingData context)
        {
            return Constant.Values.Contains(word)
            || context.InputRegisters.ContainsBoolean(word);
        }
        
        private static BooleanExpression InterpretBoolean(string word, IProcessingData context)
        {
            if (Constant.Values.Contains(word)) return new Constant(word);
            return new PlantReference(word, context);
        }
        
        private static bool IsRepresentingNumerical(string word, IProcessingData context)
        {
            return int.TryParse(word, out _)
            || context.InputRegisters.ContainsInteger(word)
            || context.HasIntVariable(word);
        }
        #endregion
    }
}