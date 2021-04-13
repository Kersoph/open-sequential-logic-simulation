using Godot;


namespace Osls.LandingPage
{
    /// <summary>
    /// Controls the lesson start button.
    /// Tells the Lesson node when the user wants to start the selected lesson.
    /// </summary>
    public class LessonStartController : CenterContainer
    {
        #region ==================== Fields / Properties ====================
        [Export] private NodePath _lessonsNodePath = "../..";
        private Button _startButton;
        private LessonsNode _lessonsNode;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            _startButton = GetNode<Button>("LessonStartButton");
            _startButton.Connect("pressed", this, nameof(LessonStartButtonPressed));
            _lessonsNode = GetNode<LessonsNode>(_lessonsNodePath);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Activates the lesson start button.
        /// </summary>
        public void ActivateStartButton()
        {
            _startButton.Disabled = false;
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        private void LessonStartButtonPressed()
        {
            _lessonsNode.StartSelectedLesson();
        }
        #endregion
        
    }
}