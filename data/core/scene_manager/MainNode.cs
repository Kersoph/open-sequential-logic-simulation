using Godot;
using Osls.Navigation;


namespace Osls
{
    /// <summary>
    /// Top node connected to the main tree. It will be rendered to the main window.
    /// Handels the visibility and layers of the modules.
    /// </summary>
    public class MainNode : Node
    {
        #region ==================== Fields ====================
        private LessonController _lessonController;
        private NavigationSteps _navigationSteps;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
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
        public void OpenNewLesson(LessonEntity lessonData)
        {
            _lessonController.ApplyNewLesson(lessonData);
            ChangePageTo(PageCategory.LogicEditor);
        }
        
        /// <summary>
        /// Requests a change of the current page to the new page.
        /// Used to privide the possibility for the user to save or cancel the action.
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
        private void QuitSfcSandbox()
        {
            GetTree().Quit();
        }
        #endregion
    }
}