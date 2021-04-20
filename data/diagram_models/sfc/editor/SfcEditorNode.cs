using Godot;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Top node for the whole sfc editor view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public class SfcEditorNode : ModelEditor
    {
        #region ==================== Fields / Properties ====================
        [Export] private NodePath PlantViewNodePath = "PlantViewNode";
        [Export] private NodePath Sfc2dEditorPath = "Sfc2dEditor";
        
        private LessonEntity _openedLesson;
        
        public MainNode MainNode { get; private set; }
        public PlantViewNode PlantViewNode { get; private set; }
        public Sfc2dEditorNode Sfc2dEditorNode { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole page. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(MainNode mainNode, LessonEntity openedLesson)
        {
            MainNode = mainNode;
            _openedLesson = openedLesson;
            PlantViewNode = GetNode<PlantViewNode>(PlantViewNodePath);
            PlantViewNode.UpdateLessonEntity(openedLesson);
            GetNode<PlantInfoPanel>("PlantInfoPanel").SetSimulationInfo(PlantViewNode.LoadedSimulationNode);
            Sfc2dEditorNode = GetNode<Sfc2dEditorNode>(Sfc2dEditorPath);
            ProcessingData data = new ProcessingData();
            data.InputRegisters = new StateTable(PlantViewNode.LoadedSimulationNode.SimulationOutput);
            data.OutputRegisters = new StateTable(PlantViewNode.LoadedSimulationNode.SimulationInput);
            Sfc2dEditorNode.InitializeEditor(data);
            TryLoadDiagram();
        }
        
        /// <summary>
        /// Saves the SFC diagram to a file
        /// </summary>
        public void SaveDiagram()
        {
            string filepath = _openedLesson.FolderPath + "/User/Diagram.sfc";
            Sfc2dEditorNode.SaveDiagram(filepath);
        }
        
        /// <summary>
        /// Loads the file and builds the SFC diagram if the file exists.
        /// </summary>
        public void TryLoadDiagram()
        {
            string filepath = _openedLesson.FolderPath + "/User/Diagram.sfc";
            Sfc2dEditorNode.TryLoadDiagram(filepath);
        }
        #endregion
    }
}