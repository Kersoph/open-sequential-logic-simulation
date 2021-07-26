using System.Collections.Generic;
using Osls.SfcEditor;
using Osls.SfcEditor.Interpreters;
using Osls.St.Boolean;


namespace Osls.SfcSimulation.Engine.Builder
{
    public static class Sources
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Looks for all transitions connected to this step
        /// </summary>
        public static List<SfcTransition> CollectTransitionSources(SfcStep source, ProgrammableLogicController pu)
        {
            List<SfcTransition> alternativeBranches = CollectUpperAlternativeBranches(source, source.Id, pu);
            List<SfcTransition> SimultaneousMerge = CollectUpperSimultaneousMerge(source.Id, pu);
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
            Godot.GD.PushWarning("TODO? " + source.Id);
            return new List<SfcTransition>() { };
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Collects all alternative branches from this step.
        /// If it returns only one transition is not an alternative branch but a normal transition.
        /// </summary>
        private static List<SfcTransition> CollectUpperAlternativeBranches(SfcStep source, int patchId, ProgrammableLogicController pu)
        {
            List<SfcTransition> transitions = new List<SfcTransition>();
            List<int> collected = Collector.CollectHorizontal(patchId, pu.SfcProgramData, BranchType.Single, true);
            foreach (int step in collected)
            {
                if (pu.SfcProgramData.SfcEntity.Lookup(step).ContainsTransition())
                {
                    SfcTransition transition = CreateTransition(step, pu);
                    transition.DependingSteps.Add(source);
                    transitions.Add(transition);
                }
                else
                {
                    int lowerStepId = step + 1;
                    if (pu.SfcProgramData.SfcEntity.Lookup(lowerStepId).SfcStepType == StepType.Pass)
                    {
                        transitions.AddRange(CollectUpperAlternativeBranches(source, lowerStepId, pu));
                    }
                }
            }
            return transitions;
        }
        
        /// <summary>
        /// Collects all Simultaneous branches to merge from this step.
        /// null if there is none.
        /// </summary>
        private static List<SfcTransition> CollectUpperSimultaneousMerge(int sourceId, ProgrammableLogicController pu)
        {
            List<int> collected = Collector.CollectHorizontal(sourceId, pu.SfcProgramData, BranchType.Double, true);
            if (collected.Count <= 1)
            {
                PatchEntity currentPatch = pu.SfcProgramData.SfcEntity.Lookup(sourceId);
                int lowerId = sourceId + 1;
                PatchEntity lowerPatch = pu.SfcProgramData.SfcEntity.Lookup(lowerId);
                if (!currentPatch.ContainsTransition() && lowerPatch != null && lowerPatch.SfcStepType == StepType.Pass)
                {
                    return CollectUpperSimultaneousMerge(lowerId, pu);
                }
                return null;
            }
            PatchEntity transitionPatch = FindSimultaneousTransition(collected, pu.SfcProgramData);
            List<SfcStep> connectedSteps = CollectConnectedSteps(collected, pu.SfcProgramData);
            SfcTransition transition = CreateTransition(transitionPatch.Key, pu);
            transition.DependingSteps = connectedSteps;
            return new List<SfcTransition> { transition };
        }
        
        private static PatchEntity FindSimultaneousTransition(List<int> patches, SfcProgramData data)
        {
            PatchEntity transitionPatch = null;
            foreach (int step in patches)
            {
                transitionPatch = data.SfcEntity.Lookup(step);
                if (transitionPatch != null && transitionPatch.ContainsTransition()) break;
            }
            if (transitionPatch == null) Godot.GD.PushError("It is not allowed wo have Simultaneous branches without one transition!");
            return transitionPatch;
        }
        
        private static List<SfcStep> CollectConnectedSteps(List<int> patches, SfcProgramData data)
        {
            List<SfcStep> connectedSteps = new List<SfcStep>();
            foreach (int step in patches)
            {
                SfcStep connectedStep = Collector.FindUpperConnectedStep(step, data);
                if (connectedStep != null)
                {
                    connectedSteps.Add(connectedStep);
                }
            }
            if (connectedSteps.Count < 2) Godot.GD.PushError("It does not make sense to merge one branch.");
            return connectedSteps;
        }
        
        /// <summary>
        /// Creates a new SfcTransition with the given transitionPatchId
        /// </summary>
        private static SfcTransition CreateTransition(int transitionPatchId, ProgrammableLogicController pu)
        {
            string transitionText = pu.SfcProgramData.SfcEntity.Lookup(transitionPatchId).TransitionText;
            BooleanExpression expression = TransitionMaster.InterpretTransitionText(transitionText, pu);
            SfcTransition transition = new SfcTransition(expression, transitionPatchId);
            return transition;
        }
        #endregion
    }
}