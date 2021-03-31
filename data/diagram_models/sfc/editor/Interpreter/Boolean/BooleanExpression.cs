namespace SfcSandbox.Data.Model.SfcEditor.Interpreter.Boolean
{
    /// <summary>
    /// Base class for all boolean expressions
    /// May convert it to an interface
    /// </summary>
    public abstract class BooleanExpression
    {
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public abstract bool Result(SfcSimulation.Engine.SfcProgramm sfcProgramm);
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public abstract bool IsValid();
    }
}