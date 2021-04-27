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
        private const string UnusedStepButton = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepButtonUnused.tscn";
        private const string PassStepButton = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepButtonPass.tscn";
        private const string StepButton = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepButton.tscn";
        private const string StartStepButton = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepButtonStart.tscn";
        private const string StepJump = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/elements/SfcStepButtonJump.tscn";
        private StepType _currentStepType;
        private SfcStepButtonBasic _stepNode;
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
        #endregion
        
        
        #region ==================== Private ====================
        private void ShowStepAs(StepType type)
        {
            if (_stepNode != null)
            {
                _stepNode.QueueFree();
                _stepNode = null;
            }
            string reference = StepButton;
            switch (type)
            {
                case StepType.Unused:
                    reference = UnusedStepButton;
                    break;
                case StepType.Pass:
                    reference = PassStepButton;
                    break;
                case StepType.Step:
                    reference = StepButton;
                    break;
                case StepType.StartingStep:
                    reference = StartStepButton;
                    break;
                case StepType.Jump:
                    reference = StepJump;
                    break;
            }
            _stepNode = (SfcStepButtonBasic)((PackedScene)GD.Load(reference)).Instance();
            AddChild(_stepNode);
        }
        #endregion
    }
}