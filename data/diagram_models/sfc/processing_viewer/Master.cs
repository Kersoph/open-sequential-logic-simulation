using Osls.SfcSimulation.Engine;
using Osls.Plants;


namespace Osls.SfcEditor
{
    /// <summary>
    /// The simulation engine master which controls and updates the plant and the PLC.
    /// </summary>
    public class Master
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// The loaded simulation scene with the controller
        /// </summary>
        public SimulationPage SimulationPage { get; }
        
        /// <summary>
        /// The loaded Programmable Logic Controller
        /// </summary>
        public ProgrammableLogicController Plc { get; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public Master(SfcEntity sfcEntity, SimulationPage simulationPage)
        {
            SimulationPage = simulationPage;
            Plc = new ProgrammableLogicController(simulationPage, sfcEntity);
            Plc.Startup();
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Is called to calculate the next steps of the simulation with the given time in ms.
        /// We update first the simulation and then the controller.
        /// </summary>
        public void UpdateSimulation(int deltaTimeMs)
        {
            SimulationPage.UpdateModel(deltaTimeMs);
            Plc.Update(deltaTimeMs);
        }
        
        /// <summary>
        /// Visualises the active or inactive status of the steps.
        /// </summary>
        public void VisualiseStatus(Sfc2dEditorControl sfc2dEditorControl)
        {
            foreach (var SfcStep in Plc.SfcProgramData.ActiveSteps)
            {
                sfc2dEditorControl.MarkStep(SfcStep.Id, true);
            }
            foreach (var SfcStep in Plc.SfcProgramData.InactiveSteps)
            {
                sfc2dEditorControl.MarkStep(SfcStep.Id, false);
            }
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsProgramSimulationValid()
        {
            return Plc.IsLogicValid();
        }
        
        /// <summary>
        /// Resets the controller like there was a blackout
        /// </summary>
        public void Reset()
        {
            Plc.Startup();
        }
        #endregion
    }
}