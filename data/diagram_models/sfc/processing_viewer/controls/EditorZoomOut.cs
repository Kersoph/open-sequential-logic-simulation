using Godot;


namespace Osls.SfcSimulation.EditorControls
{
    public class EditorZoomOut : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.IsActionPressed("ui_do_less"))
            {
                ButtonUpEvent();
            }
        }
        
        public void ButtonUpEvent()
        {
            GetNode<Sfc2dControls>("..").Sfc2dEditorNode.ZoomOut();
        }
        #endregion
    }
}