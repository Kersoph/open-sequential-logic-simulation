using System.Collections.Generic;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.Engine.Builder
{
    public static class Collector
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Looks for left depending steps.
        /// </summary>
        public static void CollectLeftDependingSteps(int currentId, SfcProgrammData data, List<int> collectedSteps, BranchType targetType, bool upperBranch)
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
        public static void CollectRightDependingSteps(int currentId, SfcProgrammData data, List<int> collectedSteps, BranchType targetType, bool upperBranch)
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
        public static SfcStep FindUpperConnectedStep(int id, SfcProgrammData data)
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
        public static SfcStep FindLowerConnectedStep(int id, SfcProgrammData data)
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