using Godot;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Control class for a SFC patch
    /// </summary>
    public class SfcPatchControl
    {
        #region ==================== Fields / Properties ====================
        private const string _patchReferencePath = "res://data/diagram_models/sfc/editor/2d_editor/sfc_patch/SfcPatch.tscn";
        
        /// <summary>
        /// The editor master holding all patch controls
        /// </summary>
        public Sfc2dEditorControl Master { get; private set; }
        
        /// <summary>
        /// The patch model entity.
        /// </summary>
        public PatchEntity Data { get; private set; }
        
        /// <summary>
        /// The connected node controlled by this class
        /// </summary>
        public SfcPatchNode SfcPatchNode { get; private set; }
        
        /// <summary>
        /// Contains metadata to visualise the SFC
        /// </summary>
        public SfcPatchMeta SfcPatchMeta { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Constructor. It will add the patch to the 2d editor.
        /// </summary>
        public SfcPatchControl(PatchEntity data, Sfc2dEditorControl control)
        {
            Data = data;
            Master = control;
            SfcPatchMeta = new SfcPatchMeta();
            Node node = ((PackedScene)GD.Load(_patchReferencePath)).Instance();
            SfcPatchNode = (SfcPatchNode)node;
            SfcPatchNode.InitializeWith(this, data);
            control.ReferenceRect.AddChild(SfcPatchNode);
        }
        
        /// <summary>
        /// Updates the visible nodes according to the data model.
        /// </summary>
        public void UpdatePatchNodes()
        {
            SfcPatchMeta.UpdatePatch(this);
            SfcPatchNode.UpdateNodes(Data);
        }
        
        /// <summary>
        /// Called when the user changes the sfc step name.
        /// </summary>
        public void UpdateNameTo(string name)
        {
            if (Data.StepName != name)
            {
                Data.StepName = name;
                Master.UpdateGrid();
            }
        }
        
        /// <summary>
        /// Called if the user toggled the top branch/merge line.
        /// This will update the model.
        /// </summary>
        public void TopLineToggled()
        {
            if (Data.UpperBranch == BranchType.Unused)
            {
                Data.UpperBranch = BranchType.Single;
            }
            else if (Data.UpperBranch == BranchType.Single)
            {
                Data.UpperBranch = BranchType.Double;
            }
            else
            {
                Data.UpperBranch = BranchType.Unused;
            }
            UpdatePatchNodes();
        }
        
        /// <summary>
        /// Called if the user toggled the bottom branch/merge line.
        /// This will update the model.
        /// </summary>
        public void BotLineToggled()
        {
            if (Data.LowerBranch == BranchType.Unused)
            {
                Data.LowerBranch = BranchType.Single;
            }
            else if (Data.LowerBranch == BranchType.Single)
            {
                Data.LowerBranch = BranchType.Double;
            }
            else
            {
                Data.LowerBranch = BranchType.Unused;
            }
            UpdatePatchNodes();
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
        public void UpdateSfcStepTo(StepType sfcStepType)
        {
            Data.SfcStepType = sfcStepType;
            Master.UpdateGrid();
        }
        
        /// <summary>
        /// Called if the user changed an action entry.
        /// This will update the model.
        /// </summary>
        public void UpdateActionTo(int index, ActionQualifier qualifier, string action)
        {
            if (Data.ActionEntries[index].Qualifier != qualifier || Data.ActionEntries[index].Action != action)
            {
                Data.ActionEntries[index].Qualifier = qualifier;
                Data.ActionEntries[index].Action = action;
                UpdatePatchNodes();
            }
        }
        
        /// <summary>
        /// Adds a new Entry to the action table.
        /// </summary>
        public void AddNewAction()
        {
            Data.ActionEntries.Add(new ActionEntity());
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
            Master.ReferenceRect.RemoveChild(SfcPatchNode);
            SfcPatchNode.QueueFree();
            SfcPatchNode = null;
            Data = null;
            Master = null;
        }
        #endregion
    }
}