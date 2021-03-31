using Godot;
using SfcSandbox.Data.Model.SfcEditor;


namespace SfcSandbox.Data.Model.SfcSimulation.EditorControls
{
    public class EditorZoomIn : Button
    {
        public override void _Ready()
        {
            this.Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public void ButtonUpEvent()
        {
            GetNode<Sfc2dEditorNode>("../../Sfc2dEditor").ZoomIn();
        }
    }
}