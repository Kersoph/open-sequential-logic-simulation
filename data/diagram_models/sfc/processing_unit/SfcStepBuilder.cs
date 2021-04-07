using System.Collections.Generic;
using Osls.SfcEditor;
using Osls.SfcEditor.Interpreter;
using Osls.SfcEditor.Interpreter.Boolean;


namespace Osls.SfcSimulation.Engine
{
    public static class SfcStepBuilder
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Looks for all transitions connected to this step
        /// </summary>
        public static List<SfcTransition> CollectTransitionSources(SfcStep source, SfcProgrammData data)
        {
            List<SfcTransition> alternativeBranches = CollectUpperAlternativeBranches(source, data);
            SfcTransition SimultaneousMerge = CollectUpperSimultaneousMerge(source, data);
            if (alternativeBranches.Count > 1)
            {
                return alternativeBranches;
            }
            else if (SimultaneousMerge != null)
            {
                return new List<SfcTransition>() { SimultaneousMerge };
            }
            else if (alternativeBranches.Count > 0)
            {
                return alternativeBranches;
            }
            Godot.GD.PushError("TODO? " + source.Id);
            return new List<SfcTransition>() { };
        }
        
        /// <summary>
        /// Assigns all destinations to this transition
        /// </summary>
        public static void AssignTransitionDestinations(SfcTransition transition, SfcProgrammData data)
        {
            List<SfcStep> lowerTargets = CollectLowerSimultaneousBranches(transition.Id, data);
            if (lowerTargets == null || lowerTargets.Count > 0)
            {
                transition.NextSteps = lowerTargets;
            }
            else
            {
                SfcStep alternativeStep = FindAlternativeMergeTarget(transition.Id, data);
                List<SfcStep> alternativeList = new List<SfcStep>(1);
                if (alternativeStep != null) alternativeList.Add(alternativeStep);
                transition.NextSteps = alternativeList;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Collects all alternative branches from this step.
        /// if it returns only one transition is not an alternative branch.
        /// </summary>
        private static List<SfcTransition> CollectUpperAlternativeBranches(SfcStep source, SfcProgrammData data)
        {
            List<SfcTransition> transitions = new List<SfcTransition>();
            List<int> collectedUpperAlternatingSteps = new List<int>() { source.Id };
            CollectLeftDependingSteps(source.Id, data, collectedUpperAlternatingSteps, BranchType.Single, true);
            CollectRightDependingSteps(source.Id, data, collectedUpperAlternatingSteps, BranchType.Single, true);
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
        /// Creates a new SfcTransition
        /// </summary>
        private static SfcTransition CreateTransition(int id, SfcProgrammData data)
        {
            string transitionText = data.SfcEntity.Lookup(id).TransitionText;
            BooleanExpression expression = TransitionMaster.InterpretTransitionText(transitionText, data.StepMaster);
            SfcTransition transition = new SfcTransition(expression, id);
            return transition;
        }
        
        /// <summary>
        /// Collects all Simultaneous branches to merge from this step.
        /// null if there is none.
        /// </summary>
        private static SfcTransition CollectUpperSimultaneousMerge(SfcStep source, SfcProgrammData data)
        {
            SfcTransition transition = null;
            List<int> collectedUpperSimultaneousSteps = new List<int>() { source.Id };
            CollectLeftDependingSteps(source.Id, data, collectedUpperSimultaneousSteps, BranchType.Double, true);
            CollectRightDependingSteps(source.Id, data, collectedUpperSimultaneousSteps, BranchType.Double, true);
            // TODO: Sucher auch tiefere
            if (collectedUpperSimultaneousSteps.Count > 1)
            {
                PatchEntity transitionStep = null;
                foreach (int step in collectedUpperSimultaneousSteps)
                {
                    transitionStep = data.SfcEntity.Lookup(step);
                    if (transitionStep != null) break;
                }
                if (transitionStep == null) Godot.GD.PushError("It is not allowed wo have Simultaneous branches without one transition! " + source.Id);
                List<SfcStep> connectedSteps = new List<SfcStep>();
                foreach (int step in collectedUpperSimultaneousSteps)
                {
                    SfcStep connectedStep = FindUpperConnectedStep(step, data);
                    if (connectedStep != null)
                    {
                        connectedSteps.Add(connectedStep);
                    }
                }
                if (connectedSteps.Count < 2) Godot.GD.PushError("It does not make sense to merge one branch. " + source.Id);
                transition = CreateTransition(transitionStep.Key, data);
                transition.DependingSteps = connectedSteps;
            }
            return transition;
        }

        /// <summary>
        /// Collects the list of lower simultanuous branches if possible.
        /// Null if there is an error in the diagram
        /// </summary>
        private static List<SfcStep> CollectLowerSimultaneousBranches(int holder, SfcProgrammData data)
        {
            List<int> connectedLowerSimultaneousSteps = new List<int>() { holder };
            CollectLeftDependingSteps(holder, data, connectedLowerSimultaneousSteps, BranchType.Double, false);
            CollectRightDependingSteps(holder, data, connectedLowerSimultaneousSteps, BranchType.Double, false);
            List<SfcStep> targetSteps = new List<SfcStep>();
            foreach (int step in connectedLowerSimultaneousSteps)
            {
                int subId = step + 1;
                PatchEntity lowerStep = data.SfcEntity.Lookup(subId);
                if (lowerStep != null)
                {
                    switch (lowerStep.SfcStepType)
                    {
                        case StepType.StartingStep:
                        case StepType.Step:
                            targetSteps.Add(data.ControlMap[lowerStep.Key]);
                            break;
                        case StepType.Jump:
                            if (data.StepMaster.ContainsStep(lowerStep.StepName))
                            {
                                int reference = data.StepMaster.GetNameKey(lowerStep.StepName);
                                targetSteps.Add(data.ControlMap[reference]);
                            }
                            else
                            {
                                return null;
                            }
                            break;
                        case StepType.Pass:
                            SfcStep foundStep = FindLowerConnectedStep(lowerStep.Key, data);
                            if (foundStep != null)
                            {
                                targetSteps.Add(foundStep);
                            }
                            break;
                        case StepType.Unused:
                            break;
                    }
                }
            }
            return targetSteps;
        }
        
        private static SfcStep FindAlternativeMergeTarget(int holder, SfcProgrammData data)
        {
            List<int> connectedLowerAlternativeSteps = new List<int>() { holder };
            CollectLeftDependingSteps(holder, data, connectedLowerAlternativeSteps, BranchType.Single, false);
            CollectRightDependingSteps(holder, data, connectedLowerAlternativeSteps, BranchType.Single, false);
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
        
        /// <summary>
        /// Looks for left depending steps.
        /// </summary>
        private static void CollectLeftDependingSteps(int currentId, SfcProgrammData data, List<int> collectedSteps, BranchType targetType, bool upperBranch)
        {
            int leftId = currentId - (1 << SfcEntity.XKeyShift);
            PatchEntity leftStep = data.SfcEntity.Lookup(leftId);
            if (leftStep != null)
            {
                if ((upperBranch && leftStep.UpperBranch == targetType)
                || (!upperBranch && leftStep.LowerBranch == targetType))
                {
                    collectedSteps.Add(leftId);
                    CollectLeftDependingSteps(leftId, data, collectedSteps, targetType, upperBranch);
                }
            }
        }
        
        /// <summary>
        /// Looks for right depending steps.
        /// </summary>
        private static void CollectRightDependingSteps(int currentId, SfcProgrammData data, List<int> collectedSteps, BranchType targetType, bool upperBranch)
        {
            PatchEntity step = data.SfcEntity.Lookup(currentId);
            if ((upperBranch && step.UpperBranch == targetType)
            || (!upperBranch && step.LowerBranch == targetType))
            {
                int rightId = currentId + (1 << SfcEntity.XKeyShift);
                if (data.SfcEntity.Lookup(rightId) != null)
                {
                    collectedSteps.Add(rightId);
                    CollectRightDependingSteps(rightId, data, collectedSteps, targetType, upperBranch);
                }
            }
        }
        
        /// <summary>
        /// Looks for an connection from this patch to any upper connected step (including itself).
        /// </summary>
        private static SfcStep FindUpperConnectedStep(int id, SfcProgrammData data)
        {
            if (data.SfcEntity.Lookup(id).ContainsRealStep())
            {
                return data.ControlMap[id];
            }
            else if (data.SfcEntity.Lookup(id).SfcStepType == StepType.Pass)
            {
                int upperId = id -1;
                if (data.SfcEntity.Lookup(upperId) != null)
                {
                    return FindUpperConnectedStep(upperId, data);
                }
            }
            return null;
        }
        
        /// <summary>
        /// Looks for a connected lower step from this patch
        /// </summary>
        private static SfcStep FindLowerConnectedStep(int id, SfcProgrammData data)
        {
            int subId = id + 1;
            PatchEntity lowerStep = data.SfcEntity.Lookup(subId);
            if (lowerStep != null)
            {
                if (data.SfcEntity.Lookup(lowerStep.Key).ContainsRealStep())
                {
                    return data.ControlMap[lowerStep.Key];
                }
                else if (lowerStep.SfcStepType == StepType.Pass)
                {
                    return FindLowerConnectedStep(lowerStep.Key, data);
                }
                else if (lowerStep.SfcStepType == StepType.Jump)
                {
                    int reference = data.StepMaster.GetNameKey(lowerStep.StepName);
                    return data.ControlMap[reference];
                }
            }
            return null;
        }
        #endregion
    }
}