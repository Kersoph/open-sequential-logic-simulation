using Godot;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.Viewer
{
    public class MouseFilter : ColorRect
    {
        #region ==================== Fields Properties ====================
        [Export] private NodePath _sfc2dEditorPath = "../Sfc2dEditor";
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Using secondary move buttons when they are not used for another control.
        /// </summary>
        public override void _GuiInput(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_translate_idle"))
            {
                GetNode<Sfc2dEditorNode>(_sfc2dEditorPath).StartDrag();
            }
            else if (@event.IsActionReleased("ui_translate_idle"))
            {
                GetNode<Sfc2dEditorNode>(_sfc2dEditorPath).StopDrag();
            }
        }
        #endregion
    }
}
