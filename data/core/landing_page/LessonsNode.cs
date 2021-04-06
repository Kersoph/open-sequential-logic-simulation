using Godot;


namespace Osls.LandingPage
{
    /// <summary>
    /// Represents the LessonsNode in the godot UI.
    /// It controls the lesson selection process.
    /// </summary>
    public class LessonsNode : Node
    {
        #region ==================== Fields / Properties ====================
        [Export] private NodePath _landingPagePath = "..";
        [Export] private NodePath _lessonSelectionGridPath = "LessonSelectorContainer/LessonCenterContainer/LessonSelectionGridNode";
        [Export] private NodePath _lessonStartControllerPath = "LessonStartContainer/LessonStartController";
        [Export] private NodePath _lessonInfoPath = "LessonInfo";
        [Export] private NodePath _lessonPreviewPath = "LessonPreview";
        
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
            _landingPageNode = GetNode<LandingPageNode>(_landingPagePath);
            _lessonSelectionGrid = GetNode<LessonSelectionGridNode>(_lessonSelectionGridPath);
            _lessonStartController = GetNode<LessonStartController>(_lessonStartControllerPath);
            _lessonInfo = GetNode<LessonInfo>(_lessonInfoPath);
            _lessonPreview = GetNode<LessonPreview>(_lessonPreviewPath);
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