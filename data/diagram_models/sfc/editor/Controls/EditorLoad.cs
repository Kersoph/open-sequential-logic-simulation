using Godot;


namespace Osls.SfcEditor
{
    public class EditorLoad : Button
    {
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public void OnButtonPressed()
        {
            GetNode<SfcEditorNode>("../../..").TryLoadDiagram();
        }
    }
}