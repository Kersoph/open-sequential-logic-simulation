using Godot;


namespace Osls.SfcEditor.Controls
{
    public class EditorSave : Button
    {
        public override void _Ready()
        {
            Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public void ButtonUpEvent()
        {
            GetNode<SfcEditorNode>("../..").SaveDiagram();
        }
    }
}