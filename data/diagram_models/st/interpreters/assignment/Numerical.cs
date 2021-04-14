namespace Osls.SfcEditor.Interpreters.Assignment
{
    /// <summary>
    /// Represents an numerical assignment
    /// </summary>
    public class Numerical : AssignmentExpression
    {
        #region ==================== Fields Properties ====================
        private readonly string _target;
        private readonly Interpreters.Numerical.NumericalExpression _source;
        private readonly bool _valid;
        #endregion
        
        
        #region ==================== Public ====================
        public Numerical(string target, Interpreters.Numerical.NumericalExpression source, IProcessingData data)
        {
            _target = target;
            _source = source;
            _valid = data.OutputRegisters.ContainsInteger(_target);
        }
        
        /// <summary>
        /// Executes the assignment according to the model.
        /// </summary>
        public override void Execute(IProcessingUnit pu)
        {
            pu.OutputRegisters.SetValue(_target, _source.Result(pu));
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return _valid && _source.IsValid();
        }
        
        public override string ToString()
        {
            string target = (_valid ? _target : "?");
            return target + " = " + (_source.IsValid() ? _source.ToString() : "?");
        }
        #endregion
    }
}