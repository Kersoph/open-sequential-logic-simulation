using Godot;


namespace Osls.SfcSimulation.Viewer
{
    public class EditorResetController : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("button_up", this, nameof(ButtonUpEvent));
        }
        
        public void ButtonUpEvent()
        {
            GetNode<Sfc2dControls>("..").SfcSimulationViewer.ResetController();
        }
        #endregion
    }
}