using Godot;


namespace Osls.SfcEditor.Controls
{
    public class EditorZoomIn : Button
    {
        public override void _Ready()
        {
            Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public void ButtonUpEvent()
        {
            GetNode<SfcEditorNode>("../..").Sfc2dEditorNode.ZoomIn();
        }
    }
}