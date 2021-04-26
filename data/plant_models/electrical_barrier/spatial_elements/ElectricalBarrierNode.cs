using Godot;


namespace Osls.Plants.ElectricalBarrier
{
    public class ElectricalBarrierNode : Spatial
    {
        #region ==================== Fields / Properties ====================
        public const string MotorKey = "M_Gate";
        public const string SensorOpenedKey = "B_Opened";
        public const string SensorClosedKey = "B_Closed";
        private bool _isBroken = false;
        
        /// <summary>
        /// True if agents perceive the barrier as opened
        /// </summary>
        public bool IsVisiblyOpen
        {
            get
            {
                float rotation = GetNode<MeshInstance>("BarrierElectricalPole").RotationDegrees.z;
                return rotation > 65;
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void Update(SimulationPage master, int deltaTime)
        {
            if (_isBroken) return;
            int motor = master.SimulationInput.PollInteger(MotorKey);
            GetNode<MeshInstance>("BarrierElectricalPole").RotateZ(motor * deltaTime * 0.0004f);
            float rotation = GetNode<MeshInstance>("BarrierElectricalPole").RotationDegrees.z;
            master.SimulationOutput.SetValue(SensorOpenedKey, rotation > 75);
            master.SimulationOutput.SetValue(SensorClosedKey, rotation < 2);
            if (rotation < -10 || rotation > 133)
            {
                BreakDown();
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        private void BreakDown()
        {
            _isBroken = true;
        }
        #endregion
    }
}