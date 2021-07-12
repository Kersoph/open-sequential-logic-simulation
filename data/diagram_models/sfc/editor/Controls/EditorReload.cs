using Godot;


namespace Osls.SfcEditor
{
    public class EditorReload : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public void OnButtonPressed()
        {
            GetNode<EditorControls>("..").LoadDiagram();
        }
        
        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.IsActionPressed("main_load"))
            {
                GetNode<EditorControls>("..").LoadDiagram();
            }
        }
        #endregion
    }
}