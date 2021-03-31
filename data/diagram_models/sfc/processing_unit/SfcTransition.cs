using System.Collections.Generic;
using SfcSandbox.Data.Model.SfcEditor.Interpreter.Boolean;
using SfcSandbox.Data.Model.SfcEditor.Interpreter;

namespace SfcSandbox.Data.Model.SfcSimulation.Engine
{
    public class SfcTransition
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// The Step patch this transition is located
        /// </summary>
        public SfcStep Holder { get; private set; }
        
        /// <summary>
        /// The step IDs we have to wait to until we can check the transition.
        /// Multiple -> "Zusammenführung simulater Verzweigung" in documentation.
        /// </summary>
        public List<SfcStep> DependingSteps { get; set; }
        
        /// <summary>
        /// The condition to fire the chnage.
        /// </summary>
        public BooleanExpression Transition { get; private set; }
        
        /// <summary>
        /// The step IDs we will activate with this transition
        /// Multiple -> "Simulate Verzweigung" in documentation.
        /// </summary>
        public List<SfcStep> NextSteps { get; set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public SfcTransition(SfcStep holder)
        {
            Holder = holder;
            DependingSteps = new List<SfcStep>();
            NextSteps = new List<SfcStep>();
            Transition = TransitionMaster.InterpretTransitionText(Holder.SourceReference.TransitionText);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates if this transition fires or not.
        /// </summary>
        public bool CalculateTransition(SfcProgramm programm)
        {
            // All dependent steps must be active
            foreach (SfcStep step in DependingSteps)
            {
                if(!programm.IsStepActive(step)) return false;
            }
            return Transition.Result(programm);
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsTransitionValid()
        {
            if(DependingSteps == null) return false;
            if(NextSteps == null) return false;
            if(Transition == null) return false;
            return Transition.IsValid();
        }
        #endregion
    }
}