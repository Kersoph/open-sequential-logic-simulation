namespace Osls
{
    /// <summary>
    /// Top node connected to the main tree. It will be rendered to the main window.
    /// Handles the visibility and layers of the modules.
    /// </summary>
    public interface IMainNode
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Opens up a new lesson and changes the view to the editor step.
        /// </summary>
        void OpenNewLesson(ILessonEntity lessonData);
        
        /// <summary>
        /// Requests a change of the current page to the new page.
        /// Used to provide the possibility for the user to save or cancel the action.
        /// </summary>
        void UserRequestsChangeTo(PageCategory page);
        
        /// <summary>
        /// Changes the main page to the given view. Forced.
        /// </summary>
        void ChangePageTo(PageCategory page);
        #endregion
    }
}
