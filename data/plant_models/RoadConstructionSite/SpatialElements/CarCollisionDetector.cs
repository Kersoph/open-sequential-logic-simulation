using Godot;


namespace Osls.SfcSimulation.PlantModels.RoadConstructionSite
{
    /// <summary>
    /// First implementation with an area collider.
    /// Unfortunaleley the collsion detection is only updated on certain times and it seems
    /// that we don't have an influence on it. For the fast forwars simulation, we want to
    /// calculate millions of steps in one frame but want the collision detecion aswell.
    /// We focus on a manual approach for now.
    /// </summary>
    public class CarCollisionDetector : Area
    {
        #region ==================== Fields Properties ====================
        public bool WasHit { get; private set; }
        #endregion
        
        
        #region ==================== Public ====================
        public override void _Ready()
        {
            WasHit = false;
            this.Connect("area_entered", this, nameof(OnAreaEntered));
        }
        
        /// <summary>
        /// Starts to look for collisions on layer 2 (bit 0b10)
        /// </summary>
        public void StartCollisionDetetion()
        {
            this.CollisionLayer = 0b10;
            this.CollisionMask = 0b10;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Event raised by the physics "area_entered".
        /// </summary>
        private void OnAreaEntered(Area otherArea)
        {
            OnCollision();
            if (otherArea is CarCollisionDetector otherDetector) otherDetector.OnCollision();
        }
        
        /// <summary>
        /// Called if this detector is colliding
        /// Can be called multiple times
        /// </summary>
        private void OnCollision()
        {
            if (!WasHit)
            {
                GetNode<DynamicCar>("..").OnCollision();
                WasHit = true;
            }
        }
        #endregion
    }
}