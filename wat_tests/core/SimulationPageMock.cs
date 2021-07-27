using Osls;
using Osls.Plants;
using System.Collections.Generic;


namespace Tests.Core
{
    /// <summary>
    /// Top node for the whole simulation view.
    /// In this mock all IO tables will be empty.
    /// </summary>
    public class SimulationPageMock : SimulationPage
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the user can have options to influence the simulation
        /// Normally called by the by the simulation UI
        /// </summary>
        public override void SetupUi() { }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Returns the input definition for the simulation
        /// </summary>
        protected override StateTable DefineInputs() { return new StateTable(new List<StateEntry<bool>>(), new List<StateEntry<int>>()); }
        
        /// <summary>
        /// Returns the output definition for the simulation
        /// </summary>
        protected override StateTable DefineOutput() { return new StateTable(new List<StateEntry<bool>>(), new List<StateEntry<int>>()); }
        
        /// <summary>
        /// Calculates the next simulation step
        /// </summary>
        protected override void CalculateNextStep(int timeMs) { }
        #endregion
    }
}