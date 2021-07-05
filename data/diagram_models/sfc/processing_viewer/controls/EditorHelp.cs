using Godot;


namespace Osls.SfcSimulation.EditorControls
{
    public class EditorHelp : Button
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("pressed", this, nameof(OnPressed));
        }
        
        public void OnPressed()
        {
            GetNode<Control>("../Help").Visible = true;
        }
        #endregion
    }
}