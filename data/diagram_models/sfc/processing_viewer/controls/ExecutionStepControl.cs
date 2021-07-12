using Godot;


namespace Osls.SfcSimulation.Viewer
{
    public class ExecutionStepControl : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("main_step_one", true))
            {
                OnButtonPressed();
                
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        private void OnButtonPressed()
        {
            SfcSimulationViewer viewer = GetNode<Sfc2dControls>("..").SfcSimulationViewer;
            if (viewer.ExecutionType == ExecutionType.Paused)
            {
                viewer.ExecutionType = ExecutionType.RunOneStep;
            }
        }
        #endregion
    }
}
