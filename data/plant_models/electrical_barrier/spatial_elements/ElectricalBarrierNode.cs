using Godot;


namespace Osls.Plants.ElectricalBarrier
{
    public class ElectricalBarrierNode : Spatial
    {
        #region ==================== Fields / Properties ====================
        public const string MotorKey = "M_Gate";
        public const string SensorOpenedKey = "B_Opened";
        public const string SensorClosedKey = "B_Closed";
        
        /// <summary>
        /// True if agents perceive the barrier as opened
        /// </summary>
        public bool IsVisiblyOpen
        {
            get
            {
                float rotation = GetNode<MeshInstance>("BarrierElectricalPole").RotationDegrees.z;
                return rotation > 50;
            }
        }
        
        /// <summary>
        /// Barrier rotation in degrees where 0° = horizontal, 90° = Open. 40° will still collide
        /// </summary>
        public float BarrierRotation { get { return GetNode<MeshInstance>("BarrierElectricalPole").RotationDegrees.z; } }
        
        /// <summary>
        /// True if the barrier crashed into something and broke down
        /// </summary>
        public bool IsBroken { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void Update(SimulationPage master, int deltaTime)
        {
            if (IsBroken) return;
            int motor = master.SimulationInput.PollInteger(MotorKey);
            GetNode<MeshInstance>("BarrierElectricalPole").RotateZ(motor * deltaTime * 0.0004f);
            float rotation = GetNode<MeshInstance>("BarrierElectricalPole").RotationDegrees.z;
            master.SimulationOutput.SetValue(SensorOpenedKey, rotation > 60);
            master.SimulationOutput.SetValue(SensorClosedKey, rotation < 2);
            if (rotation < -10 || rotation > 133)
            {
                BreakDown();
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Breaks down with a flash and smoke
        /// </summary>
        private void BreakDown()
        {
            IsBroken = true;
            GetNode<Particles>("BreakdownSmoke").Visible = true;
            GetNode<Flash>("Flash").FlashNextFrame();
        }
        #endregion
    }
}