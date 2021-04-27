using Godot;

namespace Osls.SfcEditor
{
    public class SfcConnectionLine : Control
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the model has changed or is initialized.
        /// </summary>
        public void UpdateLine(PatchEntity entity)
        {
            GetNode<ColorRect>("TransitionLineTop").Visible = entity.SfcStepType != StepType.Unused && entity.SfcStepType != StepType.Jump;
            GetNode<ColorRect>("TransitionLineMid").Visible = entity.ContainsTransition() || entity.SfcStepType == StepType.Pass;
            GetNode<ColorRect>("TransitionLineBot").Visible = entity.SfcStepType != StepType.Unused;
        }
        
        #endregion
    }
}