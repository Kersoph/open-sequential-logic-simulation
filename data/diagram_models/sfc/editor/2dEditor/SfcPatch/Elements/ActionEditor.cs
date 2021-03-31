using Godot;
using SfcSandbox.Data.Model.SfcEditor.Interpreter;
using SfcSandbox.Data.Model.SfcEditor.Interpreter.Assignment;

namespace SfcSandbox.Data.Model.SfcEditor
{
    public class ActionEditor : Control
    {
        #region ==================== Public ====================
        private ActionEditorBox _controller;
        private TextEdit _actionDescriptionNode;
        private MenuButton _qualifierNode;
        private ActionQualifier _selectedQualifier;
        private bool _isRelevant = false;
        #endregion
        
        
        #region ==================== Public ====================
        public void InitializeWith(ActionEditorBox controller)
        {
            _controller = controller;
            _actionDescriptionNode = GetNode<TextEdit>("ActionTextEditor");
            _actionDescriptionNode.Connect("focus_exited", this, nameof(OnTextEditorFocusExited));
            _qualifierNode = GetNode<MenuButton>("ActionQualifierSelector");
            ConfigureQualifierPopupMenu();
            ActionMaster.UpdateColorKeys(_actionDescriptionNode);
        }
        
        /// <summary>
        /// Called when the model has changed or is initialized.
        /// </summary>
        public void UpdateAction(SfcActionEntity action)
        {
            _selectedQualifier = action.Qualifier;
            SetQualifierText((int)_selectedQualifier);
            _actionDescriptionNode.Text = action.Action;
            _isRelevant = !string.IsNullOrEmpty(_actionDescriptionNode.Text);
            AssignmentExpression expression = ActionMaster.InterpretTransitionText(action.Action);
            _actionDescriptionNode.SetTooltip(expression == null ? "???" : expression.ToString());
            bool validExpression = expression != null && expression.IsValid();
            Color background = validExpression ? new Color(1, 0, 0, 0f) : new Color(1, 0, 0, 0.2f);
            _actionDescriptionNode.AddColorOverride("background_color", background);
        }
        
        /// <summary>
        /// Resets the action text but leaves the qualifier set as it is not relevant for inactive fields.
        /// </summary>
        public void ResetAction()
        {
            _actionDescriptionNode.Text = string.Empty;
            _isRelevant = false;
        }
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Called if the text editor looses the focus. (Was edited and the user starts doing someting else)
        /// At that time we want to update the data.
        /// </summary>
        private void OnTextEditorFocusExited()
        {
            CheckRelevancy();
            if(_isRelevant) _controller.UserChangedData(this, _selectedQualifier, _actionDescriptionNode.Text);
        }
        
        private void OnPopupElementSelected(int id)
        {
            _selectedQualifier = (ActionQualifier)id;
            SetQualifierText(id);
            if(_isRelevant) _controller.UserChangedData(this, _selectedQualifier, _actionDescriptionNode.Text);
        }
        
        private void SetQualifierText(int qualifierId)
        {
            _qualifierNode.Text = _qualifierNode.GetPopup().GetItemText(qualifierId);
        }
        
        private void ConfigureQualifierPopupMenu()
        {
            PopupMenu menue = _qualifierNode.GetPopup();
            menue.AddItem("N");
            menue.AddItem("P+");
            menue.AddItem("P-");
            menue.Connect("id_pressed", this, nameof(OnPopupElementSelected));
        }
        
        private void CheckRelevancy()
        {
            bool isRelevantNext = !string.IsNullOrEmpty(_actionDescriptionNode.Text);
            if (isRelevantNext != _isRelevant)
            {
                _isRelevant = isRelevantNext;
                if (isRelevantNext)
                {
                    _controller.AddNewAction();
                }
                else
                {
                    _controller.RemoveAction(this);
                }
            }
        }
        #endregion
    }
}