using Godot;


namespace Osls.SfcEditor
{
    public class SfcConnectionLine : Control
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the model has changed or is initialized.
        /// </summary>
        public void UpdateLine(PatchEntity entity, SfcPatchControl control)
        {
            GetNode<ColorRect>("TransitionLineTop").Visible = entity.SfcStepType != StepType.Unused && entity.SfcStepType != StepType.Jump;
            GetNode<ColorRect>("TransitionLineMid").Visible = entity.ContainsTransition() || control.SfcPatchMeta.HasPossibleTransitionSkip;
            GetNode<ColorRect>("TransitionLineBot").Visible = control.SfcPatchMeta.RequestsLowerStepConnection;
        }
        #endregion
    }
}
