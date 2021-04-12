using Godot;


namespace Osls.Plants.RoadConstructionSite
{
    public class PathController : Path
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// The minimum distance in m the previous car needs that a new car can be spawned
        /// </summary>
        [Export] public float MinimumSpawnDistance = 5f;
        
        /// <summary>
        /// Distance in m the traffic light is away from the spawn point
        /// </summary>
        [Export] public float TrafficControlSystemDistance = 20f;
        
        private DynamicCar _lastCar;
        private TrafficControlSystem _trafficLight;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the path controller
        /// </summary>
        public void Setup(TrafficControlSystem referenceTrafficLight)
        {
            _trafficLight = referenceTrafficLight;
            _trafficLight.SetDistances(this);
        }
        
        /// <summary>
        /// Returns true if a new car can be spawned. (Enough space for a new car)
        /// </summary>
        public bool SpawnPossible()
        {
            if (_lastCar == null) return true;
            return _lastCar.Offset > MinimumSpawnDistance;
        }
        
        /// <summary>
        /// Adds the car to the path controller and sets references.
        /// </summary>
        public void AddChildAndStart(DynamicCar car)
        {
            AddChild(car);
            car.StartDriving(_lastCar, _trafficLight);
            _lastCar = car;
        }
        
        /// <summary>
        /// Removes the car as a line follower.
        /// </summary>
        public void CheckAndRemoveChild(DynamicCar car)
        {
            RemoveChild(car);
            if (car == _lastCar) _lastCar = null;
        }
        #endregion
    }
}