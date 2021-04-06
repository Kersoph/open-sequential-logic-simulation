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
        public static void AssignTransitionDestinations(SfcTransition transition, SfcStep holder, SfcProgrammData data)
        {
            List<SfcStep> lowerTargets = CollectLowerSimultaneousBranches(holder, data);
            if (lowerTargets == null || lowerTargets.Count > 0)
            {
                transition.NextSteps = lowerTargets;
            }
            else
            {
                SfcStep alternativeStep = FindAlternativeMergeTarget(holder, data);
                List<SfcStep> alternativeList = new List<SfcStep>(1);
                if (alternativeStep != null) alternativeList.Add(alternativeStep);
                transition.NextSteps = alternativeList;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Collects all alternative branches to this step.
        /// if it returns only one transition is not an alternative branch.
        /// </summary>
        private static List<SfcTransition> CollectUpperAlternativeBranches(SfcStep source, SfcProgrammData data)
        {
            List<SfcTransition> transitions = new List<SfcTransition>();
            List<SfcStep> collectedUpperAlternatingSteps = new List<SfcStep>() { source };
            CollectLeftDependingSteps(source, data, collectedUpperAlternatingSteps, BranchType.Single, true);
            CollectRightDependingSteps(source, data, collectedUpperAlternatingSteps, BranchType.Single, true);
            if (collectedUpperAlternatingSteps.Count > 0)
            {
                foreach (SfcStep step in collectedUpperAlternatingSteps)
                {
                    if (data.SfcEntity.Lookup(step.Id).ContainsTransition())
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
        private static SfcTransition CreateTransition(SfcStep source, SfcProgrammData data)
        {
            string transitionText = data.SfcEntity.Lookup(source.Id).TransitionText;
            BooleanExpression expression = TransitionMaster.InterpretTransitionText(transitionText, data.StepMaster);
            SfcTransition transition = new SfcTransition(expression);
            return transition;
        }
        
        /// <summary>
        /// Collects all Simultaneous branches to merge to this step.
        /// null if there is none.
        /// </summary>
        private static SfcTransition CollectUpperSimultaneousMerge(SfcStep source, SfcProgrammData data)
        {
            SfcTransition transition = null;
            List<SfcStep> collectedUpperSimultaneousSteps = new List<SfcStep>() { source };
            CollectLeftDependingSteps(source, data, collectedUpperSimultaneousSteps, BranchType.Double, true);
            CollectRightDependingSteps(source, data, collectedUpperSimultaneousSteps, BranchType.Double, true);
            // TODO: Sucher auch tiefere
            if (collectedUpperSimultaneousSteps.Count > 1)
            {
                SfcStep transitionStep = null;
                foreach (SfcStep step in collectedUpperSimultaneousSteps)
                {
                    if (data.SfcEntity.Lookup(source.Id).ContainsTransition())
                    {
                        transitionStep = step;
                        break;
                    }
                }
                if (transitionStep == null) Godot.GD.PushError("It is not allowed wo have Simultaneous branches without one transition! " + source.Id);
                List<SfcStep> connectedSteps = new List<SfcStep>();
                foreach (SfcStep step in collectedUpperSimultaneousSteps)
                {
                    SfcStep connectedStep = FindUpperConnectedStep(step, data);
                    if (connectedStep != null)
                    {
                        connectedSteps.Add(connectedStep);
                    }
                }
                if (connectedSteps.Count < 2) Godot.GD.PushError("It does not make sense to merge one branch. " + source.Id);
                transition = CreateTransition(transitionStep, data);
                transition.DependingSteps = connectedSteps;
            }
            return transition;
        }

        /// <summary>
        /// Collects the list of lower simultanuous branches if possible.
        /// Null if there is an error in the diagram
        /// </summary>
        private static List<SfcStep> CollectLowerSimultaneousBranches(SfcStep holder, SfcProgrammData data)
        {
            List<SfcStep> connectedLowerSimultaneousSteps = new List<SfcStep>() { holder };
            CollectLeftDependingSteps(holder, data, connectedLowerSimultaneousSteps, BranchType.Double, false);
            CollectRightDependingSteps(holder, data, connectedLowerSimultaneousSteps, BranchType.Double, false);
            List<SfcStep> targetSteps = new List<SfcStep>();
            foreach (SfcStep step in connectedLowerSimultaneousSteps)
            {
                int subId = step.Id + 1;
                SfcStep lowerStep;
                if (data.ControlMap.TryGetValue(subId, out lowerStep))
                {
                    switch (data.SfcEntity.Lookup(lowerStep.Id).SfcStepType)
                    {
                        case StepType.StartingStep:
                        case StepType.Step:
                            targetSteps.Add(lowerStep);
                            break;
                        case StepType.Jump:
                            if (data.StepMaster.ContainsStep(data.SfcEntity.Lookup(lowerStep.Id).StepName))
                            {
                                int reference = data.StepMaster.GetNameKey(data.SfcEntity.Lookup(lowerStep.Id).StepName);
                                targetSteps.Add(data.ControlMap[reference]);
                            }
                            else
                            {
                                return null;
                            }
                            break;
                        case StepType.Pass:
                            SfcStep foundStep = FindLowerConnectedStep(lowerStep, data);
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
        
        private static SfcStep FindAlternativeMergeTarget(SfcStep holder, SfcProgrammData data)
        {
            List<SfcStep> connectedLowerAlternativeSteps = new List<SfcStep>() { holder };
            CollectLeftDependingSteps(holder, data, connectedLowerAlternativeSteps, BranchType.Single, false);
            CollectRightDependingSteps(holder, data, connectedLowerAlternativeSteps, BranchType.Single, false);
            foreach (SfcStep step in connectedLowerAlternativeSteps)
            {
                int subId = step.Id + 1;
                SfcStep lowerStep;
                if (data.ControlMap.TryGetValue(subId, out lowerStep))
                {
                    switch (data.SfcEntity.Lookup(lowerStep.Id).SfcStepType)
                    {
                        case StepType.StartingStep:
                        case StepType.Step:
                            return lowerStep;
                        case StepType.Jump:
                            int reference = data.StepMaster.GetNameKey(data.SfcEntity.Lookup(lowerStep.Id).StepName);
                            return data.ControlMap[reference];
                        case StepType.Pass:
                            SfcStep foundStep = FindAlternativeMergeTarget(lowerStep, data);
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
        private static void CollectLeftDependingSteps(SfcStep currentStep, SfcProgrammData data, List<SfcStep> collectedSteps, BranchType targetType, bool upperBranch)
        {
            int leftId = currentStep.Id - (1 << SfcEntity.XKeyShift);
            SfcStep leftStep;
            if (data.ControlMap.TryGetValue(leftId, out leftStep))
            {
                if ((upperBranch && data.SfcEntity.Lookup(leftStep.Id).UpperBranch == targetType)
                || (!upperBranch && data.SfcEntity.Lookup(leftStep.Id).LowerBranch == targetType))
                {
                    collectedSteps.Add(leftStep);
                    CollectLeftDependingSteps(leftStep, data, collectedSteps, targetType, upperBranch);
                }
            }
        }
        
        /// <summary>
        /// Looks for right depending steps.
        /// </summary>
        private static void CollectRightDependingSteps(SfcStep currentStep, SfcProgrammData data, List<SfcStep> collectedSteps, BranchType targetType, bool upperBranch)
        {
            if ((upperBranch && data.SfcEntity.Lookup(currentStep.Id).UpperBranch == targetType)
            || (!upperBranch && data.SfcEntity.Lookup(currentStep.Id).LowerBranch == targetType))
            {
                int rightId = currentStep.Id + (1 << SfcEntity.XKeyShift);
                SfcStep rightStep;
                if (data.ControlMap.TryGetValue(rightId, out rightStep))
                {
                    collectedSteps.Add(rightStep);
                    CollectRightDependingSteps(rightStep, data, collectedSteps, targetType, upperBranch);
                }
            }
        }
        
        /// <summary>
        /// Looks for an connection from this patch to any upper connected step (including itself).
        /// </summary>
        private static SfcStep FindUpperConnectedStep(SfcStep step, SfcProgrammData data)
        {
            if (data.SfcEntity.Lookup(step.Id).ContainsRealStep()) // todo: or passing node
            {
                return step;
            }
            else if(data.SfcEntity.Lookup(step.Id).SfcStepType == StepType.Pass)
            {
                int upperId = step.Id -1;
                SfcStep upperStep;
                if (data.ControlMap.TryGetValue(upperId, out upperStep))
                {
                    return FindUpperConnectedStep(upperStep, data);
                }
            }
            return null;
        }
        
        /// <summary>
        /// Looks for an connected lower step from this patch
        /// </summary>
        private static SfcStep FindLowerConnectedStep(SfcStep step, SfcProgrammData data)
        {
            int subId = step.Id + 1;
            SfcStep lowerStep;
            if (data.ControlMap.TryGetValue(subId, out lowerStep))
            {
                if(data.SfcEntity.Lookup(lowerStep.Id).ContainsRealStep())
                {
                    return lowerStep;
                }
                else if(data.SfcEntity.Lookup(step.Id).SfcStepType == StepType.Pass)
                {
                    return FindLowerConnectedStep(lowerStep, data);
                }
                else if(data.SfcEntity.Lookup(step.Id).SfcStepType == StepType.Jump)
                {
                    int reference = data.StepMaster.GetNameKey(data.SfcEntity.Lookup(lowerStep.Id).StepName);
                    return data.ControlMap[reference];
                }
            }
            return null;
        }
        #endregion
    }
}