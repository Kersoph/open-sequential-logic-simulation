namespace Osls.St.Assignment
{
    /// <summary>
    /// Represents an boolean assignment
    /// </summary>
    public class Boolean : AssignmentExpression
    {
        #region ==================== Fields Properties ====================
        private readonly string _target;
        private readonly St.Boolean.BooleanExpression _source;
        private readonly bool _valid;
        #endregion
        
        
        #region ==================== Public ====================
        public Boolean(string target, St.Boolean.BooleanExpression source, IProcessingData data)
        {
            _target = target;
            _source = source;
            _valid = data.OutputRegisters.ContainsBoolean(_target);
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
            return _valid && _source != null && _source.IsValid();
        }
        
        public override string ToString()
        {
            string target = (_valid ? _target : "?");
            return target + " = " + (_source != null && _source.IsValid() ? _source.ToString() : "?");
        }
        #endregion
    }
}