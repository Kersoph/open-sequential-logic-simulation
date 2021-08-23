using Godot;


namespace Osls
{
    /// <summary>
    /// Horizontal split container with automatic relative adjustment to the rect size
    /// </summary>
    public class HscRelative : HSplitContainer
    {
        #region ==================== Fields / Properties ====================
        [Export] public float RelativeOffset = 1320f/1840f;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            float minSizeOffset = 0;
            Control control = GetChildOrNull<Control>(0);
            if (control != null)
            {
                minSizeOffset = control.RectMinSize.x;
            }
            SplitOffset = Mathf.RoundToInt(RectSize.x * RelativeOffset - minSizeOffset);
        }
        #endregion
    }
}
