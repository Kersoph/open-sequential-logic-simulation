using Godot;


namespace Osls.SfcEditor
{
    public class EditorControls : Control
    {
        #region ==================== Fields / Properties ====================
        [Export] private NodePath SfcEditorNodePath = "../..";
        public SfcEditorNode SfcEditorNode { get { return GetNode<SfcEditorNode>(SfcEditorNodePath); } }
        #endregion
    }
}
