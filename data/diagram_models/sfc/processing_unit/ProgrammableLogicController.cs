namespace Osls.SfcSimulation.Engine
{
    public class ProgrammableLogicController : IProcessingUnit
    {
        #region ==================== Fields Properties ====================
        private ResettingStateTable _inputRegisters;
        private ResettingStateTable _outputRegisters;
        
        public StateTable InputRegisters { get { return _inputRegisters; } }
        public StateTable OutputRegisters { get { return _outputRegisters; } }
        public Master Master { get; private set; }
        public SfcProgramm SfcProgramm { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public ProgrammableLogicController(Master master)
        {
            Master = master;
            SfcProgrammData data = new SfcProgrammData(master.Sfc2dEditorControl.LinkSfcData());
            SfcProgramm = new SfcProgramm(this, data);
        }
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Called to initialize the PLC
        /// </summary>
        public void Startup()
        {
            _inputRegisters = new ResettingStateTable(Master.SimulationControlNode.SimulationOutput);
            _outputRegisters = new ResettingStateTable(Master.SimulationControlNode.SimulationInput);
        }
        
        /// <summary>
        /// Is called every simulation step to update the PLC.
        /// We follow the IPO (EVA) pattern like real PLC.
        /// </summary>
        public void Update()
        {
            ResetOutputRegisters();
            UpdateInputs();
            UpdateProcess();
            UpdateOutputs();
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsLogicValid()
        {
            return SfcProgramm.IsProgrammLogicValid();
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
            _inputRegisters.AssignValuesFrom(Master.SimulationControlNode.SimulationOutput);
        }
        
        /// <summary>
        /// Updates the sfc model
        /// </summary>
        private void UpdateProcess()
        {
            SfcProgramm.UpdateProcess();
        }
        
        /// <summary>
        /// Writes our output registers to the plant inputs.
        /// </summary>
        private void UpdateOutputs()
        {
            Master.SimulationControlNode.SimulationInput.AssignValuesFrom(OutputRegisters);
        }
        #endregion
    }
}