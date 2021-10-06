using Godot;


namespace Osls.Plants.Sandbox
{
    /// <summary>
    /// Future: Check possibilities to make the lists dynamic
    /// </summary>
    public class DigitalOutputs : ScrollContainer
    {
        #region ==================== Fields / Properties ====================
        [Export] private StyleBox _styleNormal;
        [Export] private StyleBox _styleActive;
        private Label[] _outputs;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public void SetupUi()
        {
            _outputs = new Label[9];
            for (int i = 0; i < _outputs.Length; i++)
            {
                _outputs[i] = GetNode<Label>("List/DO" + (i + 1));
            }
        }
        
        /// <summary>
        /// Updates the IO of the UI
        /// </summary>
        public void UpdateModel(Sandbox master)
        {
            for (int i = 0; i < _outputs.Length; i++)
            {
                bool active = master.SimulationInput.PollBoolean(_outputs[i].Name);
                _outputs[i].AddStyleboxOverride("normal", active ? _styleActive : _styleNormal);
            }
        }
        #endregion
    }
}
