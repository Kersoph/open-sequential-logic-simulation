using Godot;


namespace Osls.LandingPage
{
    /// <summary>
    /// Topmost node for the lesson landing page.
    /// Notifies the main node which lesson should be started.
    /// </summary>
    public class LandingPageNode : PageModule
    {
        #region ==================== Fields / Properties ====================
        private const string MainViewPath = "..";
        private MainNode _mainNode;
        
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public override PageCategory ScenePage { get { return PageCategory.LandingPage; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            _mainNode = GetNode<MainNode>(MainViewPath);
        }
        
        /// <summary>
        /// The currently selected lesson will be told tho the main view to
        /// switch from the landing page to the SFC editor.
        /// </summary>
        public void StartLesson(LessonEntity lesson)
        {
            _mainNode.OpenNewLesson(lesson);
        }
        #endregion
    }
}