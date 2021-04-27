using System.Collections.Generic;


namespace Osls.Plants.ElectricalBarrier
{
    /// <summary>
    /// Minimal example class for a simulation controller
    /// </summary>
    public class ElectricalBarrier : SimulationPage
    {
        #region ==================== Helpers ====================
        /// <summary>
        /// Returns the input definition for the simulation
        /// </summary>
        protected override StateTable DefineInputs()
        {
            return new StateTable(
                new Dictionary<string, bool>()
                {
                },
                new Dictionary<string, int>()
                {
                    { ElectricalBarrierNode.MotorKey, 0 },
                }
            );
        }
        
        /// <summary>
        /// Returns the output definition for the simulation
        /// </summary>
        /// <returns></returns>
        protected override StateTable DefineOutput()
        {
            return new StateTable(
                new Dictionary<string, bool>()
                {
                    { ElectricalBarrierNode.SensorOpenedKey, false },
                    { ElectricalBarrierNode.SensorClosedKey, false },
                    { GuardAgent.OpenGateSwitchKey, false },
                },
                new Dictionary<string, int>()
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
            GetNode<ElectricalBarrierNode>("ElectricalBarrierNode").Update(this, deltaTime);
            GetNode<VehicleAgentController>("VehicleAgents").Update(this, deltaTime);
            GetNode<GuardAgent>("GuardAgent").Update(this, deltaTime);
        }
        #endregion
    }
}