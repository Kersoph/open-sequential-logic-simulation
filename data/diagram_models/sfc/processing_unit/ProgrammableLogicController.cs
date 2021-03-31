namespace Osls.SfcSimulation.Engine
{
    public class ProgrammableLogicController
    {
        #region ==================== Fields Properties ====================
        public InputTable InputRegisters { get; private set; }
        public OutputTable OutputRegisters { get; private set; }
        public Master Master { get; private set; }
        
        private readonly SfcProgramm _sfcProgramm;
        #endregion
        
        
        #region ==================== Constructor ====================
        public ProgrammableLogicController(Master master)
        {
            Master = master;
            _sfcProgramm = new SfcProgramm(this);
        }
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Called to initialize the PLC
        /// </summary>
        public void Startup()
        {
            InputRegisters = Master.SimulationControlNode.SimulationOutput.CloneToInputTable();
            OutputRegisters = Master.SimulationControlNode.SimulationInput.CloneToOutputTable();
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
        
        public void VisualiseStatus()
        {
            _sfcProgramm.VisualiseStatus();
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsPlcLogicValid()
        {
            return _sfcProgramm.IsProgrammLogicValid();
        }
        #endregion
        
        
        #region ==================== Private ====================
        /// <summary>
        /// Resets the output registers to the standard value
        /// </summary>
        private void ResetOutputRegisters()
        {
            OutputRegisters.ResetRegisters();
        }
        
        /// <summary>
        /// Copies the plant state outputs to our input registers.
        /// </summary>
        private void UpdateInputs()
        {
            InputRegisters.AssignValuesFrom(Master.SimulationControlNode.SimulationOutput);
        }
        
        /// <summary>
        /// Updates the sfc model
        /// </summary>
        private void UpdateProcess()
        {
            _sfcProgramm.UpdateProcess();
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