using Godot;


namespace Osls.SfcSimulation.Viewer
{
    public class EditorZoomIn : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.IsActionPressed("ui_do_more"))
            {
                ButtonUpEvent();
            }
        }
        
        public void ButtonUpEvent()
        {
            GetNode<Sfc2dControls>("..").Sfc2dEditorNode.ZoomIn();
        }
        #endregion
    }
}