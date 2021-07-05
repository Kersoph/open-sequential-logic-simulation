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
        public void OnLoadDiagram()
        {
            SfcEditorNode.TryLoadDiagram();
            GetNode<TextInfo>("TextInfo").ShowMessage("Loaded " + SfcEditorNode.OpenedLesson.CustomDiagramFilePath);
        }
        
        /// <summary>
        /// Called by subnodes if the diagram should be saved from the CustomDiagramFilePath
        /// </summary>
        public void OnSaveDiagram()
        {
            SfcEditorNode.SaveDiagram();
            GetNode<TextInfo>("TextInfo").ShowMessage("Saved " + SfcEditorNode.OpenedLesson.CustomDiagramFilePath);
        }
        #endregion
    }
}
