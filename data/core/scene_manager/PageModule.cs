using Godot;


namespace Osls
{
    public abstract class PageModule : Node
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public abstract PageCategory ScenePage { get; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole page. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public virtual void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
        }
        
        /// <summary>
        /// Requests a change of the current page to the new page.
        /// Used to provide the possibility for the user to save or cancel the action.
        /// </summary>
        public virtual void OnUserRequestsChange(IMainNode mainNode, PageCategory nextPage)
        {
            mainNode.ChangePageTo(nextPage);
        }
        #endregion
    }
}