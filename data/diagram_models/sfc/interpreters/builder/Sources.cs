using System.Collections.Generic;
using Osls.SfcEditor;
using Osls.SfcEditor.Interpreter;
using Osls.SfcEditor.Interpreter.Boolean;


namespace Osls.SfcSimulation.Engine.Builder
{
    public static class Sources
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Looks for all transitions connected to this step
        /// </summary>
        public static List<SfcTransition> CollectTransitionSources(SfcStep source, SfcProgrammData data)
        {
            List<SfcTransition> alternativeBranches = CollectUpperAlternativeBranches(source, data);
            List<SfcTransition> SimultaneousMerge = CollectUpperSimultaneousMerge(source, data);
            if (alternativeBranches.Count > 1)
            {
                return alternativeBranches;
            }
            else if (SimultaneousMerge != null)
            {
                return SimultaneousMerge;
            }
            else if (alternativeBranches.Count > 0)
            {
                return alternativeBranches;
            }
            Godot.GD.PushError("TODO? " + source.Id);
            return new List<SfcTransition>() { };
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Collects all alternative branches from this step.
        /// If it returns only one transition is not an alternative branch but a normal transition.
        /// </summary>
        private static List<SfcTransition> CollectUpperAlternativeBranches(SfcStep source, SfcProgrammData data)
        {
            List<SfcTransition> transitions = new List<SfcTransition>();
            List<int> collectedUpperAlternatingSteps = new List<int>() { source.Id };
            Collector.CollectLeftDependingSteps(source.Id, data, collectedUpperAlternatingSteps, BranchType.Single, true);
            Collector.CollectRightDependingSteps(source.Id, data, collectedUpperAlternatingSteps, BranchType.Single, true);
            if (collectedUpperAlternatingSteps.Count > 0)
            {
                foreach (int step in collectedUpperAlternatingSteps)
                {
                    if (data.SfcEntity.Lookup(step).ContainsTransition())
                    {
                        SfcTransition transition = CreateTransition(step, data);
                        transition.DependingSteps.Add(source);
                        transitions.Add(transition);
                    }
                }
            }
            return transitions;
        }
        
        /// <summary>
        /// Creates a new SfcTransition with the given transitionPatchId
        /// </summary>
        private static SfcTransition CreateTransition(int transitionPatchId, SfcProgrammData data)
        {
            string transitionText = data.SfcEntity.Lookup(transitionPatchId).TransitionText;
            BooleanExpression expression = TransitionMaster.InterpretTransitionText(transitionText, data.StepMaster);
            SfcTransition transition = new SfcTransition(expression, transitionPatchId);
            return transition;
        }
        
        /// <summary>
        /// Collects all Simultaneous branches to merge from this step.
        /// null if there is none.
        /// </summary>
        private static List<SfcTransition> CollectUpperSimultaneousMerge(SfcStep source, SfcProgrammData data)
        {
            List<SfcTransition> transitions = null;
            List<int> collectedUpperSimultaneousSteps = new List<int>() { source.Id };
            Collector.CollectLeftDependingSteps(source.Id, data, collectedUpperSimultaneousSteps, BranchType.Double, true);
            Collector.CollectRightDependingSteps(source.Id, data, collectedUpperSimultaneousSteps, BranchType.Double, true);
            if (collectedUpperSimultaneousSteps.Count > 1)
            {
                PatchEntity transitionPatch = null;
                foreach (int step in collectedUpperSimultaneousSteps)
                {
                    transitionPatch = data.SfcEntity.Lookup(step);
                    if (transitionPatch != null && transitionPatch.ContainsTransition()) break;
                }
                if (transitionPatch == null) Godot.GD.PushError("It is not allowed wo have Simultaneous branches without one transition! " + source.Id);
                List<SfcStep> connectedSteps = new List<SfcStep>();
                int minimalConnectedId = int.MaxValue;
                foreach (int step in collectedUpperSimultaneousSteps)
                {
                    SfcStep connectedStep = Collector.FindUpperConnectedStep(step, data);
                    if (connectedStep != null)
                    {
                        connectedSteps.Add(connectedStep);
                        if (minimalConnectedId > connectedStep.Id) minimalConnectedId = connectedStep.Id;
                    }
                }
                if (connectedSteps.Count < 2) Godot.GD.PushError("It does not make sense to merge one branch. " + source.Id);
                if (minimalConnectedId == source.Id)
                {
                    SfcTransition transition = CreateTransition(transitionPatch.Key, data);
                    transition.DependingSteps = connectedSteps;
                    transitions = new List<SfcTransition> { transition };
                }
                else
                {
                    transitions = new List<SfcTransition> { };
                }
            }
            return transitions;
        }
        
        private static SfcStep FindAlternativeMergeTarget(int holder, SfcProgrammData data)
        {
            List<int> connectedLowerAlternativeSteps = new List<int>() { holder };
            Collector.CollectLeftDependingSteps(holder, data, connectedLowerAlternativeSteps, BranchType.Single, false);
            Collector.CollectRightDependingSteps(holder, data, connectedLowerAlternativeSteps, BranchType.Single, false);
            foreach (int step in connectedLowerAlternativeSteps)
            {
                int subId = step + 1;
                PatchEntity lowerStep = data.SfcEntity.Lookup(subId);
                if (lowerStep != null)
                {
                    switch (lowerStep.SfcStepType)
                    {
                        case StepType.StartingStep:
                        case StepType.Step:
                            return data.ControlMap[lowerStep.Key];
                        case StepType.Jump:
                            int reference = data.StepMaster.GetNameKey(lowerStep.StepName);
                            return data.ControlMap[reference];
                        case StepType.Pass:
                            SfcStep foundStep = FindAlternativeMergeTarget(lowerStep.Key, data);
                            if (foundStep != null)
                            {
                                return foundStep;
                            }
                            break;
                        case StepType.Unused:
                            break;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}