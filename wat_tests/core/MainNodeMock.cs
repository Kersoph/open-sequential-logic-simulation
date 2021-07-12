using Osls;


namespace Tests.Core
{
    public class MainNodeMock : IMainNode
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Opens up a new lesson and changes the view to the editor step.
        /// </summary>
        public void OpenNewLesson(ILessonEntity lessonData)
        {
        }
        
        /// <summary>
        /// Requests a change of the current page to the new page.
        /// Used to provide the possibility for the user to save or cancel the action.
        /// </summary>
        public void UserRequestsChangeTo(PageCategory page)
        {
        }
        
        /// <summary>
        /// Changes the main page to the given view. Forced.
        /// </summary>
        public void ChangePageTo(PageCategory page)
        {
        }
        #endregion
    }
}