namespace Osls.SfcEditor
{
    /// <summary>
    /// Top node for the whole sfc editor view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public class SfcEditorNode : PageModule
    {
        #region ==================== Fields / Properties ====================
        private LessonEntity _opendLesson;
        
        public MainNode MainNode { get; private set; }
        public PlantViewNode PlantViewNode { get; private set; }
        public Sfc2dEditorNode Sfc2dEditorNode { get; private set; }
        
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public override PageCategory ScenePage { get { return PageCategory.LogicEditor; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole page. Called bevore the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(MainNode mainNode, LessonEntity opendLesson)
        {
            MainNode = mainNode;
            _opendLesson = opendLesson;
            PlantViewNode = GetNode<PlantViewNode>("PlantViewNode");
            PlantViewNode.UpdateLessonEntity(opendLesson);
            GetNode<PlantInfoPanel>("PlantInfoPanel").SetSimulationInfo(PlantViewNode.LoadedSimulationNode);
            Sfc2dEditorNode = GetNode<Sfc2dEditorNode>("ViewportContainer/Viewport/Sfc2dEditor");
            Sfc2dEditorNode.InitializeEditor();
            TryLoadDiagram();
        }
        
        /// <summary>
        /// Saves the SFC diagram to a file
        /// </summary>
        public void SaveDiagram()
        {
            string filepath = _opendLesson.FolderPath + "/User/Diagram.sfc";
            Sfc2dEditorNode.SaveDiagram(filepath);
        }
        
        /// <summary>
        /// Loads the file and builds the SFC diagram if the file exists.
        /// </summary>
        public void TryLoadDiagram()
        {
            string filepath = _opendLesson.FolderPath + "/User/Diagram.sfc";
            Sfc2dEditorNode.TryLoadDiagram(filepath);
        }
        #endregion
    }
}