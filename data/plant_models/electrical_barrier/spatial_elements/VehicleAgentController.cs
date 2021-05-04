using Godot;


namespace Osls.Plants.ElectricalBarrier
{
    public class VehicleAgentController : Spatial
    {
        #region ==================== Fields / Properties ====================
        public const float PathCheckpoint = 0.23f;
        public const float PathCheckpointCollisionStart = 0.26f;
        public const float PathCheckpointCollisionEnd = 0.30f;
        public const float PathCheckpointPassed = 0.32f;
        public const float RegularCarSpeed = 0.0001f;
        
        [Export] private NodePath _barrierPath = "../ElectricalBarrierNode";
        
        /// <summary>
        /// How fast the car can drive in
        /// UnitOffset [0..1] per ms
        /// </summary>
        public float CarSpeed { get; set; } = RegularCarSpeed;
        
        /// <summary>
        /// True if a car is visibly on the place before the barrier
        /// </summary>
        public bool IsCarAtCheckpoint { get; private set; }
        
        /// <summary>
        /// True if a car is visibly under the barrier
        /// </summary>
        public bool IsCarUnderBarrier { get; private set; }
        
        /// <summary>
        /// The number of times the car could pass the whole track
        /// </summary>
        public int TimesPassedTrack { get; private set; }
        
        /// <summary>
        /// True if the car was damaged by the barrier
        /// </summary>
        public bool Damaged { get; private set; }
        
        /// <summary>
        /// Range from the path at the starting position (0) to the end position (1)
        /// </summary>
        public float CarUnitOffset { get { return GetNode<PathFollow>("VehiclePath/PathFollow").UnitOffset; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void Update(ElectricalBarrier master, int deltaTime)
        {
            PathFollow car = GetNode<PathFollow>("VehiclePath/PathFollow");
            if (IsCarDriving(car))
            {
                float newOffset = car.UnitOffset + (CarSpeed * deltaTime);
                if (newOffset < 1)
                {
                    car.UnitOffset = newOffset;
                }
                else
                {
                    ResetAgent(car);
                    TimesPassedTrack++;
                }
            }
            CheckCollision(master);
        }
        
        /// <summary>
        /// Places the car at the position from the given path unit offset [0..1]
        /// </summary>
        public void PlaceCarAt(float unitOffset)
        {
            PathFollow car = GetNode<PathFollow>("VehiclePath/PathFollow");
            car.UnitOffset = unitOffset;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Returns true if tha agent is driving
        /// </summary>
        private bool IsCarDriving(PathFollow car)
        {
            float carOffset = car.UnitOffset;
            if (carOffset < PathCheckpoint)
            {
                IsCarAtCheckpoint = false;
                IsCarUnderBarrier = false;
                return true;
            }
            if (carOffset < PathCheckpointCollisionStart)
            {
                IsCarAtCheckpoint = true;
                IsCarUnderBarrier = false;
                return GetNode<ElectricalBarrierNode>(_barrierPath).IsVisiblyOpen;
            }
            if (carOffset < PathCheckpointPassed)
            {
                IsCarAtCheckpoint = false;
                IsCarUnderBarrier = true;
                return GetNode<ElectricalBarrierNode>(_barrierPath).IsVisiblyOpen;
            }
            IsCarAtCheckpoint = false;
            IsCarUnderBarrier = false;
            return true;
        }
        
        private void ResetAgent(PathFollow car)
        {
            car.UnitOffset = 0;
            car.Rotation = new Vector3();
        }
        
        private void CheckCollision(ElectricalBarrier master)
        {
            PathFollow car = GetNode<PathFollow>("VehiclePath/PathFollow");
            float unitOffset = car.UnitOffset;
            if (unitOffset > PathCheckpointCollisionStart && unitOffset < PathCheckpointCollisionEnd)
            {
                if (master.Barrier.BarrierRotation < 10)
                {
                    BreakDown();
                }
            }
        }
        
        private void BreakDown()
        {
            Damaged = true;
            CarSpeed = 0;
        }
        #endregion
    }
}
