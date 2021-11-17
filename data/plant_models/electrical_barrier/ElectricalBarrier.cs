using System.Collections.Generic;


namespace Osls.Plants.ElectricalBarrier
{
    /// <summary>
    /// Boundary class between the simulation and the SFC Controllers
    /// </summary>
    public class ElectricalBarrier : SimulationPage
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// Links the simulated barrier node
        /// </summary>
        public ElectricalBarrierNode Barrier { get { return GetNode<ElectricalBarrierNode>("ElectricalBarrierNode"); } }
        
        /// <summary>
        /// Links the simulated vehicle controller
        /// </summary>
        public VehicleAgentController Vehicle { get { return GetNode<VehicleAgentController>("VehicleAgents"); } }
        
        /// <summary>
        /// links the simulated guard controller
        /// </summary>
        public GuardAgent Guard { get { return GetNode<GuardAgent>("GuardAgent"); } }
        
        /// <summary>
        /// links the tunnel light controller
        /// </summary>
        public TunnelLights TunnelLights { get { return GetNode<TunnelLights>("Map/TunnelLights"); } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public override void SetupUi()
        {
            GetNode<ElectricalBarrierUi>("ElectricalBarrierUi").SetupUi(this);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Returns the input definition for the simulation
        /// </summary>
        protected override StateTable DefineInputs()
        {
            return new StateTable(
                new List<StateEntry<bool>>()
                {
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>(ElectricalBarrierNode.MotorKey, 0, "Barrier motor", "A signal of 1 will lift the barrier,\na signal of -1 wil lower it and\na signal of 0 will keep it in position.\nKeep in mind that numbers retain their value and are not automatically reset!") },
                }
            );
        }
        
        /// <summary>
        /// Returns the output definition for the simulation
        /// </summary>
        protected override StateTable DefineOutput()
        {
            return new StateTable(
                new List<StateEntry<bool>>()
                {
                    { new StateEntry<bool>(ElectricalBarrierNode.SensorOpenedKey, false, "Opened position limit switch", "True if the upper position is reached.\nFalse otherwise.") },
                    { new StateEntry<bool>(ElectricalBarrierNode.SensorClosedKey, false, "Closed position limit switch", "True if the lower position is reached.\nFalse otherwise.") },
                    { new StateEntry<bool>(GuardAgent.OpenGateSwitchKey, false, "Open barrier signal", "True if the barrier should be open to let a vehicle pass.\nIt always remains true until it is save to lower the barrier.\nSo the guard keeps the switch on if something is blocking the barrier.") },
                },
                new List<StateEntry<int>>()
                {
                }
            );
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int deltaTime)
        {
            Barrier.Update(this, deltaTime);
            Vehicle.Update(this, deltaTime);
            Guard.Update(this, deltaTime);
        }
        #endregion
    }
}
