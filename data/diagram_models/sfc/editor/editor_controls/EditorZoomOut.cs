using Godot;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.EditorControls
{
    public class EditorZoomOut : Button
    {
        public override void _Ready()
        {
            Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public void ButtonUpEvent()
        {
            GetNode<Sfc2dEditorNode>("../../Sfc2dEditor").ZoomOut();
        }
    }
}