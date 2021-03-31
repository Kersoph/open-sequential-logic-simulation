using Godot;
using SfcSandbox.Data.Model;
using SfcSandbox.Data.Main;

namespace SfcSandbox.Data.Model.LandingPage
{
    /// <summary>
    /// Topmost node for the lesson langing page.
    /// Notifies the main node which lesson should be started.
    /// </summary>
    public class LandingPageNode : Node
    {
        #region ==================== Fields ====================
        private const string MainViewPath = "..";
        private MainNode _mainNode;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            _mainNode = GetNode<MainNode>(MainViewPath);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
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