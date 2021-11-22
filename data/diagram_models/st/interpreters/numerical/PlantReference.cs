namespace Osls.St.Numerical
{
    /// <summary>
    /// Used to link integer outputs of the plant to the transition
    /// </summary>
    public class PlantReference : NumericalExpression
    {
        #region ==================== Fields / Properties ====================
        private readonly string _key;
        private readonly bool _valid;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Holds a reference to a integer plant output
        /// </summary>
        public PlantReference(string key, IProcessingData data)
        {
            _key = key;
            _valid = data.InputRegisters.ContainsInteger(_key);
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override int Result(IProcessingUnit pu)
        {
            return pu.InputRegisters.PollInteger(_key);
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return _valid;
        }
        
        public override string ToString()
        {
            return _key + (IsValid() ? "" : "?");
        }
        #endregion
    }
}
