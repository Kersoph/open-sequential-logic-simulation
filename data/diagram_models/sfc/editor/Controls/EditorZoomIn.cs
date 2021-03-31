using Godot;

namespace SfcSandbox.Data.Model.SfcEditor.Controls
{
    public class EditorZoomIn : Button
    {
        public override void _Ready()
        {
            this.Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public void ButtonUpEvent()
        {
            GetNode<SfcEditorNode>("../..").Sfc2dEditorNode.ZoomIn();
        }
    }
}