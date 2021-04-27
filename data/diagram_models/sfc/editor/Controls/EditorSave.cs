using Godot;


namespace Osls.SfcEditor.Controls
{
    public class EditorSave : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public void OnButtonPressed()
        {
            GetNode<SfcEditorNode>("../../..").SaveDiagram();
        }
        
        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.IsActionPressed("main_save"))
            {
                GetNode<SfcEditorNode>("../../..").SaveDiagram();
            }
        }
        #endregion
    }
}