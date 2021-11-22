using Godot;


namespace Osls.Plants.ElectricalBarrier
{
    public class Flash : OmniLight
    {
        #region ==================== Fields / Properties ====================
        private int _flashFrames;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Process(float delta)
        {
            if (_flashFrames > 0)
            {
                Visible = true;
                _flashFrames--;
            }
            else if (Visible)
            {
                Visible = false;
            }
        }
        
        /// <summary>
        /// Emits a bright light for one frame in the next update.
        /// </summary>
        public void FlashNextFrame()
        {
            _flashFrames = 3;
        }
        #endregion
    }
}
