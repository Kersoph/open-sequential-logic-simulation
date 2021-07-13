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
        
        /// <summary>
        /// Applies the texture according to targetExecution
        /// </summary>
        public void UpdateTextureTo(ExecutionType targetExecution)
        {
            if (targetExecution == ExecutionType.RunContinuously)
            {
                GetNode<TextureRect>("Icon").Texture = _runContinuouslyTexture;
            }
            else
            {
                GetNode<TextureRect>("Icon").Texture = _pausedTexture;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void OnButtonPressed()
        {
            SfcSimulationViewer viewer = GetNode<Sfc2dControls>("..").SfcSimulationViewer;
            if (viewer.ExecutionType == ExecutionType.RunContinuously)
            {
                UpdateTextureTo(ExecutionType.Paused);
                viewer.ExecutionType = ExecutionType.Paused;
            }
            else
            {
                UpdateTextureTo(ExecutionType.RunContinuously);
                viewer.ExecutionType = ExecutionType.RunContinuously;
            }
        }
        #endregion
    }
}
