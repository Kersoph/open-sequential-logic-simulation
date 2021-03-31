using System.Collections.Generic;


namespace Osls.SfcSimulation.MinimalExample
{
    /// <summary>
    /// Minimal example calss for a simulation controller
    /// </summary>
    public class MinimalSimulationExample : SimulationControlNode
    {
        #region ==================== Methods ====================
        /// <summary>
        /// Returns the input definition for the simulation
        /// </summary>
        protected override StateTable DefineInputs()
        {
            return new StateTable(
                new Dictionary<string, bool>()
                {
                    {"Foo", true},
                    {"Bar", false}
                },
                new Dictionary<string, int>()
                {
                    {"Teki", 0},
                    {"Tori", 0}
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
                    {"Zumsl", false},
                    {"Surzul", false}
                },
                new Dictionary<string, int>()
                {
                    {"O1.12", 1},
                    {"O1.13", 2}
                }
            );
        }
        
        /// <summary>
        /// Claculate sthe next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep() { }
        #endregion
    }
}