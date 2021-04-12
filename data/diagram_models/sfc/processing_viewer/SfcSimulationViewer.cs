using Godot;
using Osls.Plants;
using Osls.SfcSimulation.Engine;


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
        private LessonEntity _opendLesson;
        private Master _simulationMaster;
        private bool _isExecutable;
        
        public MainNode MainNode { get; private set; }
        
        public Sfc2dEditorNode Sfc2dEditorNode { get; private set; }
        
        public SimulationPage LoadedSimulationNode { get; private set; }
        
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public override PageCategory ScenePage { get { return PageCategory.Simulation; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole sfc editor
        /// </summary>
        public override void InitialiseWith(MainNode mainNode, LessonEntity opendLesson)
        {
            MainNode = mainNode;
            _opendLesson = opendLesson;
            Sfc2dEditorNode = GetNode<Sfc2dEditorNode>("Sfc2dViewer/Sfc2dEditor");
            Sfc2dEditorNode.InitializeEditor();
            LoadDiagram();
            LoadedSimulationNode = (SimulationPage)((PackedScene)GD.Load(opendLesson.SimulationPath)).Instance();
            GetNode("PlantViewNode/PlantViewportContainer/PlantViewport").AddChild(LoadedSimulationNode);
            SfcEntity sfcEntity = Sfc2dEditorNode.Sfc2dEditorControl.LinkSfcData();
            _simulationMaster = new Master(sfcEntity, LoadedSimulationNode);
            _plantInfoPanel = GetNode<PlantInfoPanel>("PlantInfoPanel");
            _plantInfoPanel.SetSimulationInfo(LoadedSimulationNode);
            _isExecutable = _simulationMaster.IsProgramSimulationValid();
            if (!_isExecutable) GetNode<Label>("Sfc2dViewer/ErrorLabel").Visible = true;
        }
        
        /// <summary>
        /// Loads the file and builds the SFC diagram
        /// </summary>
        public void LoadDiagram()
        {
            string filepath = _opendLesson.FolderPath + "/User/Diagram.sfc";
            Sfc2dEditorNode.TryLoadDiagram(filepath);
        }
        
        public override void _Process(float delta)
        {
            if (_isExecutable)
            {
                _plantInfoPanel.UpdateText(true);
                int timeMs = (int)(delta * 1000);
                timeMs = timeMs < 1 ? 1 : (timeMs > 1000 ? 1000 : timeMs);
                _simulationMaster.UpdateSimulation(timeMs);
                _simulationMaster.VisualiseStatus(Sfc2dEditorNode.Sfc2dEditorControl);
            }
        }
        #endregion
    }
}