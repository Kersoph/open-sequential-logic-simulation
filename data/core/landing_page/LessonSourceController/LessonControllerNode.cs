using Godot;

namespace SfcSandbox.Data.Model.LandingPage.LessonSourceController
{
    /// <summary>
    /// Topmost node for the lesson patch fields.
    /// Sets the visuals according to the given informations and notifies the upper controller for changes.
    /// </summary>
    public class LessonControllerNode : ReferenceRect
    {
        #region ==================== Fields ====================
        /// <summary>
        /// Gets the lesson information savend in the lesson folder.
        /// </summary>
        public LessonEntity LessonEntity { get; private set; }
        
        private LessonSelectionGridNode _gridController;
        #endregion


        #region ==================== Updates ====================
        public override void _Ready()
        {
            GetNode<Button>("LessonButton").Connect("toggled", this, nameof(LessonButtonToggled));
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Sets the texts and stores the lesson entity
        /// </summary>
        public void SetLessonInfo(LessonEntity info, LessonSelectionGridNode gridController)
        {
            LessonEntity = info;
            _gridController = gridController;
            SetTitle(info.Title);
            SetStars(info.Stars);
        }
        
        /// <summary>
        /// Resets the button to not pressed.
        /// </summary>
        public void ResetButtonStatus()
        {
            Button lessonButton = GetNode<Button>("LessonButton");
            lessonButton.SetPressed(false);
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
        /// Event receiver of the LessonButton when the button is toggeled.
        /// </summary>
        private void LessonButtonToggled(bool buttonPressed)
        {
            if (buttonPressed)
            {
                _gridController.SelectionChangedTo(this);
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