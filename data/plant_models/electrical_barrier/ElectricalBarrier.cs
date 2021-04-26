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
                    {"B_Opened", false},
                    {"B_Closed", false}
                },
                new Dictionary<string, int>()
                {
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
                },
                new Dictionary<string, int>()
                {
                    {"M_Gate", 0},
                }
            );
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int deltaTime) { }
        #endregion
    }
}