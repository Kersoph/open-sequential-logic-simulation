using Godot;


namespace Osls.LandingPage
{
    /// <summary>
    /// Topmost node for the lesson patch fields.
    /// Sets the visuals according to the given informations and notifies the upper controller for changes.
    /// </summary>
    public class LessonControllerNode : ReferenceRect
    {
        #region ==================== Fields / Properties ====================
        private LessonSelectionGridNode _gridController;
        
        /// <summary>
        /// Gets the lesson information saved in the lesson folder.
        /// </summary>
        public ILessonEntity LessonEntity { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            GetNode<Button>("LessonButton").Connect("mouse_entered", this, nameof(LessonMouseEntered));
            GetNode<Button>("LessonButton").Connect("toggled", this, nameof(LessonButtonToggled));
        }
        
        /// <summary>
        /// Sets the texts and stores the lesson entity
        /// </summary>
        public void SetLessonInfo(ILessonEntity info, LessonSelectionGridNode gridController)
        {
            LessonEntity = info;
            _gridController = gridController;
            SetTitle(info.Title);
            SetStars(info.Stars);
            if (string.IsNullOrEmpty(info.SimulationPath))
            {
                GetNode<Button>("LessonButton").Disabled = true;
                GetNode<Button>("LessonButton").MouseDefaultCursorShape = CursorShape.Forbidden;
            }
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        /// <summary>
        /// Sets the BBC Title of the button
        /// </summary>
        private void SetTitle(string text)
        {
            RichTextLabel richTextLabel = GetNode<RichTextLabel>("LessonButton/RichTextLabel");
            richTextLabel.BbcodeText = text;
        }
        
        /// <summary>
        /// Event receiver of the LessonButton when the mouse entered the button.
        /// </summary>
        private void LessonMouseEntered()
        {
            _gridController.SelectionChangedTo(this);
        }
        
        /// <summary>
        /// Event receiver of the LessonButton when the button is toggled.
        /// </summary>
        private void LessonButtonToggled(bool buttonPressed)
        {
            if (buttonPressed)
            {
                _gridController.StartSelectedLesson(this);
            }
        }
        
        /// <summary>
        /// Sets the number of stars (0-3) active or inactive
        /// </summary>
        private void SetStars(int stars)
        {
            GetNode<StarVisualNode>("HBoxContainer/StarVisual1").SetToActiveTexture(stars > 0);
            GetNode<StarVisualNode>("HBoxContainer/StarVisual2").SetToActiveTexture(stars > 1);
            GetNode<StarVisualNode>("HBoxContainer/StarVisual3").SetToActiveTexture(stars > 2);
        }
        #endregion
    }
}
