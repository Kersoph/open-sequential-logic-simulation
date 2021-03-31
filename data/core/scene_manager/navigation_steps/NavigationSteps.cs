using Godot;
using SfcSandbox.Data.Model;

namespace SfcSandbox.Data.Main
{
    /// <summary>
    /// Controls the access to the navigation steps and notifies the upper node.
    /// </summary>
    public class NavigationSteps : Node
    {
        #region ==================== Fields ====================
        private MainNode _mainNode;
        private Button _landingPageButton;
        private Button _sfcStepButton;
        private Button _simulationStepButton;
        private Button _examinationStepButton;
        private Button _exitButton;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            _mainNode = GetNode<MainNode>("..");
            _landingPageButton = GetNode<Button>("LandingPageButton");
            _landingPageButton.Connect("toggled", this, nameof(LandingPageButtonToggled));
            _sfcStepButton = GetNode<Button>("SfcStepButton");
            _sfcStepButton.Connect("toggled", this, nameof(SfcStepButtonToggled));
            _simulationStepButton = GetNode<Button>("SimulationStepButton");
            _simulationStepButton.Connect("toggled", this, nameof(SimulationStepButtonToggled));
            _examinationStepButton = GetNode<Button>("ExaminationStepButton");
            _examinationStepButton.Connect("toggled", this, nameof(ExaminationStepButtonToggled));
            _exitButton = GetNode<Button>("ExitButton");
            _exitButton.Connect("toggled", this, nameof(ExitButtonToggled));
        }
        #endregion
        
        
        #region ==================== Updates ====================
        /// <summary>
        /// Updates the navigation step button visibility to the given view.
        /// </summary>
        public void VisibleViewIs(EditorView view)
        {
            _landingPageButton.Pressed = view == EditorView.LandingPage;
            _sfcStepButton.Pressed = view == EditorView.SfcStep;
            _sfcStepButton.Disabled = view == EditorView.LandingPage;
            _simulationStepButton.Pressed = view == EditorView.SimulationStep;
            _simulationStepButton.Disabled = view == EditorView.LandingPage;
            _examinationStepButton.Pressed = view == EditorView.ExaminationStep;
            _examinationStepButton.Disabled = view == EditorView.LandingPage;
            _exitButton.Pressed = view == EditorView.Exit;
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        private void LandingPageButtonToggled(bool pressed)
        {
            if (pressed)
            {
                _mainNode.UserRequestsChangeTo(EditorView.LandingPage);
            }
            else
            {
                _landingPageButton.Pressed = true; // Simple visual effect
            }
        }
        
        private void SfcStepButtonToggled(bool pressed)
        {
            if (pressed)
            {
                _mainNode.UserRequestsChangeTo(EditorView.SfcStep);
            }
            else
            {
                _sfcStepButton.Pressed = true; // Simple visual effect
            }
        }
        
        private void SimulationStepButtonToggled(bool pressed)
        {
            if (pressed)
            {
                _mainNode.UserRequestsChangeTo(EditorView.SimulationStep);
            }
            else
            {
                _simulationStepButton.Pressed = true; // Simple visual effect
            }
        }
        
        private void ExaminationStepButtonToggled(bool pressed)
        {
            if (pressed)
            {
                _mainNode.UserRequestsChangeTo(EditorView.ExaminationStep);
            }
            else
            {
                _examinationStepButton.Pressed = true; // Simple visual effect
            }
        }
        
        private void ExitButtonToggled(bool pressed)
        {
            _mainNode.UserRequestsChangeTo(EditorView.Exit);
        }
        #endregion
    }
}