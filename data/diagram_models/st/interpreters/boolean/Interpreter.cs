namespace Osls.St.Boolean
{
    public class Interpreter
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Converts the string to a logical model.
        /// </summary>
        public static BooleanExpression AsBooleanExpression(string transition, IProcessingData context)
        {
            Terminals data = new Terminals(transition);
            BooleanExpression mainExpression = InterpretBooleanExpression(data, context);
            if (!data.IsEndReached) mainExpression = AppendFailureInfo(data, mainExpression);
            return mainExpression;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Interprets the given words into a logical model.
        /// As there are different possible approaches, we choose a left to right packing method to provide
        /// a readable debug string for the user.
        /// </summary>
        internal static BooleanExpression InterpretBooleanExpression(Terminals data, IProcessingData context)
        {
            if (data.IsEndReached) return null;
            BooleanExpression currentExpression = null;
            // B -> ( B )
            if (data.Current == "(")
            {
                data.MoveNext();
                BooleanExpression subExpression = InterpretBooleanExpression(data, context);
                if (data.IsEndReached || data.Current != ")") return new Grouping(currentExpression, false);
                data.MoveNext();
                currentExpression = new Grouping(subExpression, true);
                if (data.IsEndReached) return currentExpression;
            }
            
            // B -> I B
            if (LogicalInverter.Values.Contains(data.Current))
            {
                data.MoveNext();
                BooleanExpression nextExpression = InterpretBooleanExpression(data, context);
                currentExpression = new LogicalInverter(nextExpression);
                if (data.IsEndReached) return currentExpression;
            }
            // B -> b
            if (IsRepresentingBoolean(data.Current, context))
            {
                currentExpression = InterpretBoolean(data.Current, context);
                data.MoveNext();
                if (data.IsEndReached) return currentExpression;
            }
            // B -> N R N
            else if (IsRepresentingNumerical(data.Current, context))
            {
                // N -> n
                Numerical.NumericalExpression leftNumber = Numerical.Interpreter.AsNumericalExpression(data.Current, context);
                data.MoveNext();
                if (data.IsEndReached) return null;
                // R -> r
                string relation = data.Current;
                if (!RelationalOperation.Values.Contains(relation)) return null;
                data.MoveNext();
                if (data.IsEndReached) return null;
                // N -> n
                if (!IsRepresentingNumerical(data.Current, context)) return null;
                Numerical.NumericalExpression rightNumber = Numerical.Interpreter.AsNumericalExpression(data.Current, context);
                currentExpression = new RelationalOperation(relation, leftNumber, rightNumber);
                data.MoveNext();
                if (data.IsEndReached) return currentExpression;
            }
            // B -> B E B
            if (LogicalCombination.Values.Contains(data.Current))
            {
                string combination = data.Current;
                data.MoveNext();
                BooleanExpression nextExpression = InterpretBooleanExpression(data, context);
                return new LogicalCombination(combination, currentExpression, nextExpression);
            }
            if (data.Current == ")")
            {
                return currentExpression;
            }
            return currentExpression; // Partial failure
        }
        
        private static bool IsRepresentingBoolean(string word, IProcessingData context)
        {
            return Constant.Values.Contains(word)
            || context.InputRegisters.ContainsBoolean(word)
            || context.HasBoolVariable(word);
        }
        
        private static BooleanExpression InterpretBoolean(string word, IProcessingData context)
        {
            if (Constant.Values.Contains(word)) return new Constant(word);
            if (context.HasBoolVariable(word)) return new StepReference(word);
            return new PlantReference(word, context);
        }
        
        private static bool IsRepresentingNumerical(string word, IProcessingData context)
        {
            return int.TryParse(word, out _)
            || context.InputRegisters.ContainsInteger(word)
            || context.HasIntVariable(word);
        }
        
        private static BooleanExpression AppendFailureInfo(Terminals data, BooleanExpression mainExpression)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            while (!data.IsEndReached)
            {
                builder.Append(data.Current);
                builder.Append(" ");
                data.MoveNext();
            }
            return new FailureHelper(mainExpression, builder.ToString());
        }
        #endregion
    }
}
