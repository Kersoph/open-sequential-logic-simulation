using System;
using System.Collections.Generic;
using Osls.SfcEditor;
using Osls.SfcEditor.Interpreter;
using Osls.SfcEditor.Interpreter.Assignment;


namespace Osls.SfcSimulation.Engine
{
    public class SfcStep
    {
        #region ==================== Fields Properties ====================
        public int Id { get; private set; }
        public int StepCounter { get; private set; }
        
        private Dictionary<ActionQualifier, List<AssignmentExpression>> _actions;
        private List<SfcTransition> _transitions;
        #endregion
        
        
        #region ==================== Constructor ====================
        public SfcStep(PatchEntity source)
        {
            Id = source.Key;
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Parses the diagram and sets up the transitions reached by this step.
        /// </summary>
        public void Initialise(SfcProgrammData data)
        {
            AssignActionsFrom(data.SfcEntity.Lookup(Id));
            _transitions = SfcStepBuilder.CollectTransitionSources(this, data);
            foreach (SfcTransition transition in _transitions)
            {
                SfcStepBuilder.AssignTransitionDestinations(transition, this, data);
            }
        }
        
        public void ExecuteActions(SfcProgramm context, ActionQualifier qualifier)
        {
            List<AssignmentExpression> actions = _actions[qualifier];
            foreach (AssignmentExpression action in actions)
            {
                action.Execute(context);
            }
            UpdateStepCounter(qualifier);
        }
        
        /// <summary>
        /// Calculatest the transitions and updates the step status according to the result
        /// </summary>
        public void CalculateTransition(SfcProgramm context)
        {
            foreach (SfcTransition transition in _transitions)
            {
                if (transition.CalculateTransition(context))
                {
                    context.UpdateStepStatus(transition.NextSteps, transition.DependingSteps);
                    return; // If multiple transitions could fire due to user error, we use the first mode.
                }
            }
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsStepValid()
        {
            foreach (List<AssignmentExpression> assignments in _actions.Values)
            {
                foreach (AssignmentExpression expression in assignments)
                {
                    if (expression == null || !expression.IsValid())
                    {
                        return false;
                    }
                }
            }
            if (_transitions == null) return true;
            foreach (SfcTransition transition in _transitions)
            {
                if (!transition.IsTransitionValid()) return false;
            }
            return true;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void AssignActionsFrom(PatchEntity source)
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
                    if(StepCounter != int.MaxValue) StepCounter += (int)(1000f * Master.StepUpdateTime);
                    break;
                case ActionQualifier.PMinus:
                    StepCounter = 0;
                    break;
            }
        }
        #endregion
    }
}