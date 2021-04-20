using System;
using System.Collections.Generic;
using Osls.SfcEditor;
using Osls.SfcEditor.Interpreters;
using Osls.St.Assignment;
using Osls.SfcSimulation.Engine.Builder;


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
        public void Initialise(ProgrammableLogicController pu)
        {
            AssignActionsFrom(pu.SfcProgramData.SfcEntity.Lookup(Id), pu);
            _transitions = Sources.CollectTransitionSources(this, pu);
            foreach (SfcTransition transition in _transitions)
            {
                Destinations.AssignTransitionDestinations(transition, pu.SfcProgramData);
            }
        }
        
        /// <summary>
        /// Executes the ations with the marked qualifier in the given program context and delta time
        /// </summary>
        public void ExecuteActions(SfcProgram context, ActionQualifier qualifier, int deltaTimeMs)
        {
            List<AssignmentExpression> actions = _actions[qualifier];
            foreach (AssignmentExpression action in actions)
            {
                action.Execute(context.Plc);
            }
            UpdateStepCounter(qualifier, deltaTimeMs);
        }
        
        /// <summary>
        /// Calculates the transitions and updates the step status according to the result
        /// </summary>
        public void CalculateTransition(SfcProgram context)
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
                if (!transition.IsTransitionValid())
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void AssignActionsFrom(PatchEntity source, IProcessingData context)
        {
            _actions = new Dictionary<ActionQualifier, List<AssignmentExpression>>();
            foreach (ActionQualifier qualifier in (ActionQualifier[]) Enum.GetValues(typeof(ActionQualifier)))
            {
                _actions.Add(qualifier, new List<AssignmentExpression>());
            }
            
            for (int i = 0; i < source.ActionEntries.Count; i++)
            {
                AssignmentExpression expression = ActionMaster.InterpretTransitionText(source.ActionEntries[i].Action, context);
                _actions[source.ActionEntries[i].Qualifier].Add(expression);
            }
        }
        
        private void UpdateStepCounter(ActionQualifier qualifier, int deltaTimeMs)
        {
            switch (qualifier)
            {
                case ActionQualifier.PPlus:
                    break;
                case ActionQualifier.N:
                    if(StepCounter != int.MaxValue) StepCounter += deltaTimeMs;
                    break;
                case ActionQualifier.PMinus:
                    StepCounter = 0;
                    break;
            }
        }
        #endregion
    }
}