using Godot;


namespace Osls.SfcSimulation.Viewer
{
    public class ExecutionControl : Button
    {
        #region ==================== Fields / Properties ====================
        [Export] private Texture _runContinuouslyTexture;
        [Export] private Texture _pausedTexture;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("main_pause"))
            {
                OnButtonPressed();
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void OnButtonPressed()
        {
            SfcSimulationViewer viewer = GetNode<Sfc2dControls>("..").SfcSimulationViewer;
            if (viewer.ExecutionType == ExecutionType.RunContinuously)
            {
                viewer.ExecutionType = ExecutionType.Paused;
                GetNode<TextureRect>("Icon").Texture = _pausedTexture;
            }
            else
            {
                viewer.ExecutionType = ExecutionType.RunContinuously;
                GetNode<TextureRect>("Icon").Texture = _runContinuouslyTexture;
            }
        }
        #endregion
    }
}
