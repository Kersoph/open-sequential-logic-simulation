namespace Osls.SfcEditor.Interpreter.Boolean
{
    /// <summary>
    /// Used to link boolean outputs of the plant to the transition
    /// </summary>
    public class PlantReference : BooleanExpression
    {
        #region ==================== Fields Properties ====================
        private readonly string _key;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Holds a reference to a boolean plant output
        /// </summary>
        public PlantReference(string key)
        {
            _key = key;
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override bool Result(SfcSimulation.Engine.SfcProgramm sfcProgramm)
        {
            return sfcProgramm.Plc.InputRegisters.PollBoolean(_key);
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return PlantViewNode.LoadedSimulationNode.SimulationOutput.ContainsBoolean(_key);
        }
        
        public override string ToString()
        {
            return _key + (IsValid() ? "" : "?");
        }
        #endregion
    }
}