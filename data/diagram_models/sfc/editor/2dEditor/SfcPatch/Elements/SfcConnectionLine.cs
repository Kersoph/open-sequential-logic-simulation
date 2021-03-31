using Godot;

namespace Osls.SfcEditor
{
    public class SfcConnectionLine : Control
    {
        #region ==================== Properties ====================
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Called when the model has changed or is initialized.
        /// </summary>
        public void UpdateLine(SfcPatchEntity entity)
        {
            GetNode<ColorRect>("TransitionLineTop").Visible = entity.SfcStepType != SfcStepType.Unused && entity.SfcStepType != SfcStepType.Jump;
            GetNode<ColorRect>("TransitionLineMid").Visible = entity.ContainsTransition() || entity.SfcStepType == SfcStepType.Pass;
            GetNode<ColorRect>("TransitionLineBot").Visible = entity.SfcStepType != SfcStepType.Unused;
        }
        
        #endregion
    }
}