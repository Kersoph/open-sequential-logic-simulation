namespace Osls
{
    /// <summary>
    /// Top node for the model editor view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public class ModelEditor : PageModule
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public override PageCategory ScenePage { get { return PageCategory.LogicEditor; } }
        #endregion
    }
}
