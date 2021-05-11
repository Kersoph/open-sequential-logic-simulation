using Godot;


namespace Osls.Bubbles
{
    public class BubbleSprite : Sprite3D
    {
        #region ==================== Fields / Properties ====================
        public enum Bubble { Say, Think, Shout}
        public enum Expression { Attention, Exclamation, Confused, Surprised, Ok, Nok, Sleeping, Annoyed, Frustrated, Angry, Loving, Happy }
        
        private const int LineOffset = 2;
        private float _remainingTime;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Shows the 3dSprite with the bubble type and expression for the given time [ms] and then hides again.
        /// </summary>
        public void Show(Bubble bubble, Expression expression, float time)
        {
            int y = (int)bubble * LineOffset + (int)expression / Hframes;
            int x = (int)expression % Hframes;
            FrameCoords = new Vector2(x, y);
            _remainingTime = time;
            Visible = true;
        }
        
        public override void _Process(float delta)
        {
            if (Visible)
            {
                _remainingTime -= delta;
                if (_remainingTime < 0)
                {
                    Visible = false;
                }
            }
        }
        #endregion
    }
}