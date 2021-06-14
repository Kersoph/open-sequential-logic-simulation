using Godot;


namespace Osls.SfcEditor.Controls
{
    public class EditorZoomIn : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.IsActionPressed("ui_do_more"))
            {
                OnButtonPressed();
            }
        }
        
        public void OnButtonPressed()
        {
            GetNode<SfcEditorNode>("../../..").Sfc2dEditorNode.ZoomIn();
        }
        #endregion
    }
}