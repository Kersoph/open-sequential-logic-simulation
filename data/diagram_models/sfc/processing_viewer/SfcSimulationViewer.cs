using Godot;
using Osls.Plants;
using Osls.Core;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Top node for the whole sfc simulation view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public class SfcSimulationViewer : PageModule
    {
        #region ==================== Fields / Properties ====================
        private LessonView _lessonView;
        private ILessonEntity _openedLesson;
        private Master _simulationMaster;
        private bool _isExecutable;
        private Sfc2dEditorNode _sfc2dEditorNode;
        private SimulationPage _loadedSimulationNode;
        private ProcessingData _processingData;
        
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public override PageCategory ScenePage { get { return PageCategory.Simulation; } }
        
        /// <summary>
        /// The currently set execution type of the simulation
        /// </summary>
        public ExecutionType ExecutionType { get; set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole sfc editor
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _openedLesson = openedLesson;
            _processingData = InitialisePlant();
            InitialiseDiagram();
            InitialiseSimulation(mainNode, openedLesson);
            if (!_isExecutable) GetNode<Label>("HscRelative/Sfc2dViewer/ErrorLabel").Visible = true;
        }
        
        public override void _Process(float delta)
        {
            if (_isExecutable)
            {
                switch (ExecutionType)
                {
                    case ExecutionType.RunContinuously:
                        ExecuteSimulation(delta);
                        break;
                    case ExecutionType.RunOneStep:
                        ExecuteSimulation(0.05f);
                        ExecutionType = ExecutionType.Paused;
                        break;
                    case ExecutionType.Paused:
                        break;
                }
            }
        }
        
        /// <summary>
        /// Resets the controller like there was a blackout
        /// </summary>
        public void ResetController()
        {
            _simulationMaster.Reset();
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Loads the plant node and links the I/O Table
        /// </summary>
        private ProcessingData InitialisePlant()
        {
            _lessonView = GetNode<LessonView>("HscRelative/LessonView");
            _lessonView.LoadAndShowInfo(_openedLesson);
            _loadedSimulationNode = _lessonView.PlantView.LoadedSimulationNode;
            ProcessingData data = new ProcessingData();
            data.InputRegisters = new StateTable(_loadedSimulationNode.SimulationOutput);
            data.OutputRegisters = new StateTable(_loadedSimulationNode.SimulationInput);
            return data;
        }
        
        /// <summary>
        /// Loads the file and builds the SFC diagram
        /// </summary>
        private void InitialiseDiagram()
        {
            _sfc2dEditorNode = GetNode<Sfc2dEditorNode>("HscRelative/Sfc2dViewer/Sfc2dEditor");
            _sfc2dEditorNode.InitializeEditor(_processingData);
            string filepath = _openedLesson.TemporaryDiagramFilePath;
            _sfc2dEditorNode.TryLoadDiagram(filepath);
        }
        
        /// <summary>
        /// Loads the simulation node and creates a simulation master
        /// </summary>
        private void InitialiseSimulation(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _loadedSimulationNode.InitialiseWith(mainNode, openedLesson);
            _simulationMaster = new Master(_processingData.SfcEntity, _loadedSimulationNode);
            _loadedSimulationNode.SetupUi();
            _isExecutable = _simulationMaster.IsProgramSimulationValid();
        }
        
        /// <summary>
        /// Executes one step in the simulation with the given delta time
        /// </summary>
        private void ExecuteSimulation(float delta)
        {
            _lessonView.IoInfo.UpdateText(_processingData.InputRegisters, _processingData.OutputRegisters);
            int timeMs = (int)(delta * 1000);
            timeMs = timeMs < 1 ? 1 : (timeMs > 1000 ? 1000 : timeMs);
            _simulationMaster.UpdateSimulation(timeMs);
            _simulationMaster.VisualiseStatus(_sfc2dEditorNode.Sfc2dEditorControl);
        }
        #endregion
    }
}