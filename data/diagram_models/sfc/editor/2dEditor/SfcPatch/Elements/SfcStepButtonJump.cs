using Godot;
using Osls.SfcEditor.Interpreter;

namespace Osls.SfcEditor
{
    public class SfcStepButtonJump : SfcStepButtonBasic
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
            TextEdit node = GetNode<TextEdit>("StepNameEditor");
            node.Text = text;
            bool validReference = StepMaster.ContainsStep(text);
            Color background = validReference ? new Color(1, 0, 0, 0f) : new Color(1, 0, 0, 0.2f);
            node.AddColorOverride("background_color", background);
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