using Godot;

namespace Osls.SfcEditor
{
    /// <summary>
    /// Topmost node for the SfcStepNode.tscn
    /// Controls the visual appearance of the SFC Step
    /// </summary>
    public class SfcStepNode : Control
    {
        #region ==================== Fields Properties ====================
        private const string UnusedStepButton = "res://Data/Model/SfcEditor/2dEditor/SfcPatch/Elements/SfcStepButtonUnused.tscn";
        private const string PassStepButton = "res://Data/Model/SfcEditor/2dEditor/SfcPatch/Elements/SfcStepButtonPass.tscn";
        private const string StepButton = "res://Data/Model/SfcEditor/2dEditor/SfcPatch/Elements/SfcStepButton.tscn";
        private const string StartStepButton = "res://Data/Model/SfcEditor/2dEditor/SfcPatch/Elements/SfcStepButtonStart.tscn";
        private const string StepJump = "res://Data/Model/SfcEditor/2dEditor/SfcPatch/Elements/SfcStepButtonJump.tscn";
        private SfcStepType _currentStepType;
        private SfcStepButtonBasic _stepNode;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Updates the visivle nodes according to the given step type.
        /// </summary>
        public void UpdateStep(SfcPatchEntity data)
        {
            if (data.SfcStepType != _currentStepType || _stepNode == null)
            {
                ShowStepAs(data.SfcStepType);
                _currentStepType = data.SfcStepType;
            }
            _stepNode.SetEditorText(data.StepName);
        }
        
        /// <summary>
        /// Called by sub nodes: The current button was pressed.
        /// </summary>
        public void NotifyButtonPressed()
        {
            // Todo: Later we will provide direct options with a gui
            SfcPatchControl controller = GetNode<SfcPatchNode>("..").SfcPatchControl;
            switch (_currentStepType)
            {
                case SfcStepType.Unused:
                    controller.UpdateSfcSetpTo(SfcStepType.Step);
                    break;
                case SfcStepType.Step:
                    controller.UpdateSfcSetpTo(SfcStepType.StartingStep);
                    break;
                case SfcStepType.StartingStep:
                    controller.UpdateSfcSetpTo(SfcStepType.Pass);
                    break;
                case SfcStepType.Pass:
                    controller.UpdateSfcSetpTo(SfcStepType.Jump);
                    break;
                case SfcStepType.Jump:
                    controller.UpdateSfcSetpTo(SfcStepType.Unused);
                    break;
            }
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
        private void ShowStepAs(SfcStepType type)
        {
            if (_stepNode != null)
            {
                _stepNode.QueueFree();
                _stepNode = null;
            }
            string reference = StepButton;
            switch (type)
            {
                case SfcStepType.Unused:
                    reference = UnusedStepButton;
                    break;
                case SfcStepType.Pass:
                    reference = PassStepButton;
                    break;
                case SfcStepType.Step:
                    reference = StepButton;
                    break;
                case SfcStepType.StartingStep:
                    reference = StartStepButton;
                    break;
                case SfcStepType.Jump:
                    reference = StepJump;
                    break;
            }
            _stepNode = (SfcStepButtonBasic)((PackedScene)GD.Load(reference)).Instance();
            this.AddChild(_stepNode);
        }
        #endregion
    }
}