using Godot;


namespace Osls.Plants.ElectricalBarrier
{
    public class GuardAgent : Spatial
    {
        #region ==================== Fields / Properties ====================
        public const string OpenGateSwitchKey = "S_OpenGate";
        private NodePath VehicleAgentPath = "../VehicleAgents";
        
        /// <summary>
        /// Set to false to prevent the gate being opened
        /// </summary>
        public bool AllowVehiclePass { get; set; } = true;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void Update(SimulationPage master)
        {
            VehicleAgentController vehicles = GetNode<VehicleAgentController>(VehicleAgentPath);
            if (AllowVehiclePass && (vehicles.IsCarAtCheckpoint || vehicles.IsCarUnderBarrier))
            {
                master.SimulationOutput.SetValue(OpenGateSwitchKey, true);
            }
            else
            {
                master.SimulationOutput.SetValue(OpenGateSwitchKey, false);
            }
        }
        #endregion
    }
}