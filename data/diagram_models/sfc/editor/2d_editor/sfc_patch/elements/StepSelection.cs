using Godot;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Used to select a step logic for the patch
    /// </summary>
    public class StepSelection : MenuButton
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            var popup = GetPopup();
            popup.Connect("id_pressed", this, nameof(OnItemSelected));
        }
        
        public override void _UnhandledKeyInput(InputEventKey key)
        {
            if (Pressed && key.IsActionPressed("main_remove"))
            {
                OnItemSelected(0);
                Pressed = false;
                ReleaseFocus();
                GetPopup().Visible = false;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void OnItemSelected(int id)
        {
            StepType newType = StepType.Unused;
            switch (id)
            {
                case 0:
                    newType = StepType.Unused;
                    break;
                case 1:
                    newType = StepType.StartingStep;
                    break;
                case 2:
                    newType = StepType.Step;
                    break;
                case 3:
                    newType = StepType.Jump;
                    break;
                case 4:
                    newType = StepType.Pass;
                    break;
            }
            GetNode<SfcStepNode>("..").NotifyStepSelected(newType);
        }
        #endregion
    }
}
