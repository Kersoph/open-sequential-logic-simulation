namespace Osls.SfcEditor.Interpreters.Numerical
{
    public class Interpreter
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Converts the string to a numerical Expression.
        /// </summary>
        public static NumericalExpression AsNumericalExpression(string word, IProcessingData context)
        {
            if (int.TryParse(word, out int number)) return new Constant(number);
            if (context.HasIntVariable(word)) return new StepReference(word);
            return new PlantReference(word);
        }
        #endregion
    }
}