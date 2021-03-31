using Godot;

namespace SfcSandbox.Data.Model.SfcEditor.Controls
{
    public class EditorZoomOut : Button
    {
        public override void _Ready()
        {
            this.Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public void ButtonUpEvent()
        {
            GetNode<SfcEditorNode>("../..").Sfc2dEditorNode.ZoomOut();
        }
    }
}