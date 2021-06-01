using Godot;

namespace Osls.SfcEditor
{
    /// <summary>
    /// Topmost node for the SfcStepButton.tscn
    /// </summary>
    public class SfcStepVisual : SfcStepVisualBasic
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            GetNode<TextEdit>("StepNameEditor").Connect("focus_exited", this, nameof(OnTextEditorFocusExited));
        }
        
        /// <summary>
        /// Displays the text as a multi line string in the editor if possible.
        /// </summary>
        public override void SetEditorText(string text, Sfc2dEditorControl context)
        {
            GetNode<TextEdit>("StepNameEditor").Text = text;
        }
        
        /// <summary>
        /// Marks or unmarks the step if possible
        /// </summary>
        public override void MarkStep(bool setMark)
        {
            GetNode<Control>("StepMark").Visible = setMark;
        }
        
        /// <summary>
        /// Applies all user edits to the data model.
        /// </summary>
        public override void ApplyEdits()
        {
            string stepName = GetNode<TextEdit>("StepNameEditor").Text;
            GetNode<SfcStepNode>("..").NotifyUserUpdatedName(stepName);
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
        #endregion
    }
}