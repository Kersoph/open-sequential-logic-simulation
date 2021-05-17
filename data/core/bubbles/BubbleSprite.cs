using Godot;


namespace Osls.Bubbles
{
    public class BubbleSprite : Sprite3D
    {
        #region ==================== Fields / Properties ====================
        public enum Bubble { Say, Think, Shout, None }
        public enum Expression { Attention, Exclamation, Confused, Surprised, Ok, Nok, Sleeping, Annoyed, Angry, Frustrated, Loving, Happy, Sad, Laughing, Busy, Depressed, Dizzy, Home, Idea, Money, Music, Waiting }
        
        private const int LineOffset = 4;
        private float _remainingTime;
        private bool _importantDisplay;
        
        /// <summary>
        /// Cumulated time in s from the last activation
        /// </summary>
        public float TimeSinceLastActivation { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Shows the 3dSprite with the bubble type and expression for the given time [s] and then hides again.
        /// </summary>
        public void ShowAs(Bubble bubble, Expression expression, float time, bool important = false)
        {
            if (_importantDisplay && ! important) return;
            _importantDisplay = important;
            int y = (int)bubble * LineOffset + (int)expression / Hframes;
            int x = (int)expression % Hframes;
            FrameCoords = new Vector2(x, y);
            _remainingTime = time;
            TimeSinceLastActivation = 0;
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
                    _importantDisplay = false;
                }
            }
            TimeSinceLastActivation += delta;
        }
        #endregion
    }
}