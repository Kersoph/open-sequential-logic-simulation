using Godot;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Control class for a SFC patch
    /// </summary>
    public class SfcPatchControl
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// The patch model entity.
        /// </summary>
        public SfcPatchEntity Data { get; private set; }
        /// <summary>
        /// The connected node controlled by this class
        /// </summary>
        public SfcPatchNode SfcPatchNode { get; private set; }
        
        private Sfc2dEditorControl _control;
        private const string _patchReferencePath = "res://Data/Model/SfcEditor/2dEditor/SfcPatch/SfcPatch.tscn";
        #endregion;
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Contstuctor. It will add the patch to the 2d editor.
        /// </summary>
        public SfcPatchControl(SfcPatchEntity data, Sfc2dEditorControl control)
        {
            Data = data;
            Node node = ((PackedScene)GD.Load(_patchReferencePath)).Instance();
            SfcPatchNode = (SfcPatchNode)node;
            SfcPatchNode.InitializeWith(this, data);
            control.ReferenceRect.AddChild(SfcPatchNode);
            _control = control;
        }
        
        /// <summary>
        /// Updates the visible nodes according to the data model.
        /// </summary>
        public void UpdatePatchNodes()
        {
            SfcPatchNode.UpdateNodes(Data);
        }
        
        /// <summary>
        /// Called when the user changes the sfc step name.
        /// </summary>
        public void UpdateNameTo(string name)
        {
            Data.StepName = name;
            _control.UpdateGrid();
        }
        
        /// <summary>
        /// Called if the user toggled the top branch/merge line.
        /// This will update the model.
        /// </summary>
        public void TopLineToggled()
        {
            if (Data.UpperBranch == SfcBranchLineType.Unused)
            {
                Data.UpperBranch = SfcBranchLineType.Single;
            }
            else
            {
                Data.UpperBranch = SfcBranchLineType.Unused;
            }
            _control.UpdateGrid();
        }
        
        /// <summary>
        /// Called if the user toggled the bottom branch/merge line.
        /// This will update the model.
        /// </summary>
        public void BotLineToggled()
        {
            if (Data.LowerBranch == SfcBranchLineType.Unused)
            {
                Data.LowerBranch = SfcBranchLineType.Single;
            }
            else
            {
                Data.LowerBranch = SfcBranchLineType.Unused;
            }
            _control.UpdateGrid();
        }
        
        /// <summary>
        /// Called if the user updated the transition.
        /// This will update the model.
        /// </summary>
        public void UpdateTransitionTo(string transitionText)
        {
            Data.TransitionText = transitionText;
            UpdatePatchNodes();
        }
        
        /// <summary>
        /// Called if the user changed the step.
        /// This will update the model.
        /// </summary>
        public void UpdateSfcSetpTo(SfcStepType sfcStepType)
        {
            Data.SfcStepType = sfcStepType;
            _control.UpdateGrid();
        }
        
        /// <summary>
        /// Called if the user changed an action entry.
        /// This will update the model.
        /// </summary>
        public void UpdateActionTo(int index, ActionQualifier qualifier, string action)
        {
            Data.ActionEntries[index].Qualifier = qualifier;
            Data.ActionEntries[index].Action = action;
            UpdatePatchNodes();
        }
        
        /// <summary>
        /// Adds a new Entry to the action table.
        /// </summary>
        public void AddNewAction()
        {
            Data.ActionEntries.Add(new SfcActionEntity());
            // No need to call for an update here, as it propagates the data via UpdateActionTo.
        }
        
        /// <summary>
        /// Removes an entry at the given position.
        /// </summary>
        public void RemoveActionAt(int index)
        {
            Data.ActionEntries.RemoveAt(index);
            UpdatePatchNodes();
        }
        
        /// <summary>
        /// Removes all nodes from the patch field and prepares to be collected
        /// </summary>
        public void RemovePatch()
        {
            _control.ReferenceRect.RemoveChild(SfcPatchNode);
            SfcPatchNode.QueueFree();
            SfcPatchNode = null;
            Data = null;
            _control = null;
        }
        #endregion
    }
}