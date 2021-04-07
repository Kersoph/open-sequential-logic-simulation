using System.Collections.Generic;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.Engine.Builder
{
    public static class Collector
    {
        #region ==================== Nested ====================
        private class CollectorEntity
        {
            public SfcProgrammData Data { get; }
            public List<int> CollectedSteps { get; }
            public BranchType TargetType { get; }
            public bool UpperBranch { get; }
            
            /// <summary>
            /// Creates a new collextor entity to hold the data
            /// </summary>
            /// <param name="data">program reference</param>
            /// <param name="targetType">whoch branch type to search</param>
            /// <param name="upperBranch">true for upper, false for lower</param>
            public CollectorEntity(SfcProgrammData data, BranchType targetType, bool upperBranch)
            {
                Data = data;
                CollectedSteps = new List<int>();
                TargetType = targetType;
                UpperBranch = upperBranch;
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Collects all steps horizontally connected with the given targetType from sourceId
        /// </summary>
        public static List<int> CollectHorizontal(int sourceId, SfcProgrammData data, BranchType targetType, bool upperBranch)
        {
            CollectorEntity entity = new CollectorEntity(data, targetType, upperBranch);
            entity.CollectedSteps.Add(sourceId);
            CollectLeftDependingSteps(sourceId, entity);
            CollectRightDependingSteps(sourceId, entity);
            return entity.CollectedSteps;
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
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Looks for left depending steps.
        /// </summary>
        private static void CollectLeftDependingSteps(int currentId, CollectorEntity entity)
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
        private static void CollectRightDependingSteps(int currentId, CollectorEntity entity)
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
        #endregion
    }
}