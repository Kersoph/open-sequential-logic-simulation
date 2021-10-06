using Godot;

namespace Osls.Plants.Sandbox
{
    /// <summary>
    /// Future: Check possibilities to make the lists dynamic
    /// </summary>
    public class AnalogueIO : ScrollContainer
    {
        #region ==================== Fields / Properties ====================
        private SpinBox[] _inputs;
        private Label[] _outputs;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public void SetupUi()
        {
            _inputs = new SpinBox[3];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = GetNode<SpinBox>("List/AI" + (i + 1));
            }
            _outputs = new Label[3];
            for (int i = 0; i < _outputs.Length; i++)
            {
                _outputs[i] = GetNode<Label>("List/AO" + (i + 1));
            }
        }
        
        /// <summary>
        /// Updates the IO of the UI
        /// </summary>
        public void UpdateModel(Sandbox master)
        {
            for (int i = 0; i < _inputs.Length; i++)
            {
                int result;
                if (_inputs[i].Value >= int.MaxValue)
                {
                    result = int.MaxValue;
                }
                else if (_inputs[i].Value <= int.MinValue)
                {
                    result = int.MinValue;
                }
                else
                {
                    result = System.Convert.ToInt32(_inputs[i].Value);
                }
                master.SimulationOutput.SetValue(_inputs[i].Name, result);
            }
            for (int i = 0; i < _outputs.Length; i++)
            {
                _outputs[i].Text = _outputs[i].Name + ": " + master.SimulationInput.PollInteger(_outputs[i].Name);
            }
        }
        #endregion
    }
}
