using Godot;

namespace SfcSandbox.Data.Model.SfcEditor.Controls
{
    public class EditorSave : Button
    {
        public override void _Ready()
        {
            this.Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public void ButtonUpEvent()
        {
            GetNode<SfcEditorNode>("../..").SaveDiagram();
        }
    }
}