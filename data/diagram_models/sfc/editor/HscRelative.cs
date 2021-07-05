using Godot;


namespace Osls
{
    /// <summary>
    /// Horizontal split container with automatic relative adjustment to the rect size
    /// </summary>
    public class HscRelative : HSplitContainer
    {
        #region ==================== Fields / Properties ====================
        [Export] public float RelativeOffst = 1320f/1840f;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            SplitOffset = Mathf.RoundToInt(RectSize.x * RelativeOffst);
        }
        #endregion
    }
}
