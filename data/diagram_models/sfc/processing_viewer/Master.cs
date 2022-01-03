using Osls.SfcSimulation.Engine;
using Osls.Plants;
using Osls.SfcEditor;
using System.Collections.Generic;


namespace Osls.SfcSimulation.Viewer
{
    /// <summary>
    /// The simulation engine master which controls and updates the plant and the PLC.
    /// </summary>
    public class Master : IDiagramSimulationMaster
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// The loaded simulation scene with the controller
        /// </summary>
        protected SimulationPage SimulationPage { get; }
        
        /// <summary>
        /// The loaded Programmable Logic Controller
        /// </summary>
        protected ProgrammableLogicController Plc { get; }
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
        /// Returns true if at least one SFC soon active step is marked from the given set.
        /// Can be called between updates to know which steps will be active.
        /// </summary>
        public bool HasSoonActiveSteps(HashSet<int> checkSet)
        {
            foreach (int entry in checkSet)
            {
                if (Plc.SfcProgramData.ControlMap.ContainsKey(entry)
                && Plc.SfcProgramData.SoonActiveSteps.Contains(Plc.SfcProgramData.ControlMap[entry]))
                {
                    return true;
                }
            }
            return false;
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
