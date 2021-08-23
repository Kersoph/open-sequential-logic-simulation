using Godot;
using Osls.Bubbles;


namespace Osls.Plants.ElectricalBarrier
{
    public class VehicleAgentController : Spatial
    {
        #region ==================== Fields / Properties ====================
        public const float PathCheckpoint = 0.23f;
        public const float PathCheckpointCollisionStart = 0.26f;
        public const float PathCheckpointCollisionEnd = 0.30f;
        public const float PathCheckpointPassed = 0.32f;
        public const float EntersTunnelSoon = 0.34f;
        public const float EntersTunnel = 0.45f;
        public const float ExitsTunnel = 0.9f;
        public const float RegularCarSpeed = 0.0001f;
        
        [Export] private NodePath _barrierPath = "../ElectricalBarrierNode";
        [Export] private NodePath _BubbleSpritePath = "VehiclePath/PathFollow/HighCar_Black/BubbleSprite";
        
        protected BubbleSprite Bubble { get { return GetNode<BubbleSprite>(_BubbleSpritePath); } }
        
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
        /// The cumulated time per car the driver had to wait despite being allowed to access the area
        /// </summary>
        public int WaitingTime { get; private set; }
        
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
                OnDriving(car, master, deltaTime);
            }
            else
            {
                OnWait(master, deltaTime);
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
            WaitingTime = 0;
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
            Bubble.ShowAs(BubbleSprite.Bubble.Shout, BubbleSprite.Expression.Exclamation, 3, true);
        }
        
        private void OnDriving(PathFollow car, ElectricalBarrier master, int deltaTime)
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
            if (car.UnitOffset > EntersTunnel && car.UnitOffset < ExitsTunnel)
            {
                if (!master.TunnelLights.AreLightsOn)
                {
                    Bubble.ShowAs(BubbleSprite.Bubble.Think, BubbleSprite.Expression.Confused, 0.5f);
                }
            }
        }
        
        private void OnWait(ElectricalBarrier master, int deltaTime)
        {
            if (!master.Guard.AllowVehiclePass) return;
            WaitingTime += deltaTime;
            if (WaitingTime > 3000 && WaitingTime < 3500)
            {
                Bubble.ShowAs(BubbleSprite.Bubble.Think, BubbleSprite.Expression.Confused, 1);
            }
            else if (WaitingTime > 6000 && WaitingTime < 6800)
            {
                Bubble.ShowAs(BubbleSprite.Bubble.Think, BubbleSprite.Expression.Annoyed, 0.5f);
            }
            else if (WaitingTime > 6800 && WaitingTime < 7500)
            {
                Bubble.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Confused, 1);
            }
            else if (WaitingTime > 15000 && WaitingTime < 15500)
            {
                Bubble.ShowAs(BubbleSprite.Bubble.Shout, BubbleSprite.Expression.Surprised, 1);
            }
            else if (WaitingTime > 25000 && WaitingTime < 26000)
            {
                Bubble.ShowAs(BubbleSprite.Bubble.Shout, BubbleSprite.Expression.Surprised, 1);
            }
            else if (WaitingTime > 26000 && WaitingTime < 26500)
            {
                Bubble.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Frustrated, 1);
            }
            else if (WaitingTime > 26500 && WaitingTime < 27000)
            {
                Bubble.ShowAs(BubbleSprite.Bubble.Shout, BubbleSprite.Expression.Exclamation, 3);
            }
        }
        #endregion
    }
}
