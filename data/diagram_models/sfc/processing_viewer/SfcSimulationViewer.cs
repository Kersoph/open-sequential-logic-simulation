using Godot;
using Osls.Plants;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Top node for the whole sfc simulation view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public class SfcSimulationViewer : PageModule
    {
        #region ==================== Fields / Properties ====================
        private PlantInfoPanel _plantInfoPanel;
        private LessonEntity _openedLesson;
        private Master _simulationMaster;
        private bool _isExecutable;
        private Sfc2dEditorNode _sfc2dEditorNode;
        private SimulationPage _loadedSimulationNode;
        
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
            ProcessingData data = InitialisePlant();
            InitialiseDiagram(data);
            InitialiseSimulation(data);
            _plantInfoPanel = GetNode<PlantInfoPanel>("PlantInfoPanel");
            _plantInfoPanel.SetSimulationInfo(_loadedSimulationNode);
            if (!_isExecutable) GetNode<Label>("Sfc2dViewer/ErrorLabel").Visible = true;
        }
        
        public override void _Process(float delta)
        {
            if (_isExecutable)
            {
                _plantInfoPanel.UpdateText(true);
                int timeMs = (int)(delta * 1000);
                timeMs = timeMs < 1 ? 1 : (timeMs > 1000 ? 1000 : timeMs);
                _simulationMaster.UpdateSimulation(timeMs);
                _simulationMaster.VisualiseStatus(_sfc2dEditorNode.Sfc2dEditorControl);
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Loads the plant node and links the I/O Table
        /// </summary>
        private ProcessingData InitialisePlant()
        {
            _loadedSimulationNode = (SimulationPage)((PackedScene)GD.Load(_openedLesson.SimulationPath)).Instance();
            GetNode("PlantViewNode/PlantViewportContainer/PlantViewport").AddChild(_loadedSimulationNode);
            ProcessingData data = new ProcessingData();
            data.InputRegisters.AssignValuesFrom(_loadedSimulationNode.SimulationOutput);
            data.OutputRegisters.AssignValuesFrom(_loadedSimulationNode.SimulationInput);
            return data;
        }
        
        /// <summary>
        /// Loads the file and builds the SFC diagram
        /// </summary>
        private void InitialiseDiagram(ProcessingData data)
        {
            _sfc2dEditorNode = GetNode<Sfc2dEditorNode>("Sfc2dViewer/Sfc2dEditor");
            _sfc2dEditorNode.InitializeEditor(data);
            string filepath = _openedLesson.FolderPath + "/User/Diagram.sfc";
            _sfc2dEditorNode.TryLoadDiagram(filepath);
        }
        
        /// <summary>
        /// Loads the simulation node and creates a simulation master
        /// </summary>
        private void InitialiseSimulation(ProcessingData data)
        {
            _simulationMaster = new Master(data.SfcEntity, _loadedSimulationNode);
            _isExecutable = _simulationMaster.IsProgramSimulationValid();
        }
        #endregion
    }
}