using Godot;
using Osls.Navigation;


namespace Osls
{
    /// <summary>
    /// Top node connected to the main tree. It will be rendered to the main window.
    /// Handles the visibility and layers of the modules.
    /// </summary>
    public class MainNode : Node, IMainNode
    {
        #region ==================== Fields ====================
        private LessonController _lessonController;
        private NavigationSteps _navigationSteps;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            HandleWindow();
            _lessonController = new LessonController(this);
            _navigationSteps = GetNode<NavigationSteps>("NavigationSteps");
            ChangePageTo(PageCategory.LandingPage);
        }
        
        public override void _Notification(int id)
        {
            if (id == MainLoop.NotificationWmQuitRequest)
            {
                UserRequestsChangeTo(PageCategory.Exit);
            }
        }
        
        /// <summary>
        /// Opens up a new lesson and changes the view to the editor step.
        /// </summary>
        public void OpenNewLesson(ILessonEntity lessonData)
        {
            _lessonController.ApplyNewLesson(lessonData);
            _navigationSteps.VisibleViewIs(PageCategory.LogicEditor);
        }
        
        /// <summary>
        /// Requests a change of the current page to the new page.
        /// Used to provide the possibility for the user to save or cancel the action.
        /// </summary>
        public void UserRequestsChangeTo(PageCategory page)
        {
            _lessonController.UserRequestsChangeTo(page);
        }
        
        /// <summary>
        /// Changes the main page to the given view. Forced.
        /// </summary>
        public void ChangePageTo(PageCategory page)
        {
            _lessonController.ApplyPage(page);
            _navigationSteps.VisibleViewIs(page);
            if (page == PageCategory.Exit) QuitSfcSandbox();
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void HandleWindow()
        {
            OS.WindowMaximized = true;
        }
        
        private void QuitSfcSandbox()
        {
            GetTree().Quit();
        }
        #endregion
    }
}