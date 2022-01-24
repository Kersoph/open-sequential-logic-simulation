using Godot;


namespace Osls.SfcEditor
{
    public class EditorControls : Control
    {
        #region ==================== Fields / Properties ====================
        [Export] private NodePath SfcEditorNodePath = "../../..";
        public SfcEditorNode SfcEditorNode { get { return GetNode<SfcEditorNode>(SfcEditorNodePath); } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called by subnodes if the diagram should be loaded from the CustomDiagramFilePath
        /// </summary>
        public void LoadDiagram()
        {
            SfcEditorNode.TryLoadDiagram();
        }
        
        /// <summary>
        /// Called by the editor when the diagram was loaded
        /// </summary>
        public void OnLoadDiagram()
        {
            GetNode<TextInfo>("TextInfo").ShowMessage("Loaded " + SfcEditorNode.OpenedLesson.CustomDiagramFilePath);
        }
        
        /// <summary>
        /// Called by subnodes if the diagram should be saved from the CustomDiagramFilePath
        /// </summary>
        public void SaveDiagram()
        {
            SfcEditorNode.SaveDiagram();
        }
        
        /// <summary>
        /// Called by the editor when de diagram was saved
        /// </summary>
        public void OnSaveDiagram(Error result)
        {
            switch (result)
            {
                case Error.Ok:
                    GetNode<TextInfo>("TextInfo").ShowMessage("Saved " + SfcEditorNode.OpenedLesson.CustomDiagramFilePath);
                    break;
                case Error.FileNoPermission:
                    GetNode<TextInfo>("TextInfo").ShowMessage("We do not have write permission at: " + SfcEditorNode.OpenedLesson.CustomDiagramFilePath);
                    break;
                default:
                    GetNode<TextInfo>("TextInfo").ShowMessage("Could not save: " + SfcEditorNode.OpenedLesson.CustomDiagramFilePath + " " + result);
                    break;
            }
        }
        #endregion
    }
}
