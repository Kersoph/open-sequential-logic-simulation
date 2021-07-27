using Godot;


namespace Osls.SfcEditor.Controls
{
    public class EditorWrite : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnButtonPressed));
        }
        
        public void OnButtonPressed()
        {
            GetNode<FileDialog>("../FileDialog").SaveFile();
        }
        #endregion
    }
}