using Godot;


namespace Osls.SfcSimulation.EditorControls
{
    public class EditorResetController : Button
    {
        #region ==================== Fields / Properties ====================
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