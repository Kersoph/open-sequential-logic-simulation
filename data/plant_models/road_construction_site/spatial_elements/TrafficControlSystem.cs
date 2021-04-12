using Godot;

namespace Osls.Plants.RoadConstructionSite
{
    /// <summary>
    /// Topmost node for the TrafficControlSystem.tscn
    /// </summary>
    public class TrafficControlSystem : Spatial
    {
        #region ==================== Fields Properties ====================
        private static Color Red = Color.Color8(255, 0, 0, 255);
        private static Color Green = Color.Color8(0, 255, 0, 255);
        
        public PathController Controller { get; private set; }
        
        private bool _isGreen;
        private float _lightDistance;
        private bool _carInRange;
        #endregion
        
        
        #region ==================== Public ====================
        public void SetDistances(PathController controller)
        {
            Controller = controller;
            _lightDistance = Controller.TrafficControlSystemDistance;
        }
        
        /// <summary>
        /// Sets the signalisation to green if the argument is true. Otherwise to red.
        /// </summary>
        public void SetToGreen(bool green)
        {
            _isGreen = green;
            GetNode<Light>("SignalLight").LightColor = green ? Green : Red;
        }
        
        /// <summary>
        /// Returns true if the car needs to stop to be in the stop distance.
        /// </summary>
        public bool CheckStopSignal(float carOffsetDistance, float stopDistance)
        {
            if(carOffsetDistance < _lightDistance) _carInRange = true;
            return (carOffsetDistance > _lightDistance - stopDistance && carOffsetDistance < _lightDistance) && !_isGreen;
        }
        
        /// <summary>
        /// Returns true if a car was in sensor range since the last poll
        /// </summary>
        public bool IsCarInRange()
        {
            bool value = _carInRange;
            _carInRange = false;
            return value;
        }
        #endregion
    }
}