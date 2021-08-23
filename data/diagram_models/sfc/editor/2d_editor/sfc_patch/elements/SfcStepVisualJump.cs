using Godot;


namespace Osls.SfcEditor
{
    public class SfcStepVisualJump : SfcStepVisualBasic
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            GetNode<TextEdit>("StepNameEditor").Connect("focus_exited", this, nameof(OnTextEditorFocusExited));
            GetNode<TextEdit>("StepNameEditor").Connect("text_changed", this, nameof(OnTextChanged));
        }
        
        /// <summary>
        /// Displays the text as a multi line string in the editor if possible.
        /// </summary>
        public override void SetEditorText(string text, Sfc2dEditorControl context)
        {
            TextEdit node = GetNode<TextEdit>("StepNameEditor");
            node.Text = text;
            bool validReference = context.Data.StepMaster.ContainsStep(text);
            Color background = validReference ? new Color(1, 0, 0, 0f) : new Color(1, 0, 0, 0.2f);
            node.AddColorOverride("background_color", background);
        }
        
        /// <summary>
        /// Applies all user edits to the data model.
        /// </summary>
        public override void ApplyEdits()
        {
            string stepName = GetNode<TextEdit>("StepNameEditor").Text;
            GetNode<SfcStepNode>("..").NotifyUserUpdatedName(stepName);
        }
        
        /// <summary>
        /// Called when this step was changed to directly delegate the focus to the control if needed.
        /// </summary>
        public override void OnCreationFocus()
        {
            GetNode<TextEdit>("StepNameEditor").CallDeferred("grab_focus");
            GetNode<TextEdit>("StepNameEditor").CallDeferred("select_all");
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Called if the line editor looses the focus. (Was edited and the user starts doing someting else)
        /// At that time we want to update the data.
        /// </summary>
        private void OnTextEditorFocusExited()
        {
            ApplyEdits();
        }
        
        /// <summary>
        /// Called if the string in the text editor changed.
        /// If a newline is added (enter pressed) we want to loose focus to push the changes.
        /// </summary>
        private void OnTextChanged()
        {
            string currentText = GetNode<TextEdit>("StepNameEditor").Text;
            int escapePosition = currentText.IndexOf('\n');
            if (escapePosition >= 0)
            {
                string subtext = currentText.Remove(escapePosition, 1);
                GetNode<TextEdit>("StepNameEditor").Text = subtext;
                GetNode<TextEdit>("StepNameEditor").ReleaseFocus();
            }
        }
        #endregion
    }
}