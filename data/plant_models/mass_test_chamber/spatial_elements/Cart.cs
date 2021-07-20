using Godot;


namespace Osls.Plants.MassTestChamber
{
    public class Cart : Spatial
    {
        #region ==================== Fields / Properties ====================
        [Export] private float _speed = 0.3f;
        [Export] private Vector3 _backPosition = new Vector3(23f, 3f, 0f);
        [Export] private Vector3 _frontPosition = new Vector3(13f, 3f, 0f);
        private int _endStopCollisionTime = 0;
        
        /// <summary>
        /// The position of the cart with 0 back and 1 front
        /// </summary>
        public float RailPosition { get; private set; } = 0f;
        
        /// <summary>
        /// True if the cart is at the front position
        /// </summary>
        public bool FrontPositionReached { get; private set; }
        
        /// <summary>
        /// True if the cart is at the back position
        /// </summary>
        public bool BackPositionReached { get; private set; }
        
        /// <summary>
        /// True if the cart is broken
        /// </summary>
        public bool IsBroken { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called before the simulation is added to the main tree
        /// </summary>
        public void Setup()
        {
            CheckPosition(0);
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void Drive(bool forward, int deltaTime)
        {
            if (IsBroken) return;
            float direction = forward ? 1f : -1f;
            float movement = direction * _speed * deltaTime * 0.001f;
            RailPosition += movement;
            CheckPosition(deltaTime);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void CheckPosition(int deltaTime)
        {
            FrontPositionReached = RailPosition > 0.95f;
            BackPositionReached = RailPosition < 0.05;
            if (RailPosition > 1f)
            {
                RailPosition = 1f;
                _endStopCollisionTime += deltaTime;
                CheckBreakdown();
            }
            else if (RailPosition < 0f)
            {
                RailPosition = 0f;
                _endStopCollisionTime += deltaTime;
                CheckBreakdown();
            }
            Vector3 position = (1f - RailPosition) * _backPosition + RailPosition * _frontPosition;
            Translation = position;
        }
        
        private void CheckBreakdown()
        {
            if (_endStopCollisionTime > 500)
            {
                IsBroken = true;
            }
        }
        #endregion
    }
}
