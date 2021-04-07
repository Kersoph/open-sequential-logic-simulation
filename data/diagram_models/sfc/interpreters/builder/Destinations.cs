using System.Collections.Generic;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.Engine.Builder
{
    public static class Destinations
    {
        #region ==================== Public Methods ====================
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
        /// Collects the list of lower simultanuous branches if possible.
        /// Null if there is an error in the diagram
        /// </summary>
        private static List<SfcStep> CollectLowerSimultaneousBranches(int holder, SfcProgrammData data)
        {
            List<int> collected = Collector.CollectHorizontal(holder, data, BranchType.Double, false);
            List<SfcStep> targetSteps = new List<SfcStep>();
            foreach (int step in collected)
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
                            SfcStep foundStep = Collector.FindLowerConnectedStep(lowerStep.Key, data);
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
            List<int> collected = Collector.CollectHorizontal(holder, data, BranchType.Single, false);
            foreach (int step in collected)
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