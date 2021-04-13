using System.Collections.Generic;


namespace Osls.Plants.MinimalExample
{
    /// <summary>
    /// Minimal example class for a simulation controller
    /// </summary>
    public class MinimalSimulationExample : SimulationPage
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
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int deltaTime) { }
        #endregion
    }
}