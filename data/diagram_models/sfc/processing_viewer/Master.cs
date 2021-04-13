using Osls.SfcSimulation.Engine;
using Osls.Plants;


namespace Osls.SfcEditor
{
    /// <summary>
    /// The simulation engine master which controls and updates the plant and the PLC.
    /// </summary>
    public class Master
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// The loaded simulation scene with the controller
        /// </summary>
        private readonly SimulationPage _simulationPage;
        
        private readonly ProgrammableLogicController _programmableLogicController;
        #endregion
        
        
        #region ==================== Constructor ====================
        public Master(SfcEntity sfcEntity, SimulationPage simulationPage)
        {
            _simulationPage = simulationPage;
            _programmableLogicController = new ProgrammableLogicController(simulationPage, sfcEntity);
            _programmableLogicController.Startup();
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Is called to calculate the next steps of the simulation with the given time in ms.
        /// We update first the simulation and then the controller.
        /// </summary>
        public void UpdateSimulation(int deltaTimeMs)
        {
            _simulationPage.UpdateModel(deltaTimeMs);
            _programmableLogicController.Update(deltaTimeMs);
        }
        
        /// <summary>
        /// Visualises the active or inactive status of the steps.
        /// </summary>
        public void VisualiseStatus(Sfc2dEditorControl sfc2dEditorControl)
        {
            foreach (var SfcStep in _programmableLogicController.SfcProgram.Data.ActiveSteps)
            {
                sfc2dEditorControl.MarkStep(SfcStep.Id, true);
            }
            foreach (var SfcStep in _programmableLogicController.SfcProgram.Data.InactiveSteps)
            {
                sfc2dEditorControl.MarkStep(SfcStep.Id, false);
            }
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsProgramSimulationValid()
        {
            return _programmableLogicController.IsLogicValid();
        }
        #endregion
    }
}