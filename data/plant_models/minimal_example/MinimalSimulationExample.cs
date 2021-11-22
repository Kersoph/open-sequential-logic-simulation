using System.Collections.Generic;


namespace Osls.Plants.MinimalExample
{
    /// <summary>
    /// Minimal example class for a simulation controller
    /// </summary>
    public class MinimalSimulationExample : SimulationPage
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public override void SetupUi()
        {
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
                    { new StateEntry<bool>("Foo", true, "Foo description", "Foo hint") },
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>("Bar", 0, "Bar description", "Bar hint") },
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
                    { new StateEntry<bool>("Zumsl", false, "Zumsl description", "Zumsl hint") },
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>("M15.2", 1, "M15.2 description", "M15.2 hint") }
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
