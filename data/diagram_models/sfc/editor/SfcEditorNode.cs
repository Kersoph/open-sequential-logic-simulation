using Godot;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Top node for the whole sfc editor view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public class SfcEditorNode : Node
    {
        #region ==================== Fields Properties ====================
        public MainNode MainNode { get; private set; }
        public PlantViewNode PlantViewNode { get; private set; }
        public Sfc2dEditorNode Sfc2dEditorNode { get; private set; }
        
        private LessonEntity _opendLesson;
        #endregion;
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Initializes the whole sfc editor
        /// </summary>
        public void InitializeEditor(MainNode mainNode, LessonEntity opendLesson)
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