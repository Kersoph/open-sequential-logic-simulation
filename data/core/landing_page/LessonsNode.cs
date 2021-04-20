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
        [Export] private NodePath _lessonInfoPath = "LessonInfo";
        [Export] private NodePath _lessonPreviewPath = "LessonPreview";
        
        private LandingPageNode _landingPageNode;
        private LessonInfo _lessonInfo;
        private LessonPreview _lessonPreview;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            _landingPageNode = GetNode<LandingPageNode>(_landingPagePath);
            _lessonInfo = GetNode<LessonInfo>(_lessonInfoPath);
            _lessonPreview = GetNode<LessonPreview>(_lessonPreviewPath);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Informs the grid, that the selection has changed to this button and the UI needs to be updated.
        /// </summary>
        public void SelectionChangedTo(LessonEntity selectedLesson)
        {
            _lessonInfo.UpdateLessonEntity(selectedLesson);
            _lessonPreview.UpdateLessonEntity(selectedLesson);
        }
        
        /// <summary>
        /// The given lesson will be forwarded to the main view to
        /// switch from the landing page to the editor.
        /// </summary>
        public void StartSelectedLesson(LessonEntity selectedLesson)
        {
            _landingPageNode.StartLesson(selectedLesson);
        }
        #endregion
    }
}