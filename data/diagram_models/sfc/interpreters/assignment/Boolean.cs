namespace Osls.SfcEditor.Interpreters.Assignment
{
    /// <summary>
    /// Represents an boolean assignment
    /// </summary>
    public class Boolean : AssignmentExpression
    {
        #region ==================== Fields Properties ====================
        private readonly string _target;
        private readonly Interpreters.Boolean.BooleanExpression _source;
        #endregion
        
        
        #region ==================== Public ====================
        public Boolean(string target, Interpreters.Boolean.BooleanExpression source)
        {
            _target = target;
            _source = source;
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
            return PlantViewNode.LoadedSimulationNode.SimulationInput.ContainsBoolean(_target)
            && _source.IsValid();
        }
        
        public override string ToString()
        {
            bool isTargetValid = PlantViewNode.LoadedSimulationNode.SimulationInput.ContainsBoolean(_target);
            string target = (isTargetValid ? _target : "?");
            return target + " = " + (_source.IsValid() ? _source.ToString() : "?");
        }
        #endregion
    }
}