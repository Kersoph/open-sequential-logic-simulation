using Godot;
using Osls.SfcEditor.Interpreters;
using Osls.SfcEditor.Interpreters.Boolean;


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
        #endregion
        
        
        #region ==================== Public Methods ====================
        public void Initialise()
        {
            _textEdit = GetNode<TextEdit>("TransitionTextEditor");
            _patchController = GetNode<SfcPatchNode>("..").SfcPatchControl;
            _textEdit.Connect("focus_exited", this, nameof(OnTextEditorFocusExited));
            _textEdit.Connect("mouse_entered", this, nameof(OnMouseEntered));
            _textEdit.Connect("mouse_exited", this, nameof(OnMouseExited));
        }
        
        /// <summary>
        /// Called when the model has changed or is initialized.
        /// </summary>
        public void UpdateTransition(string transitionText, Sfc2dEditorControl contex)
        {
            if (_textEdit != null)
            {
                TransitionMaster.UpdateColorKeys(_textEdit, contex.Data.StepMaster);
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
        #endregion
        
        
        #region ==================== Private Methods ====================
        /// <summary>
        /// The user modified the text: Update the model
        /// </summary>
        private void OnTextEditorFocusExited()
        {
            _patchController.UpdateTransitionTo(_textEdit.Text);
        }
        
        /// <summary>
        /// Visual feedback for the user to change the transition.
        /// </summary>
        private void OnMouseEntered()
        {
            GetNode<Control>("TransitionTexture").Visible = true;
            GetNode<Control>("TransitionTexture").Modulate = new Color(1, 1, 1, 0.5f);
            GetNode<Control>("TransitionTextureHint").Visible = true;
        }
        
        /// <summary>
        /// Reset visual feedback
        /// </summary>
        private void OnMouseExited()
        {
            GetNode<Control>("TransitionTexture").Modulate = new Color(1, 1, 1, 1);
            GetNode<Control>("TransitionTextureHint").Visible = false;
            UpdateVisualRepresentation();
        }

        private void UpdateVisualRepresentation()
        {
            if (string.IsNullOrEmpty(_textEdit.Text))
            {
                GetNode<Control>("TransitionTexture").Visible = false;
                _textEdit.AddColorOverride("background_color", new Color(1, 0, 0, 0f));
            }
            else
            {
                GetNode<Control>("TransitionTexture").Visible = true;
            }
        }
        #endregion
    }
}