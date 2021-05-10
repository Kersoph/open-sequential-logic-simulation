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
        private LessonEntity _openedLesson;
        private Master _simulationMaster;
        private bool _isExecutable;
        private Sfc2dEditorNode _sfc2dEditorNode;
        private SimulationPage _loadedSimulationNode;
        private ProcessingData _processingData;
        
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public override PageCategory ScenePage { get { return PageCategory.Simulation; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole sfc editor
        /// </summary>
        public override void InitialiseWith(MainNode _mainNode, LessonEntity openedLesson)
        {
            _openedLesson = openedLesson;
            _processingData = InitialisePlant();
            InitialiseDiagram();
            InitialiseSimulation();
            if (!_isExecutable) GetNode<Label>("Sfc2dViewer/ErrorLabel").Visible = true;
        }
        
        public override void _Process(float delta)
        {
            if (_isExecutable)
            {
                _lessonView.IoInfo.UpdateText(_processingData.InputRegisters, _processingData.OutputRegisters);
                int timeMs = (int)(delta * 1000);
                timeMs = timeMs < 1 ? 1 : (timeMs > 1000 ? 1000 : timeMs);
                _simulationMaster.UpdateSimulation(timeMs);
                _simulationMaster.VisualiseStatus(_sfc2dEditorNode.Sfc2dEditorControl);
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
            _lessonView = GetNode<LessonView>("LessonView");
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
            _sfc2dEditorNode = GetNode<Sfc2dEditorNode>("Sfc2dViewer/Sfc2dEditor");
            _sfc2dEditorNode.InitializeEditor(_processingData);
            string filepath = _openedLesson.TemporaryDiagramFilePath;
            _sfc2dEditorNode.TryLoadDiagram(filepath);
        }
        
        /// <summary>
        /// Loads the simulation node and creates a simulation master
        /// </summary>
        private void InitialiseSimulation()
        {
            _simulationMaster = new Master(_processingData.SfcEntity, _loadedSimulationNode);
            _isExecutable = _simulationMaster.IsProgramSimulationValid();
        }
        #endregion
    }
}