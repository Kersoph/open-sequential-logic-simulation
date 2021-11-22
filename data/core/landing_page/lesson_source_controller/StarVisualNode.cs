using Godot;


namespace Osls.LandingPage
{
    /// <summary>
    /// The visual stars of this node.
    /// </summary>
    public class StarVisualNode : TextureRect
    {
        #region ==================== Fields ====================
        [Export] public Texture ActiveTexture;
        [Export] public Texture InactiveTexture;
        #endregion
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Sets the texture of this star to active or inactive
        /// </summary>
        public void SetToActiveTexture(bool active)
        {
            Texture = active ? ActiveTexture : InactiveTexture;
        }
        #endregion
    }
}