using Osls.SfcEditor;


namespace Osls.SfcSimulation.Engine
{
    /// <summary>
    /// The simulation engine master which controls and updates the plant and the PLC.
    /// </summary>
    public class Master
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// Reference frequency of the simulation steps in Hz.
        /// </summary>
        public const int StepUpdateFrequency = 60;
        
        /// <summary>
        /// Reference update time of the simulation in seconds.
        /// </summary>
        public const float StepUpdateTime = 1f / (float)StepUpdateFrequency;
        
        /// <summary>
        /// The loaded simulation scene with the controller
        /// </summary>
        public SimulationControlNode SimulationControlNode {get; private set; }
        
        /// <summary>
        /// Holds the referenced sfc 2d editor control with the logical diagram
        /// </summary>
        public Sfc2dEditorControl Sfc2dEditorControl { get; private set; }
        
        private readonly ProgrammableLogicController _programmableLogicController;
        #endregion
        
        
        #region ==================== Constructor ====================
        public Master(Sfc2dEditorControl sfc2dEditorControl, SimulationControlNode simulationControl)
        {
            Sfc2dEditorControl = sfc2dEditorControl;
            SimulationControlNode = simulationControl;
            _programmableLogicController = new ProgrammableLogicController(this);
            _programmableLogicController.Startup();
        }
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Is called every frame to calculate the next steps of the simulation.
        /// We update n times first the simulation and then the controller.
        /// </summary>
        public void UpdateSimulation()
        {
            SimulationControlNode.UpdateModel();
            _programmableLogicController.Update();
            _programmableLogicController.VisualiseStatus();
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsProgramSimulationValid()
        {
            return _programmableLogicController.IsPlcLogicValid();
        }
        #endregion
    }
}