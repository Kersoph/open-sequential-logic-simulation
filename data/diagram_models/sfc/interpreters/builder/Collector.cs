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
        public static List<int> CollectToTransition(int currentId, CollectorEntity entity)
        {
            /*
            CollectLeftDependingSteps(currentId, data, connectedLowerAlternativeSteps, BranchType.Single, false);
            CollectRightDependingSteps(currentId, data, connectedLowerAlternativeSteps, BranchType.Single, false);
            */
            return null;
        }
        
        /// <summary>
        /// Looks for left depending steps.
        /// </summary>
        public static void CollectLeftDependingSteps(int currentId, CollectorEntity entity)
        {
            int leftId = currentId - (1 << SfcEntity.XKeyShift);
            PatchEntity leftStep = entity.Data.SfcEntity.Lookup(leftId);
            if (leftStep != null)
            {
                if ((entity.UpperBranch && leftStep.UpperBranch == entity.TargetType)
                || (!entity.UpperBranch && leftStep.LowerBranch == entity.TargetType))
                {
                    entity.CollectedSteps.Add(leftId);
                    CollectLeftDependingSteps(leftId, entity);
                }
            }
        }
        
        /// <summary>
        /// Looks for right depending steps.
        /// </summary>
        public static void CollectRightDependingSteps(int currentId, CollectorEntity entity)
        {
            PatchEntity step = entity.Data.SfcEntity.Lookup(currentId);
            if ((entity.UpperBranch && step.UpperBranch == entity.TargetType)
            || (!entity.UpperBranch && step.LowerBranch == entity.TargetType))
            {
                int rightId = currentId + (1 << SfcEntity.XKeyShift);
                if (entity.Data.SfcEntity.Lookup(rightId) != null)
                {
                    entity.CollectedSteps.Add(rightId);
                    CollectRightDependingSteps(rightId, entity);
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