using Godot;


namespace Osls.SfcEditor.Controls
{
    public class EditorZoomOut : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.IsActionPressed("ui_do_less"))
            {
                OnButtonPressed();
            }
        }
        
        public void OnButtonPressed()
        {
            GetNode<EditorControls>("..").SfcEditorNode.Sfc2dEditorNode.ZoomOut();
        }
        #endregion
    }
}
