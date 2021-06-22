using Godot;
using Osls.Core;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Top node for the whole sfc editor view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public class SfcEditorNode : ModelEditor
    {
        #region ==================== Fields / Properties ====================
        [Export] private NodePath LessonViewPath = "LessonView";
        [Export] private NodePath Sfc2dEditorPath = "Sfc2dBackground/Sfc2dEditor";
        
        public ILessonEntity OpenedLesson { get; private set; }
        public IMainNode MainNode { get; private set; }
        public LessonView LessonView { get; private set; }
        public Sfc2dEditorNode Sfc2dEditorNode { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole page. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            MainNode = mainNode;
            OpenedLesson = openedLesson;
            LessonView = GetNode<LessonView>(LessonViewPath);
            LessonView.LoadAndShowInfo(openedLesson);
            Sfc2dEditorNode = GetNode<Sfc2dEditorNode>(Sfc2dEditorPath);
            ProcessingData data = new ProcessingData();
            data.InputRegisters = new StateTable(LessonView.PlantView.LoadedSimulationNode.SimulationOutput);
            data.OutputRegisters = new StateTable(LessonView.PlantView.LoadedSimulationNode.SimulationInput);
            Sfc2dEditorNode.InitializeEditor(data);
            TryLoadDiagram();
        }
        
        /// <summary>
        /// Saves the SFC diagram to a file
        /// </summary>
        public void SaveDiagram()
        {
            string filepath = OpenedLesson.CustomDiagramFilePath;
            Sfc2dEditorNode.SaveDiagram(filepath);
        }
        
        /// <summary>
        /// Loads the file and builds the SFC diagram if the file exists.
        /// </summary>
        public void TryLoadDiagram()
        {
            string filepath = OpenedLesson.CustomDiagramFilePath;
            if (string.IsNullOrEmpty(filepath))
            {
                filepath = OpenedLesson.FolderPath + LessonEntity.UserResultDirectory + "/Diagram.sfc";
                OpenedLesson.CustomDiagramFilePath = filepath;
            }
            Sfc2dEditorNode.TryLoadDiagram(filepath);
        }
        
        /// <summary>
        /// Saves the data to a temporary file which is always written when the scene changes
        /// </summary>
        public void SerialiseToTemp()
        {
            Sfc2dEditorNode.SaveDiagram(OpenedLesson.TemporaryDiagramFilePath);
        }
        
        /// <summary>
        /// Requests a change of the current page to the new page.
        /// Used to provide the possibility for the user to save or cancel the action.
        /// </summary>
        public override void OnUserRequestsChange(IMainNode mainNode, PageCategory nextPage)
        {
            Sfc2dEditorNode.Sfc2dEditorControl.ApplyAllEdits();
            SaveDiagram();
            SerialiseToTemp();
            mainNode.ChangePageTo(nextPage);
        }
        #endregion
    }
}