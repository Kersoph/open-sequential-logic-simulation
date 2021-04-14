using Osls.Plants;
using Osls.SfcEditor;
using System.Collections.Generic;


namespace Osls.SfcSimulation.Engine
{
    public class ProgrammableLogicController : IProcessingUnit
    {
        #region ==================== Fields Properties ====================
        private ResettingStateTable _inputRegisters;
        private ResettingStateTable _outputRegisters;
        private SimulationPage _simulationPage;
        
        /// <summary>
        /// Links the input register of the processing unit
        /// </summary>
        public StateTable InputRegisters { get { return _inputRegisters; } }
        
        /// <summary>
        /// Links the output register of the processing unit
        /// </summary>
        public StateTable OutputRegisters { get { return _outputRegisters; } }
        
        /// <summary>
        /// Links the internal integer variables of the processing unit
        /// </summary>
        public IEnumerable<string> IntVariables { get { return SfcProgramData.StepMaster.PatchStepTimeMap.Keys; } }
        
        public SfcProgramData SfcProgramData { get; private set; }
        protected SfcProgram SfcProgram { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public ProgrammableLogicController(SimulationPage simulationPage, SfcEntity sfcEntity)
        {
            _simulationPage = simulationPage;
            _inputRegisters = new ResettingStateTable(_simulationPage.SimulationOutput);
            _outputRegisters = new ResettingStateTable(_simulationPage.SimulationInput);
            SfcProgramData = new SfcProgramData(sfcEntity, this);
            SfcProgram = new SfcProgram(this, SfcProgramData);
        }
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Called to initialize the PLC
        /// </summary>
        public void Startup()
        {
            SfcProgramData.InitializeSfcSteps(this);
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
        
        /// <summary>
        /// Returns true if there is a internal variable with this key
        /// </summary>
        public bool HasIntVariable(string key)
        {
            return SfcProgramData.StepMaster.ContainsInternalNumeric(key);
        }
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        public int LookupIntVariable(string key)
        {
            return SfcProgramData.GetStepFromMapKey(SfcProgramData.StepMaster.GetStepTimeKey(key)).StepCounter;
        }
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        public bool LookupBoolVariable(string key)
        {
            throw new System.NotImplementedException();
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