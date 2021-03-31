using Godot;


namespace Osls.SfcEditor
{
    public class EditorLoad : Button
    {
        public override void _Ready()
        {
            Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public void ButtonUpEvent()
        {
            GetNode<SfcEditorNode>("../..").TryLoadDiagram();
        }
    }
}