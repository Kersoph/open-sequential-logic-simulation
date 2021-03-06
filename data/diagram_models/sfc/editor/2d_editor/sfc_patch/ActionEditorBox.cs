using Godot;
using System.Collections.Generic;


namespace Osls.SfcEditor
{
    public class ActionEditorBox : Control
    {
        #region ==================== Fields / Properties ====================
        private const string EditorScenePath = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/ActionEditor.tscn";
        
        private SfcPatchControl _patchController;
        private readonly List<ActionEditor> _actionEditors = new List<ActionEditor>();
        private bool _isEditable;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called only once when the node is created by the patch node.
        /// </summary>
        public void InitializeWith(SfcPatchControl sfcPatchControl, bool isEditable)
        {
            _patchController = sfcPatchControl;
            _isEditable = isEditable;
        }
        
        /// <summary>
        /// Called when the model has changed or is initialized.
        /// </summary>
        public void UpdateActions(PatchEntity entity, IProcessingData context)
        {
            UpdateVisibility(entity);
            for (int i = 0; i < entity.ActionEntries.Count; i++)
            {
                if (i >= _actionEditors.Count) AddActionEditor(context);
                _actionEditors[i].UpdateAction(entity.ActionEntries[i], context);
            }
            if (_isEditable)
            {
                if (entity.ActionEntries.Count >= _actionEditors.Count) AddActionEditor(context);
                _actionEditors[entity.ActionEntries.Count].ResetAction();
                for (int i = entity.ActionEntries.Count + 1; i < _actionEditors.Count; i++)
                {
                    RemoveActionEditor(_actionEditors[i]);
                }
            }
        }
        
        /// <summary>
        /// Called by the action editor to update an entry.
        /// </summary>
        public void UserChangedData(ActionEditor editor, ActionQualifier qualifier, string action)
        {
            int index = _actionEditors.IndexOf(editor);
            _patchController.UpdateActionTo(index, qualifier, action);
        }
        
        /// <summary>
        /// Called by the action editor to add a new entry as it is getting relevant.
        /// </summary>
        public void AddNewAction()
        {
            _patchController.AddNewAction();
        }
        
        /// <summary>
        /// Called by the action editor to remove an entry as it is no longer relevant.
        /// </summary>
        public void RemoveAction(ActionEditor editor)
        {
            int index = _actionEditors.IndexOf(editor);
            _patchController.RemoveActionAt(index);
        }
        
        /// <summary>
        /// Applies all user edits to the data model.
        /// </summary>
        public void ApplyAllEdits()
        {
            for (int i = 0; i < _actionEditors.Count; i++)
            {
                _actionEditors[i].ApplyAllEdits();
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void UpdateVisibility(PatchEntity entity)
        {
            Visible = entity.ContainsRealStep();
        }
        
        private void AddActionEditor(IProcessingData data)
        {
            Node node = ((PackedScene)GD.Load(EditorScenePath)).Instance();
            ActionEditor actionEditor = (ActionEditor)node;
            _actionEditors.Add(actionEditor);
            GetNode<BoxContainer>("ScrollContainer/VerticalBoxContainer").AddChild(actionEditor);
            actionEditor.InitializeWith(this, data);
        }
        
        private void RemoveActionEditor(ActionEditor actionEditor)
        {
            _actionEditors.Remove(actionEditor);
            actionEditor.QueueFree();
        }
        #endregion
    }
}
