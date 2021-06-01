using Godot;

namespace Osls.SfcEditor
{
    /// <summary>
    /// Topmost node for the SfcStepNode.tscn
    /// Controls the visual appearance of the SFC Step
    /// </summary>
    public class SfcStepNode : ReferenceRect
    {
        #region ==================== Fields Properties ====================
        private const string UnusedStep = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepVisualUnused.tscn";
        private const string PassStep = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepVisualPass.tscn";
        private const string Step = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepVisual.tscn";
        private const string StartStep = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepVisualStart.tscn";
        private const string StepJump = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepVisualJump.tscn";
        private StepType _currentStepType;
        private SfcStepVisualBasic _stepNode;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Updates the visible nodes according to the given step type.
        /// </summary>
        public void UpdateStep(PatchEntity data, Sfc2dEditorControl context)
        {
            if (data.SfcStepType != _currentStepType || _stepNode == null)
            {
                ShowStepAs(data.SfcStepType);
                _currentStepType = data.SfcStepType;
            }
            _stepNode.SetEditorText(data.StepName, context);
        }
        
        /// <summary>
        /// Called by sub nodes when the step selection was updated
        /// </summary>
        public void NotifyStepSelected(StepType selectedType)
        {
            SfcPatchControl controller = GetNode<SfcPatchNode>("..").SfcPatchControl;
            controller.UpdateSfcStepTo(selectedType);
        }
        
        /// <summary>
        /// Called when the user changes the sfc step name.
        /// </summary>
        public void NotifyUserUpdatedName(string name)
        {
            GetNode<SfcPatchNode>("..").SfcPatchControl.UpdateNameTo(name);
        }
        
        /// <summary>
        /// Marks or unmarks the step if possible
        /// </summary>
        public void MarkStep(bool setMark)
        {
            _stepNode.MarkStep(setMark);
        }
        
        /// <summary>
        /// Shows or hides the content hint for the user
        /// </summary>
        public void HintContent(bool active)
        {
            if (_currentStepType == StepType.Unused)
            {
                GetNode<Control>("HintPatch").Visible = active;
            }
            else
            {
                GetNode<Control>("HintPatch").Visible = false;
            }
        }
        
        /// <summary>
        /// Applies all user edits to the data model.
        /// Called when the editor is closed or the data saved.
        /// </summary>
        public void ApplyAllEdits()
        {
            _stepNode.ApplyEdits();
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void ShowStepAs(StepType type)
        {
            if (_stepNode != null)
            {
                _stepNode.QueueFree();
                _stepNode = null;
            }
            string reference = Step;
            switch (type)
            {
                case StepType.Unused:
                    reference = UnusedStep;
                    break;
                case StepType.Pass:
                    reference = PassStep;
                    break;
                case StepType.Step:
                    reference = Step;
                    break;
                case StepType.StartingStep:
                    reference = StartStep;
                    break;
                case StepType.Jump:
                    reference = StepJump;
                    break;
            }
            _stepNode = (SfcStepVisualBasic)((PackedScene)GD.Load(reference)).Instance();
            AddChild(_stepNode);
        }
        #endregion
    }
}