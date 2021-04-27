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
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called only once when the node is created by the patch control.
        /// </summary>
        public void InitializeWith(SfcPatchControl sfcPatchControl, PatchEntity data)
        {
            SfcPatchControl = sfcPatchControl;
            SetCellPosition(data.X, data.Y);
            GetNode<SfcTransitionNode>("SfcTransition").Initialise();
            Connect("mouse_entered", this, nameof(OnMouseEntered));
            Connect("mouse_exited", this, nameof(OnMouseExited));
        }
        
        /// <summary>
        /// Updates the patch nodes according to the given data
        /// </summary>
        public void UpdateNodes(PatchEntity data)
        {
            GetNode<SfcStepNode>("SfcStepNode").UpdateStep(data, SfcPatchControl.Master);
            GetNode<SfcLineButton>("SfcLineButtonTop").UpdateBranchLine(data.UpperBranch);
            GetNode<SfcLineButton>("SfcLineButtonBot").UpdateBranchLine(data.LowerBranch);
            GetNode<SfcTransitionNode>("SfcTransition").UpdateTransition(data.TransitionText, SfcPatchControl.Master);
            GetNode<ActionEditorBox>("ActionEditorBox").UpdateActions(data, SfcPatchControl.Master.Data);
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
            Vector2 sizeOffset = RectMinSize;
            float xOffset = sizeOffset.x * x;
            float yOffset = sizeOffset.y * y;
            SetPosition(new Vector2(xOffset, yOffset));
        }
        
        private void OnMouseEntered()
        {
            GetNode<SfcStepNode>("SfcStepNode").HintContent(true);
            GetNode<SfcLineButton>("SfcLineButtonTop").HintContent(true);
            GetNode<SfcLineButton>("SfcLineButtonBot").HintContent(true);
        }
        
        private void OnMouseExited()
        {
            GetNode<SfcStepNode>("SfcStepNode").HintContent(false);
            GetNode<SfcLineButton>("SfcLineButtonTop").HintContent(false);
            GetNode<SfcLineButton>("SfcLineButtonBot").HintContent(false);
        }
        #endregion
    }
}