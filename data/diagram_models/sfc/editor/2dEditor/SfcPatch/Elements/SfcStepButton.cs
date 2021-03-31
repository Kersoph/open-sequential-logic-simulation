using Godot;

namespace Osls.SfcEditor
{
    /// <summary>
    /// Topmost node for the SfcStepButton.tscn
    /// </summary>
    public class SfcStepButton : SfcStepButtonBasic
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            GetNode<TextEdit>("StepNameEditor").Connect("focus_exited", this, nameof(OnTextEditorFocusExited));
        }
        
        /// <summary>
        /// Displays the text as a multi line string in the editor if possible.
        /// </summary>
        public override void SetEditorText(string text)
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
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Called if the line editor looses the focus. (Was edited and the user starts doing someting else)
        /// At that time we want to update the data.
        /// </summary>
        private void OnTextEditorFocusExited()
        {
            string stepName = GetNode<TextEdit>("StepNameEditor").Text;
            GetNode<SfcStepNode>("..").NotifyUserUpdatedName(stepName);
        }
        #endregion
    }
}