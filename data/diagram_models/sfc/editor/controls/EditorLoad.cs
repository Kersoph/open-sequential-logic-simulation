using Godot;


namespace Osls.SfcEditor
{
    public class EditorLoad : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public void OnButtonPressed()
        {
            GetNode<FileDialog>("../FileDialog").LoadFile();
        }
        #endregion
    }
}