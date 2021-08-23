using Godot;
using Osls.SfcEditor.Interpreters;
using Osls.St.Boolean;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Topmost node for the SfcTransitionNode.tscn
    /// Controls the appearance of the transition.
    /// </summary>
    public class SfcTransitionNode : Control
    {
        #region ==================== Fields Properties ====================
        private TextEdit _textEdit;
        private SfcPatchControl _patchController;
        private bool _hover;
        private bool _focus;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public void Initialise()
        {
            _textEdit = GetNode<TextEdit>("TransitionTextEditor");
            _patchController = GetNode<SfcPatchNode>("..").SfcPatchControl;
            _textEdit.Connect("focus_exited", this, nameof(OnTextEditorFocusExited));
            _textEdit.Connect("mouse_entered", this, nameof(OnMouseEntered));
            _textEdit.Connect("mouse_exited", this, nameof(OnMouseExited));
            _textEdit.Connect("focus_entered", this, nameof(OnFocusEntered));
            _textEdit.Connect("focus_exited", this, nameof(OnFocusExited));
            _textEdit.Connect("text_changed", this, nameof(OnTextChanged));
        }
        
        /// <summary>
        /// Called when the model has changed or is initialized.
        /// </summary>
        public void UpdateTransition(string transitionText, Sfc2dEditorControl contex)
        {
            if (_textEdit != null)
            {
                TransitionMaster.UpdateColorKeys(_textEdit, contex.Data);
                _textEdit.Text = transitionText;
                UpdateVisualRepresentation();
                if (!string.IsNullOrEmpty(transitionText))
                {
                    BooleanExpression expression = TransitionMaster.InterpretTransitionText(transitionText, contex.Data);
                    _textEdit.HintTooltip = expression == null ? "???" : expression.ToString();
                    bool validExpression = expression != null && expression.IsValid();
                    Color background = validExpression ? new Color(1, 0, 0, 0f) : new Color(1, 0, 0, 0.2f);
                    _textEdit.AddColorOverride("background_color", background);
                }
            }
        }
        
        /// <summary>
        /// Applies all user edits to the data model.
        /// </summary>
        public void ApplyEdits()
        {
            if (_patchController.Data.TransitionText != _textEdit.Text)
            {
                _patchController.UpdateTransitionTo(_textEdit.Text);
            }
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        /// <summary>
        /// The user modified the text: Update the model
        /// </summary>
        private void OnTextEditorFocusExited()
        {
            ApplyEdits();
        }
        
        /// <summary>
        /// Visual feedback for the user to change the transition.
        /// </summary>
        private void OnMouseEntered()
        {
            _hover = true;
            UpdateVisualRepresentation();
        }
        
        /// <summary>
        /// Reset visual feedback
        /// </summary>
        private void OnMouseExited()
        {
            _hover = false;
            UpdateVisualRepresentation();
        }
        
        /// <summary>
        /// Visual feedback for the user when the transition is edited
        /// </summary>
        private void OnFocusEntered()
        {
            _focus = true;
            UpdateVisualRepresentation();
        }
        
        /// <summary>
        /// Reset visual feedback
        /// </summary>
        private void OnFocusExited()
        {
            _focus = false;
            UpdateVisualRepresentation();
        }
        
        /// <summary>
        /// Confirmes the text when a new line is added (the user pressed enter)
        /// By removing the char and lose the focus.
        /// </summary>
        private void OnTextChanged()
        {
            string currentText = _textEdit.Text;
            int escapePosition = currentText.IndexOf('\n');
            if (escapePosition >= 0)
            {
                string subtext = currentText.Remove(escapePosition, 1);
                _textEdit.Text = subtext;
                _textEdit.ReleaseFocus();
            }
        }
        
        private void UpdateVisualRepresentation()
        {
            if (string.IsNullOrEmpty(_textEdit.Text) && !_focus && !_hover)
            {
                GetNode<Control>("TransitionTexture").Visible = false;
                _textEdit.AddColorOverride("background_color", new Color(1, 0, 0, 0f));
                GetNode<Control>("TransitionTextureHint").Visible = false;
            }
            else
            {
                GetNode<Control>("TransitionTexture").Visible = true;
                GetNode<Control>("TransitionTextureHint").Visible = true;
            }
        }
        #endregion
    }
}