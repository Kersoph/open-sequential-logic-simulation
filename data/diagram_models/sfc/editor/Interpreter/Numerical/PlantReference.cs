namespace Osls.SfcEditor.Interpreter.Numerical
{
    /// <summary>
    /// Used to link integer outputs of the plant to the transition
    /// </summary>
    public class PlantReference : NumericalExpression
    {
        #region ==================== Fields Properties ====================
        private readonly string _key;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Holds a reference to a integer plant output
        /// </summary>
        public PlantReference(string key)
        {
            _key = key;
        }
        
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public override int Result(SfcSimulation.Engine.SfcProgramm sfcProgramm)
        {
            return PlantViewNode.LoadedSimulationNode.SimulationOutput.PollInteger(_key);
        }
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public override bool IsValid()
        {
            return PlantViewNode.LoadedSimulationNode.SimulationOutput.ContainsInteger(_key);
        }
        
        public override string ToString()
        {
            return _key + (IsValid() ? "" : "?");
        }
        #endregion
    }
}