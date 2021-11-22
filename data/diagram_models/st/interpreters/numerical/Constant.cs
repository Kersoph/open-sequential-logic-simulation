namespace Osls.St.Numerical
{
    /// <summary>
    /// Represents an constant integer
    /// </summary>
    public class Constant : NumericalExpression
    {
        #region ==================== Fields / Properties ====================
        private readonly int _number;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public Constant(int number)
        {
            _number = number;
        }
        
        /// <summary>
        /// Calculates the result of this numerical expression
        /// </summary>
        public override int Result(IProcessingUnit pu)
        {
            return _number;
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return true;
        }
        
        public override string ToString()
        {
            return _number.ToString();
        }
        #endregion
    }
}
