using Osls.St.Boolean;
using Osls.St.Numerical;
using System.Collections.Generic;


namespace Osls.St.Assignment
{
    public class Interpreter
    {
        #region ==================== Fields / Properties ====================
        private static readonly HashSet<string> AssignmentSymbol = new HashSet<string>() { "=", ":=" };
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Converts the string to a logical model.
        /// </summary>
        public static AssignmentExpression AsAssignmentExpression(string transition, IProcessingData processingData)
        {
            Terminals data = new Terminals(transition);
            return InterpretAssignmentExpression(data, processingData);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Interprets the given words into a logical model.
        /// We follow a fixed y = x format according the requirements.
        /// </summary>
        private static AssignmentExpression InterpretAssignmentExpression(Terminals data, IProcessingData context)
        {
            string targetWord = data.Current;
            data.MoveNext();
            // A -> b_REF
            if (data.IsEndReached) return InterpretAsDirectBooleanAssignment(targetWord, context);
            string assignmentSymbol = data.Current;
            data.MoveNext();
            if (!AssignmentSymbol.Contains(assignmentSymbol) || data.IsEndReached) return null;
            // A -> b_REF O B
            if (context.OutputRegisters.ContainsBoolean(targetWord))
            {
                BooleanExpression sourceExpression = St.Boolean.Interpreter.InterpretBooleanExpression(data, context);
                return new Boolean(targetWord, sourceExpression, context);
            }
            // A -> n_REF O N
            else if (context.OutputRegisters.ContainsInteger(targetWord))
            {
                string numerical = data.Current;
                NumericalExpression sourceExpression = St.Numerical.Interpreter.AsNumericalExpression(numerical, context);
                return new Numerical(targetWord, sourceExpression, context);
            }
            return null; // not valid
        }
        
        /// <summary>
        /// If there is only one word in the action box, it can be assumed that the user likes to activate / set a boolean.
        /// From a feature request: It seem to be like this in other SFC-ST actions.
        /// </summary>
        private static Boolean InterpretAsDirectBooleanAssignment(string targetWord, IProcessingData context)
        {
            if (context.OutputRegisters.ContainsBoolean(targetWord))
            {
                return new Boolean(targetWord, new St.Boolean.Constant("true"), context);
            }
            return null;
        }
        #endregion
    }
}
