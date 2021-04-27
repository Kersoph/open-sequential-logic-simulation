using Godot;


namespace Osls.Plants.ElectricalBarrier
{
    public class Flash : OmniLight
    {
        #region ==================== Fields / Properties ====================
        private bool _flashNextFrame;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Process(float delta)
        {
            if (_flashNextFrame)
            {
                Visible = true;
                _flashNextFrame = false;
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
            _flashNextFrame = true;
        }
        #endregion
    }
}