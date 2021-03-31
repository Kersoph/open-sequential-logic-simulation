using System;
using System.Collections.Generic;
using SfcSandbox.Data.Model.SfcEditor;
using SfcSandbox.Data.Model.SfcEditor.Interpreter;
using SfcSandbox.Data.Model.SfcEditor.Interpreter.Assignment;

namespace SfcSandbox.Data.Model.SfcSimulation.Engine
{
    public class SfcStep
    {
        #region ==================== Fields Properties ====================
        public SfcPatchEntity SourceReference { get; private set; }
        public int Id { get; private set; }
        public int StepCounter { get; private set; }
        
        private readonly SfcProgramm _controler;
        private Dictionary<ActionQualifier, List<AssignmentExpression>> _actions;
        private List<SfcTransition> _transitions;
        #endregion
        
        
        #region ==================== Constructor ====================
        public SfcStep(SfcPatchEntity source, SfcProgramm controler, int id)
        {
            SourceReference = source;
            _controler = controler;
            Id = id;
            AssignActionsFrom(source);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public void ExecuteActions(ActionQualifier qualifier)
        {
            List<AssignmentExpression> actions = _actions[qualifier];
            foreach (AssignmentExpression action in actions)
            {
                action.Execute(_controler);
            }
            UpdateStepCounter(qualifier);
        }
        
        /// <summary>
        /// Calculatest the transitions and updates the step status according to the result
        /// </summary>
        public void CalculateTransition()
        {
            foreach (SfcTransition transition in _transitions)
            {
                if (transition.CalculateTransition(_controler))
                {
                    _controler.UpdateStepStatus(transition.NextSteps, transition.DependingSteps);
                    return; // If multiple transitions could fire due to user error, we use the first mode.
                }
            }
        }
        
        /// <summary>
        /// Parses the diagram and sets up the transitions reached by this step.
        /// </summary>
        public void InitializeTransitions(Dictionary<int, SfcStep> allSteps)
        {
            _transitions = SfcStepBuilder.CollectTransitionSources(this, allSteps);
            foreach (SfcTransition transition in _transitions)
            {
                SfcStepBuilder.AssignTransitionDestinations(transition, allSteps);
            }
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsStepValid()
        {
            foreach(List<AssignmentExpression> assignments in _actions.Values)
            {
                foreach(AssignmentExpression expression in assignments)
                {
                    if(expression == null || !expression.IsValid())
                    {
                        return false;
                    }
                }
            }
            if(_transitions == null) return true;
            foreach(SfcTransition transition in _transitions)
            {
                if(!transition.IsTransitionValid()) return false;
            }
            return true;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void AssignActionsFrom(SfcPatchEntity source)
        {
            _actions = new Dictionary<ActionQualifier, List<AssignmentExpression>>();
            foreach (ActionQualifier qualifier in (ActionQualifier[]) Enum.GetValues(typeof(ActionQualifier)))
            {
                _actions.Add(qualifier, new List<AssignmentExpression>());
            }
            
            for (int i = 0; i < source.ActionEntries.Count; i++)
            {
                AssignmentExpression expression = ActionMaster.InterpretTransitionText(source.ActionEntries[i].Action);
                _actions[source.ActionEntries[i].Qualifier].Add(expression);
            }
        }
        
        private void UpdateStepCounter(ActionQualifier qualifier)
        {
            switch (qualifier)
            {
                case ActionQualifier.PPlus:
                    break;
                case ActionQualifier.N:
                    if(StepCounter != Int32.MaxValue) StepCounter += (int)(1000f * Master.StepUpdateTime);
                    break;
                case ActionQualifier.PMinus:
                    StepCounter = 0;
                    break;
            }
        }
        #endregion
    }
}