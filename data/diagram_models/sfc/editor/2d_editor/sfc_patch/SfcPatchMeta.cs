namespace Osls.SfcEditor
{
    public class SfcPatchMeta
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// True if this patch can connect to a lower patch.
        /// </summary>
        public bool RequestsLowerStepConnection { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Updates the metadata according to the data model.
        /// </summary>
        public void UpdatePatch(SfcPatchControl control)
        {
            CheckLowerStepConnection(control);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Checks if this patch can connect to a existing lower patch with:
        /// - Incoming line left/right
        /// - Transition form this patch
        /// </summary>
        private void CheckLowerStepConnection(SfcPatchControl control)
        {
            PatchEntity lowerPatch = control.Master.Data.SfcEntity.Lookup(control.Data.X, control.Data.Y + 1);
            bool hasLowerActiveStep = lowerPatch != null && lowerPatch.SfcStepType != StepType.Unused;
            if (hasLowerActiveStep)
            {
                PatchEntity leftPatch = control.Master.Data.SfcEntity.Lookup(control.Data.X - 1, control.Data.Y);
                bool hasLeftConnection = leftPatch != null && leftPatch.LowerBranch != BranchType.Unused;
                bool hasRightConnection = control.Data.LowerBranch != BranchType.Unused;
                bool hasTransition = control.Data.ContainsTransition() || control.Data.SfcStepType == StepType.Pass;
                RequestsLowerStepConnection = hasLeftConnection || hasTransition || hasRightConnection;
            }
            else
            {
                RequestsLowerStepConnection = false;
            }
        }
        #endregion
    }
}