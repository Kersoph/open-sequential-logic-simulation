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
        private const float CarSpeed = 0.0001f;
        
        [Export] private NodePath _barrierPath = "../ElectricalBarrierNode";
        
        /// <summary>
        /// True if a car is visibly on the place before the barrier
        /// </summary>
        public bool IsCarAtCheckpoint { get; private set; }
        
        /// <summary>
        /// True if a car is visibly under the barrier
        /// </summary>
        public bool IsCarUnderBarrier { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void Update(SimulationPage master, int deltaTime)
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
                }
            }
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
        #endregion
    }
}
