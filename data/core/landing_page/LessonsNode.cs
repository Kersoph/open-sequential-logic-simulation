using Godot;


namespace Osls.LandingPage
{
    /// <summary>
    /// Represents the LessonsNode in the godot UI.
    /// It controls the lesson selection process.
    /// </summary>
    public class LessonsNode : Node
    {
        #region ==================== Fields ====================
        private const string LandingPagePath = "..";
        private const string LessonSelectionGridPath = "LessonSelectorContainer/LessonCenterContainer/LessonSelectionGridNode";
        private const string LessonStartControllerPath = "LessonStartContainer/LessonStartController";
        private const string LessonInfoPath = "LessonInfo";
        private const string LessonPreviewPath = "LessonPreview";
        
        private LandingPageNode _landingPageNode;
        private LessonSelectionGridNode _lessonSelectionGrid;
        private LessonStartController _lessonStartController;
        private LessonInfo _lessonInfo;
        private LessonPreview _lessonPreview;
        private LessonEntity _selectedLesson;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            _landingPageNode = GetNode<LandingPageNode>(LandingPagePath);
            _lessonSelectionGrid = GetNode<LessonSelectionGridNode>(LessonSelectionGridPath);
            _lessonStartController = GetNode<LessonStartController>(LessonStartControllerPath);
            _lessonInfo = GetNode<LessonInfo>(LessonInfoPath);
            _lessonPreview = GetNode<LessonPreview>(LessonPreviewPath);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Informs the grid, that the selection has changed to this button and the UI needs to be updated.
        /// </summary>
        public void SelectionChangedTo(LessonControllerNode node)
        {
            _selectedLesson = node.LessonEntity;
            _lessonStartController.ActivateStartButton();
            _lessonInfo.UpdateLessonEntity(_selectedLesson);
            _lessonPreview.UpdateLessonEntity(_selectedLesson);
        }

        /// <summary>
        /// The currently selected lesson will be told tho the main view to
        /// switch from the landing page to the SFC editor.
        /// </summary>
        public void StartSelectedLesson()
        {
            _landingPageNode.StartLesson(_selectedLesson);
        }
        #endregion
    }
}