using Osls.Plants;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.Engine
{
    public class ProgrammableLogicController : IProcessingUnit
    {
        #region ==================== Fields Properties ====================
        private ResettingStateTable _inputRegisters;
        private ResettingStateTable _outputRegisters;
        private SimulationPage _simulationPage;
        
        public StateTable InputRegisters { get { return _inputRegisters; } }
        public StateTable OutputRegisters { get { return _outputRegisters; } }
        
        public SfcProgram SfcProgram { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public ProgrammableLogicController(SimulationPage simulationPage, SfcEntity sfcEntity)
        {
            _simulationPage = simulationPage;
            SfcProgramData data = new SfcProgramData(sfcEntity);
            SfcProgram = new SfcProgram(this, data);
        }
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Called to initialize the PLC
        /// </summary>
        public void Startup()
        {
            _inputRegisters = new ResettingStateTable(_simulationPage.SimulationOutput);
            _outputRegisters = new ResettingStateTable(_simulationPage.SimulationInput);
        }
        
        /// <summary>
        /// Is called every simulation step to update the PLC.
        /// We follow the IPO (EVA) pattern like real PLC.
        /// </summary>
        public void Update(int deltaTimeMs)
        {
            ResetOutputRegisters();
            UpdateInputs();
            UpdateProcess(deltaTimeMs);
            UpdateOutputs();
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsLogicValid()
        {
            return SfcProgram.IsProgramLogicValid();
        }
        #endregion
        
        
        #region ==================== Private ====================
        /// <summary>
        /// Resets the output registers to the standard value
        /// </summary>
        private void ResetOutputRegisters()
        {
            _outputRegisters.ResetRegisters();
        }
        
        /// <summary>
        /// Copies the plant state outputs to our input registers.
        /// </summary>
        private void UpdateInputs()
        {
            _inputRegisters.AssignValuesFrom(_simulationPage.SimulationOutput);
        }
        
        /// <summary>
        /// Updates the sfc model
        /// </summary>
        private void UpdateProcess(int timeMs)
        {
            SfcProgram.UpdateProcess(timeMs);
        }
        
        /// <summary>
        /// Writes our output registers to the plant inputs.
        /// </summary>
        private void UpdateOutputs()
        {
            _simulationPage.SimulationInput.AssignValuesFrom(OutputRegisters);
        }
        #endregion
    }
}