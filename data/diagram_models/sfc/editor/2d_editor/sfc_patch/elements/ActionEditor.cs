using Godot;
using Osls.SfcEditor.Interpreters;
using Osls.St.Assignment;


namespace Osls.SfcEditor
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
        public void InitializeWith(ActionEditorBox controller, IProcessingData data)
        {
            _controller = controller;
            _actionDescriptionNode = GetNode<TextEdit>("ActionTextEditor");
            _actionDescriptionNode.Connect("focus_exited", this, nameof(OnTextEditorFocusExited));
            _qualifierNode = GetNode<MenuButton>("ActionQualifierSelector");
            ConfigureQualifierPopupMenu();
            ActionMaster.UpdateColorKeys(_actionDescriptionNode, data);
        }
        
        /// <summary>
        /// Called when the model has changed or is initialized.
        /// </summary>
        public void UpdateAction(ActionEntity action, IProcessingData context)
        {
            _selectedQualifier = action.Qualifier;
            SetQualifierText((int)_selectedQualifier);
            _actionDescriptionNode.Text = action.Action;
            _isRelevant = !string.IsNullOrEmpty(_actionDescriptionNode.Text);
            AssignmentExpression expression = ActionMaster.InterpretTransitionText(action.Action, context);
            _actionDescriptionNode.HintTooltip = expression == null ? "???" : expression.ToString();
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
        
        /// <summary>
        /// Applies the edits to address immediate data.
        /// Used when the scene changes or is saved.
        /// </summary>
        public void ApplyAllEdits()
        {
            CheckRelevancy();
            if (_isRelevant) _controller.UserChangedData(this, _selectedQualifier, _actionDescriptionNode.Text);
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
            if (_isRelevant) _controller.UserChangedData(this, _selectedQualifier, _actionDescriptionNode.Text);
        }
        
        private void OnPopupElementSelected(int id)
        {
            _selectedQualifier = (ActionQualifier)id;
            SetQualifierText(id);
            if (_isRelevant) _controller.UserChangedData(this, _selectedQualifier, _actionDescriptionNode.Text);
        }
        
        private void SetQualifierText(int qualifierId)
        {
            _qualifierNode.Text = _qualifierNode.GetPopup().GetItemText(qualifierId);
        }
        
        private void ConfigureQualifierPopupMenu()
        {
            PopupMenu menu = _qualifierNode.GetPopup();
            menu.AddItem("N");
            menu.AddItem("P+");
            menu.AddItem("P-");
            menu.Connect("id_pressed", this, nameof(OnPopupElementSelected));
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