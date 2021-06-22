using Godot;
using Osls.Core;


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
        [Export] private NodePath _lessonViewPath = "LessonView";
        
        private LandingPageNode _landingPageNode;
        private LessonView _lessonView;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            _landingPageNode = GetNode<LandingPageNode>(_landingPagePath);
            _lessonView = GetNode<LessonView>(_lessonViewPath);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Informs the grid, that the selection has changed to this button and the UI needs to be updated.
        /// </summary>
        public void SelectionChangedTo(ILessonEntity selectedLesson)
        {
            _lessonView.LoadAndShowInfo(selectedLesson);
        }
        
        /// <summary>
        /// The given lesson will be forwarded to the main view to
        /// switch from the landing page to the editor.
        /// </summary>
        public void StartSelectedLesson(ILessonEntity selectedLesson)
        {
            if (_lessonView.PlantView.LoadedSimulationNode != null)
            {
                _landingPageNode.StartLesson(selectedLesson);
            }
            else
            {
                GD.PushWarning("could not load " + selectedLesson.Title);
            }
        }
        #endregion
    }
}