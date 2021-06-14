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
            GetNode<SfcEditorNode>("../../..").TryLoadDiagram();
        }
        
        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.IsActionPressed("main_load"))
            {
                GetNode<SfcEditorNode>("../../..").TryLoadDiagram();
            }
        }
        #endregion
    }
}