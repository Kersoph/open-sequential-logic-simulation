namespace Osls.SfcSimulation.Engine
{
    public class ProgrammableLogicController
    {
        #region ==================== Fields Properties ====================
        public ResettingStateTable InputRegisters { get; private set; }
        public ResettingStateTable OutputRegisters { get; private set; }
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
            InputRegisters = new ResettingStateTable(Master.SimulationControlNode.SimulationOutput);
            OutputRegisters = new ResettingStateTable(Master.SimulationControlNode.SimulationInput);
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
        public bool IsPlcLogicValid()
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