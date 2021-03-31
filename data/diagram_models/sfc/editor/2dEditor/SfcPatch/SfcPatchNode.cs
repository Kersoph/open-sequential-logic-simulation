using Godot;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Topmost node for the SfcPatchNode.tscn
    /// Boundary class to show the visual representation of the sfc model.
    /// </summary>
    public class SfcPatchNode : ReferenceRect
    {
        #region ==================== Properties ====================
        /// <summary>
        /// The controller for this node
        /// </summary>
        public SfcPatchControl SfcPatchControl { get; private set; }
        #endregion;
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Called only once when the node is created by the patch control.
        /// </summary>
        public void InitializeWith(SfcPatchControl sfcPatchControl, SfcPatchEntity data)
        {
            SfcPatchControl = sfcPatchControl;
            SetCellPosition(data.X, data.Y);
            UpdateNodes(data);
        }
        
        /// <summary>
        /// Updates the noced according to the given data
        /// </summary>
        public void UpdateNodes(SfcPatchEntity data)
        {
            GetNode<SfcStepNode>("SfcStepNode").UpdateStep(data);
            GetNode<SfcLineButton>("SfcLineButtonTop").UpdateBranchLine(data.UpperBranch);
            GetNode<SfcLineButton>("SfcLineButtonBot").UpdateBranchLine(data.LowerBranch);
            GetNode<SfcTransitionNode>("SfcTransition").UpdateTransition(data.TransitionText);
            GetNode<ActionEditorBox>("ActionEditorBox").UpdateActions(data);
            GetNode<SfcConnectionLine>("SfcConnectionLine").UpdateLine(data);
        }
        
        /// <summary>
        /// Marks or unmarks the step if possible
        /// </summary>
        public void MarkStep(bool setMark)
        {
            GetNode<SfcStepNode>("SfcStepNode").MarkStep(setMark);
        }
        #endregion
        
        
        #region ==================== Private ====================
        private void SetCellPosition(int x, int y)
        {
            Vector2 sizeOffset = this.RectMinSize;
            float xOffset = sizeOffset.x * x;
            float yOffset = sizeOffset.y * y;
            this.SetPosition(new Vector2(xOffset, yOffset));
        }
        #endregion
    }
}