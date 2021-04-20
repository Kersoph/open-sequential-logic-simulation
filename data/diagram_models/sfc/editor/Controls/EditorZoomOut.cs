using Godot;


namespace Osls.SfcEditor.Controls
{
    public class EditorZoomOut : Button
    {
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public void OnButtonPressed()
        {
            GetNode<SfcEditorNode>("../../..").Sfc2dEditorNode.ZoomOut();
        }
    }
}