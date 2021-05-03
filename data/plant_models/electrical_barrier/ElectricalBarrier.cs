using System.Collections.Generic;


namespace Osls.Plants.ElectricalBarrier
{
    /// <summary>
    /// Minimal example class for a simulation controller
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
                    { new StateEntry<int>(ElectricalBarrierNode.MotorKey, 0, "Barrier Motor", "A signal of 1 will lift the barrier and a signal of -1 wil lower it") },
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
                    { new StateEntry<bool>(ElectricalBarrierNode.SensorOpenedKey, false, "Opened position limit switch", "True if the upper position is reached") },
                    { new StateEntry<bool>(ElectricalBarrierNode.SensorClosedKey, false, "Closed position limit switch", "True if the lower position is reached") },
                    { new StateEntry<bool>(GuardAgent.OpenGateSwitchKey, false, "Guards open signal", "True if the guard wants to open the barrier") },
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
            Guard.Update(this);
        }
        #endregion
    }
}