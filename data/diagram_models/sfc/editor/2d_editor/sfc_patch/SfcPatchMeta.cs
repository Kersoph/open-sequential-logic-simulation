namespace Osls.SfcEditor
{
    public class SfcPatchMeta
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// True if this patch can connect to a lower patch.
        /// </summary>
        public bool RequestsLowerStepConnection { get; private set; }
        
        /// <summary>
        /// True if this patch can skip the transition to a pass patch.
        /// This is used when the upper branch os preformed somewhere else.
        /// For example when a simultaneous merge is stated further down the path.
        /// </summary>
        public bool HasPossibleTransitionSkip { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Updates the metadata according to the data model.
        /// </summary>
        public void UpdatePatch(SfcPatchControl control)
        {
            HasPossibleTransitionSkip = CheckPossibleTransitionSkip(control.Master, control.Data);
            RequestsLowerStepConnection = CheckLowerStepConnection(control, HasPossibleTransitionSkip);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Checks if this patch can connect to a existing lower patch with:
        /// - Incoming line left/right
        /// - Transition form this patch
        /// </summary>
        private static bool CheckLowerStepConnection(SfcPatchControl control, bool skippedTransition)
        {
            PatchEntity lowerPatch = control.Master.Data.SfcEntity.Lookup(control.Data.X, control.Data.Y + 1);
            bool hasLowerActiveStep = lowerPatch != null && lowerPatch.SfcStepType != StepType.Unused;
            if (!hasLowerActiveStep) return false;
            if (skippedTransition || control.Data.ContainsTransition()) return true;
            PatchEntity leftPatch = control.Master.Data.SfcEntity.Lookup(control.Data.X - 1, control.Data.Y);
            return HasLowerBranchConnection(control.Data, leftPatch);
        }
        
        /// <summary>
        /// Checks if this patch can skip the transition if it is connected to
        /// StepType.Pass with a upper BranchType.
        /// </summary>
        private static bool CheckPossibleTransitionSkip(Sfc2dEditorControl control, PatchEntity sourcePatch)
        {
            if (sourcePatch.ContainsTransition()) return false;
            if (sourcePatch.ContainsRealStep())
            {
                PatchEntity leftPatch = control.Data.SfcEntity.Lookup(sourcePatch.X - 1, sourcePatch.Y);
                if (HasUpperBranchConnection(sourcePatch, leftPatch)) return false;
                if (HasLowerBranchConnection(sourcePatch, leftPatch)) return false;
                return FindUpperBranchConnectionsThroughPasses(control, sourcePatch);
            }
            else if (sourcePatch.SfcStepType == StepType.Pass)
            {
                PatchEntity leftPatch = control.Data.SfcEntity.Lookup(sourcePatch.X - 1, sourcePatch.Y);
                return !HasUpperBranchConnection(sourcePatch, leftPatch);
            }
            return false;
        }
        
        /// <summary>
        /// Traverses down along passes and tries to find a patch with upper connections but without lower ones.
        /// </summary>
        private static bool FindUpperBranchConnectionsThroughPasses(Sfc2dEditorControl control, PatchEntity currentPatch)
        {
            PatchEntity lowerPatch = control.Data.SfcEntity.Lookup(currentPatch.X, currentPatch.Y + 1);
            bool hasLowerTransitionStep = lowerPatch != null && lowerPatch.SfcStepType == StepType.Pass;
            if (!hasLowerTransitionStep) return false;
            PatchEntity leftPatch = control.Data.SfcEntity.Lookup(lowerPatch.X - 1, lowerPatch.Y);
            if (HasUpperBranchConnection(lowerPatch, leftPatch)) return true;
            if (HasLowerBranchConnection(lowerPatch, leftPatch)) return false;
            return FindUpperBranchConnectionsThroughPasses(control, lowerPatch);
        }
        
        private static bool HasUpperBranchConnection(PatchEntity patch, PatchEntity leftPatch)
        {
            bool hasUpperLeftConnection = leftPatch != null && leftPatch.UpperBranch != BranchType.Unused;
            bool hasUpperRightConnection = patch.UpperBranch != BranchType.Unused;
            return hasUpperLeftConnection || hasUpperRightConnection;
        }
        
        private static bool HasLowerBranchConnection(PatchEntity patch, PatchEntity leftPatch)
        {
            bool hasLowerLeftConnection = leftPatch != null && leftPatch.LowerBranch != BranchType.Unused;
            bool hasLowerRightConnection = patch.LowerBranch != BranchType.Unused;
            return hasLowerLeftConnection || hasLowerRightConnection;
        }
        #endregion
    }
}
