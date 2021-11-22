using Godot;


namespace Osls.Plants.Sandbox
{
    /// <summary>
    /// Future: Check possibilities to make the lists dynamic
    /// </summary>
    public class DigitalInputs : ScrollContainer
    {
        #region ==================== Fields / Properties ====================
        private Button[] _buttons;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public void SetupUi()
        {
            _buttons = new Button[9];
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i] = GetNode<Button>("List/DI" + (i + 1));
            }
        }
        
        /// <summary>
        /// Updates the IO of the UI
        /// </summary>
        public void UpdateModel(Sandbox master)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                master.SimulationOutput.SetValue(_buttons[i].Name, _buttons[i].Pressed);
            }
        }
        #endregion
    }
}
