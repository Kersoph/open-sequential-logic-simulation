using Godot;


namespace Osls.SfcSimulation.Viewer
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
            GetNode<Sfc2dControls>("..").Sfc2dEditorNode.ZoomOut();
        }
        #endregion
    }
}
