using Godot;


namespace Osls.SfcSimulation.Viewer
{
    public class EditorResetController : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public void OnButtonPressed()
        {
            GetNode<Sfc2dControls>("..").SfcSimulationViewer.ResetController();
        }
        #endregion
    }
}