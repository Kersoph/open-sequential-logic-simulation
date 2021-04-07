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
            List<int> collected = Collector.CollectHorizontal(source.Id, data, BranchType.Single, true);
            foreach (int step in collected)
            {
                if (data.SfcEntity.Lookup(step).ContainsTransition())
                {
                    SfcTransition transition = CreateTransition(step, data);
                    transition.DependingSteps.Add(source);
                    transitions.Add(transition);
                }
            }
            return transitions;
        }
        
        /// <summary>
        /// Collects all Simultaneous branches to merge from this step.
        /// null if there is none.
        /// </summary>
        private static List<SfcTransition> CollectUpperSimultaneousMerge(SfcStep source, SfcProgrammData data)
        {
            List<int> collected = Collector.CollectHorizontal(source.Id, data, BranchType.Double, true);
            if (collected.Count <= 1) return null;
            PatchEntity transitionPatch = FindSimultaneousTransition(collected, data);
            List<SfcStep> connectedSteps = CollectConnectedSteps(collected, data);
            if (!IsMinimalId(connectedSteps, source.Id)) return new List<SfcTransition> { };
            SfcTransition transition = CreateTransition(transitionPatch.Key, data);
            transition.DependingSteps = connectedSteps;
            return new List<SfcTransition> { transition };
        }
        
        private static PatchEntity FindSimultaneousTransition(List<int> patches, SfcProgrammData data)
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
        
        private static List<SfcStep> CollectConnectedSteps(List<int> patches, SfcProgrammData data)
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
        
        private static bool IsMinimalId(List<SfcStep> steps, int id)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                if (steps[i].Id < id) return false;
            }
            return true;
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
        #endregion
    }
}