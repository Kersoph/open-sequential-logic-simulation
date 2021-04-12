namespace Osls.Plants
{
    /// <summary>
    /// Top node for the whole simulation view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public abstract class SimulationPage : PageModule
    {
        #region ==================== Fields ====================
        private StateTable _simulationInput;
        private StateTable _simulationOutput;
        #endregion
        
        
        #region ==================== Properties ====================
        /// <summary>
        /// Gets the input table from the simulation.
        /// Used to set values from the processing unit to the simulation module.
        /// </summary>
        public StateTable SimulationInput {
            get
            {
                if (_simulationInput == null)
                {
                    _simulationInput = DefineInputs();
                }
                return _simulationInput;
            }
        }
        
        /// <summary>
        /// Gets the output table of the simulation.
        /// Used to read the outputs from the simulation to set the inputs to the processing unit.
        /// </summary>
        public StateTable SimulationOutput {
            get
            {
                if (_simulationOutput == null)
                {
                    _simulationOutput = DefineOutput();
                }
                return _simulationOutput;
            }
        }
        
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public override PageCategory ScenePage { get { return PageCategory.Simulation; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void UpdateModel(int timeMs)
        {
            CalculateNextStep();
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Returns the input definition for the simulation
        /// </summary>
        protected abstract StateTable DefineInputs();
        
        /// <summary>
        /// Returns the output definition for the simulation
        /// </summary>
        protected abstract StateTable DefineOutput();
        
        /// <summary>
        /// Calculates the next simulation step
        /// </summary>
        protected abstract void CalculateNextStep();
        #endregion
    }
}