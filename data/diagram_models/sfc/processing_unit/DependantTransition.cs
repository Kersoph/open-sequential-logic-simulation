using Osls.SfcEditor.Interpreter.Boolean;


namespace Osls.SfcSimulation.Engine
{
    public class DependantTransition : SfcTransition
    {
        #region ==================== Fields Properties ====================
        #endregion
        
        
        #region ==================== Constructor ====================
        public DependantTransition(BooleanExpression transition, int id) : base (transition, id)
        {
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates if this transition fires or not.
        /// </summary>
        public override bool CalculateTransition(SfcProgramm programm)
        {
            return false;
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public override bool IsTransitionValid()
        {
            return true;
        }
        #endregion
    }
}