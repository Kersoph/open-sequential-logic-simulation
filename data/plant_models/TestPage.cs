namespace Osls.Plants
{
    /// <summary>
    /// Top node for the whole test view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public abstract class TestPage : PageModule
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public override PageCategory ScenePage { get { return PageCategory.Examination; } }
        #endregion
    }
}