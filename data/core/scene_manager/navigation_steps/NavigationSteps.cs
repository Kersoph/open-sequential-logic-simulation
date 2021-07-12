using Godot;


namespace Osls.Navigation
{
    /// <summary>
    /// Controls the access to the navigation steps and notifies the upper node.
    /// Careful: With the update of 3.2+ the button behaviour changed with the events.
    /// </summary>
    public class NavigationSteps : Node
    {
        #region ==================== Fields ====================
        private IMainNode _mainNode;
        private Button _landingPageButton;
        private Button _sfcStepButton;
        private Button _simulationStepButton;
        private Button _examinationStepButton;
        private Button _exitButton;
        private PageCategory _currentPage;
        #endregion
        
        
        #region ==================== Public Methods ====================
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
        
        /// <summary>
        /// Updates the navigation step button visibility to the given view.
        /// </summary>
        public void VisibleViewIs(PageCategory view)
        {
            _currentPage = view;
            _landingPageButton.Pressed = view == PageCategory.LandingPage;
            _sfcStepButton.Pressed = view == PageCategory.LogicEditor;
            _sfcStepButton.Disabled = view == PageCategory.LandingPage;
            _simulationStepButton.Pressed = view == PageCategory.Simulation;
            _simulationStepButton.Disabled = view == PageCategory.LandingPage;
            _examinationStepButton.Pressed = view == PageCategory.Examination;
            _examinationStepButton.Disabled = view == PageCategory.LandingPage;
            _exitButton.Pressed = view == PageCategory.Exit;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void LandingPageButtonToggled(bool pressed)
        {
            UpdateButtonState(_landingPageButton, PageCategory.LandingPage, pressed);
        }
        
        private void SfcStepButtonToggled(bool pressed)
        {
            UpdateButtonState(_sfcStepButton, PageCategory.LogicEditor, pressed);
        }
        
        private void SimulationStepButtonToggled(bool pressed)
        {
            UpdateButtonState(_simulationStepButton, PageCategory.Simulation, pressed);
        }
        
        private void ExaminationStepButtonToggled(bool pressed)
        {
            UpdateButtonState(_examinationStepButton, PageCategory.Examination, pressed);
        }
        
        private void ExitButtonToggled(bool pressed)
        {
            UpdateButtonState(_exitButton, PageCategory.Exit, pressed);
        }
        
        private void UpdateButtonState(Button button, PageCategory pages, bool pressed)
        {
            if (pressed)
            {
                _mainNode.UserRequestsChangeTo(pages);
            }
            else
            {
                if (_currentPage == pages)
                {
                    _mainNode.UserRequestsChangeTo(pages);
                }
            }
        }
        #endregion
    }
}