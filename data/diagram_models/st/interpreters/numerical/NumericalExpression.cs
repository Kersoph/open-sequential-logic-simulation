namespace Osls.SfcEditor.Interpreters.Numerical
{
    /// <summary>
    /// Base class for all numerical expressions
    /// May convert it to an interface
    /// </summary>
    public abstract class NumericalExpression
    {
        #region ==================== Public ====================
        /// <summary>
        /// Calculates the result of this numerical expression
        /// </summary>
        public abstract int Result(IProcessingUnit pu);
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public abstract bool IsValid();
        #endregion
    }
}