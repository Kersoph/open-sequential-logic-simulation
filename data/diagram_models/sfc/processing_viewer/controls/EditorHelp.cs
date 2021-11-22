using Godot;


namespace Osls.SfcEditor
{
    public class EditorHelp : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public void OnButtonPressed()
        {
            GetNode<EditorControls>("..").SfcEditorNode.HelpPage.Visible = true;
        }
        #endregion
    }
}
