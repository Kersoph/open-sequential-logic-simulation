using System.Collections.Generic;
using Osls.SfcEditor;
using Osls.SfcEditor.Interpreter;


namespace Osls.SfcSimulation.Engine
{
    public static class SfcStepBuilder
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Looks for all transitions connected to this step
        /// </summary>
        public static List<SfcTransition> CollectTransitionSources(SfcStep owner, Dictionary<int, SfcStep> allSteps)
        {
            List<SfcTransition> alternativeBranches = CollectUpperAlternativeBranches(owner, allSteps);
            SfcTransition SimultaneousMerge = CollectUpperSimultaneousMerge(owner, allSteps);
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
            Godot.GD.PushError("TODO? " + owner.Id);
            return new List<SfcTransition>() { };
        }
        
        /// <summary>
        /// Assigns all destinations to this transition
        /// </summary>
        public static void AssignTransitionDestinations(SfcTransition transition, Dictionary<int, SfcStep> allSteps)
        {
            List<SfcStep> lowerTargets = CollectLowerSimultaneousBranches(transition.Holder, allSteps);
            if (lowerTargets == null || lowerTargets.Count > 0)
            {
                transition.NextSteps = lowerTargets;
            }
            else
            {
                SfcStep alternativeStep = FindAlternativeMergeTarget(transition.Holder, allSteps);
                List<SfcStep> alternativeList = new List<SfcStep>(1);
                if(alternativeStep != null) alternativeList.Add(alternativeStep);
                transition.NextSteps = alternativeList;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Collects all alternative branches to this step.
        /// if it returns only one transition is not an alternative branch.
        /// </summary>
        private static List<SfcTransition> CollectUpperAlternativeBranches(SfcStep owner, Dictionary<int, SfcStep> allSteps)
        {
            List<SfcTransition> transitions = new List<SfcTransition>();
            List<SfcStep> collectedUpperAlternatingSteps = new List<SfcStep>() { owner };
            CollectLeftDependingSteps(owner, allSteps, collectedUpperAlternatingSteps, BranchType.Single, true);
            CollectRightDependingSteps(owner, allSteps, collectedUpperAlternatingSteps, BranchType.Single, true);
            if (collectedUpperAlternatingSteps.Count > 0)
            {
                foreach (SfcStep step in collectedUpperAlternatingSteps)
                {
                    if (step.SourceReference.ContainsTransition())
                    {
                        SfcTransition transition = new SfcTransition(step);
                        transition.DependingSteps.Add(owner);
                        transitions.Add(transition);
                    }
                }
            }
            return transitions;
        }

        /// <summary>
        /// Collects all Simultaneous branches to merge to this step.
        /// null if there is none.
        /// </summary>
        private static SfcTransition CollectUpperSimultaneousMerge(SfcStep owner, Dictionary<int, SfcStep> allSteps)
        {
            SfcTransition transition = null;
            List<SfcStep> collectedUpperSimultaneousSteps = new List<SfcStep>() { owner };
            CollectLeftDependingSteps(owner, allSteps, collectedUpperSimultaneousSteps, BranchType.Double, true);
            CollectRightDependingSteps(owner, allSteps, collectedUpperSimultaneousSteps, BranchType.Double, true);
            // TODO: Sucher auch tiefere
            if (collectedUpperSimultaneousSteps.Count > 1)
            {
                SfcStep transitionStep = null;
                foreach (SfcStep step in collectedUpperSimultaneousSteps)
                {
                    if (step.SourceReference.ContainsTransition())
                    {
                        transitionStep = step;
                        break;
                    }
                }
                if (transitionStep == null) Godot.GD.PushError("It is not allowed wo have Simultaneous branches without one transition! " + owner.Id);
                List<SfcStep> connectedSteps = new List<SfcStep>();
                foreach (SfcStep step in collectedUpperSimultaneousSteps)
                {
                    SfcStep connectedStep = FindUpperConnectedStep(step, allSteps);
                    if (connectedStep != null)
                    {
                        connectedSteps.Add(connectedStep);
                    }
                }
                if (connectedSteps.Count < 2) Godot.GD.PushError("It does not make sense to merge one branch. " + owner.Id);
                transition = new SfcTransition(transitionStep);
                transition.DependingSteps = connectedSteps;
            }
            return transition;
        }

        /// <summary>
        /// Collects the list of lower simultanuous branches if possible.
        /// Null if there is an error in the diagram
        /// </summary>
        private static List<SfcStep> CollectLowerSimultaneousBranches(SfcStep source, Dictionary<int, SfcStep> allSteps)
        {
            List<SfcStep> connectedLowerSimultaneousSteps = new List<SfcStep>() { source };
            CollectLeftDependingSteps(source, allSteps, connectedLowerSimultaneousSteps, BranchType.Double, false);
            CollectRightDependingSteps(source, allSteps, connectedLowerSimultaneousSteps, BranchType.Double, false);
            List<SfcStep> targetSteps = new List<SfcStep>();
            foreach (SfcStep step in connectedLowerSimultaneousSteps)
            {
                int subId = step.Id + 1;
                SfcStep lowerStep;
                if (allSteps.TryGetValue(subId, out lowerStep))
                {
                    switch (lowerStep.SourceReference.SfcStepType)
                    {
                        case StepType.StartingStep:
                        case StepType.Step:
                            targetSteps.Add(lowerStep);
                            break;
                        case StepType.Jump:
                            if(StepMaster.ContainsStep(lowerStep.SourceReference.StepName))
                            {
                                int reference = StepMaster.GetNameKey(lowerStep.SourceReference.StepName);
                                targetSteps.Add(allSteps[reference]);
                            }
                            else
                            {
                                return null;
                            }
                            break;
                        case StepType.Pass:
                            SfcStep foundStep = FindLowerConnectedStep(lowerStep, allSteps);
                            if(foundStep != null)
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
        
        private static SfcStep FindAlternativeMergeTarget(SfcStep source, Dictionary<int, SfcStep> allSteps)
        {
            List<SfcStep> connectedLowerAlternativeSteps = new List<SfcStep>() { source };
            CollectLeftDependingSteps(source, allSteps, connectedLowerAlternativeSteps, BranchType.Single, false);
            CollectRightDependingSteps(source, allSteps, connectedLowerAlternativeSteps, BranchType.Single, false);
            foreach (SfcStep step in connectedLowerAlternativeSteps)
            {
                int subId = step.Id + 1;
                SfcStep lowerStep;
                if (allSteps.TryGetValue(subId, out lowerStep))
                {
                    switch (lowerStep.SourceReference.SfcStepType)
                    {
                        case StepType.StartingStep:
                        case StepType.Step:
                            return lowerStep;
                        case StepType.Jump:
                            int reference = StepMaster.GetNameKey(lowerStep.SourceReference.StepName);
                            return allSteps[reference];
                        case StepType.Pass:
                            SfcStep foundStep = FindAlternativeMergeTarget(lowerStep, allSteps);
                            if(foundStep != null)
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
        private static void CollectLeftDependingSteps(SfcStep currentStep, Dictionary<int, SfcStep> allSteps, List<SfcStep> collectedSteps, BranchType targetType, bool upperBranch)
        {
            int leftId = currentStep.Id - (1 << SfcEntity.XKeyShift);
            SfcStep leftStep;
            if (allSteps.TryGetValue(leftId, out leftStep))
            {
                if ((upperBranch && leftStep.SourceReference.UpperBranch == targetType)
                || (!upperBranch && leftStep.SourceReference.LowerBranch == targetType))
                {
                    collectedSteps.Add(leftStep);
                    CollectLeftDependingSteps(leftStep, allSteps, collectedSteps, targetType, upperBranch);
                }
            }
        }
        
        /// <summary>
        /// Looks for right depending steps.
        /// </summary>
        private static void CollectRightDependingSteps(SfcStep currentStep, Dictionary<int, SfcStep> allSteps, List<SfcStep> collectedSteps, BranchType targetType, bool upperBranch)
        {
            if ((upperBranch && currentStep.SourceReference.UpperBranch == targetType)
            || (!upperBranch && currentStep.SourceReference.LowerBranch == targetType))
            {
                int rightId = currentStep.Id + (1 << SfcEntity.XKeyShift);
                SfcStep rightStep;
                if (allSteps.TryGetValue(rightId, out rightStep))
                {
                    collectedSteps.Add(rightStep);
                    CollectRightDependingSteps(rightStep, allSteps, collectedSteps, targetType, upperBranch);
                }
            }
        }
        
        /// <summary>
        /// Looks for an connection from this patch to any upper connected step (including itself).
        /// </summary>
        private static SfcStep FindUpperConnectedStep(SfcStep step, Dictionary<int, SfcStep> allSteps)
        {
            if (step.SourceReference.ContainsRealStep()) // todo: or passing node
            {
                return step;
            }
            else if(step.SourceReference.SfcStepType == StepType.Pass)
            {
                int upperId = step.Id -1;
                SfcStep upperStep;
                if (allSteps.TryGetValue(upperId, out upperStep))
                {
                    return FindUpperConnectedStep(upperStep, allSteps);
                }
            }
            return null;
        }
        
        /// <summary>
        /// Looks for an connected lower step from this patch
        /// </summary>
        private static SfcStep FindLowerConnectedStep(SfcStep step, Dictionary<int, SfcStep> allSteps)
        {
            int subId = step.Id + 1;
            SfcStep lowerStep;
            if (allSteps.TryGetValue(subId, out lowerStep))
            {
                if(lowerStep.SourceReference.ContainsRealStep())
                {
                    return lowerStep;
                }
                else if(step.SourceReference.SfcStepType == StepType.Pass)
                {
                    return FindLowerConnectedStep(lowerStep, allSteps);
                }
                else if(step.SourceReference.SfcStepType == StepType.Jump)
                {
                    int reference = StepMaster.GetNameKey(lowerStep.SourceReference.StepName);
                    return allSteps[reference];
                }
            }
            return null;
        }
        #endregion
    }
}