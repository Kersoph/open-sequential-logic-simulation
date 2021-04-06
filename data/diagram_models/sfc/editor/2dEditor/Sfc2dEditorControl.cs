using Godot;
using System.Collections.Generic;
using System.Linq;
using Osls.SfcEditor.Interpreter;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Control class to the Sfc2dEditorNode
    /// </summary>
    public class Sfc2dEditorControl
    {
        #region ==================== Fields Properties ====================
        public ReferenceRect ReferenceRect { get; private set; }
        protected SfcEntity Data { get; private set; }
        public StepMaster StepMaster { get; private set; }
        private readonly Dictionary<int, SfcPatchControl> _controlMap = new Dictionary<int, SfcPatchControl>();
        #endregion
        
        
        #region ==================== Public ====================
        public Sfc2dEditorControl(ReferenceRect referenceRect)
        {
            ReferenceRect = referenceRect;
            Data = new SfcEntity();
            StepMaster = new StepMaster();
        }
        
        /// <summary>
        /// Recalculates the whole model and updates the visible nodes.
        /// </summary>
        public void UpdateGrid()
        {
            UpdateLogicalModel();
            foreach (SfcPatchControl patchControl in _controlMap.Values)
            {
                patchControl.UpdatePatchNodes();
            }
        }
        
        /// <summary>
        /// Clones the control map into a new dicionary.
        /// </summary>
        public SfcEntity LinkSfcData()
        {
            return Data; // todo: Read from file?
        }
        
        /// <summary>
        /// Marks or unmarks the step with the given id
        /// </summary>
        public void MarkStep(int id, bool setMark)
        {
            _controlMap[id].SfcPatchNode.MarkStep(setMark);
        }
        
        /// <summary>
        /// Loads the data from the stream. Written in "WriteTo".
        /// </summary>
        public void ReadFrom(System.IO.BinaryReader reader)
        {
            Data.ReadFrom(reader);
            foreach (SfcPatchControl patch in _controlMap.Values)
            {
                patch.RemovePatch();
            }
            _controlMap.Clear();
            IReadOnlyCollection<PatchEntity> patches = Data.Patches;
            foreach (PatchEntity entity in patches)
            {
                SfcPatchControl control = new SfcPatchControl(entity, this);
                _controlMap.Add(entity.Key, control);
            }
            UpdateGrid();
        }
        
        /// <summary>
        /// Writes the data from the stream. Read by "ReadFrom".
        /// </summary>
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            Data.WriteTo(writer);
        }
        
        /// <summary>
        /// Creates a starting step at 1, 0.
        /// </summary>
        public void CreateDefaultDiagram()
        {
            PatchEntity entity = new PatchEntity(1, 0)
            {
                SfcStepType = StepType.StartingStep
            };
            Data.AddPatch(entity);
            SfcPatchControl control = new SfcPatchControl(entity, this);
            _controlMap.Add(entity.Key, control);
            UpdateGrid();
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Updates the whole logical data model.
        /// </summary>
        private void UpdateLogicalModel()
        {
            // We want a copy because we will add empty forward patches to the dict
            PatchEntity[] patches = Data.Patches.ToArray();
            foreach (PatchEntity patch in patches)
            {
                if (patch.ContainsRelevantData())
                {
                    EnsureNeighbours(patch);
                }
            }
            StepMaster.UpdateStepNames(Data);
        }
        
        /// <summary>
        /// Ensures that a relevant patch has at least an empty patch on each side.
        /// </summary>
        private void EnsureNeighbours(PatchEntity patchData)
        {
            EnsurePatchAt(patchData.X + 1, patchData.Y);
            EnsurePatchAt(patchData.X, patchData.Y + 1);
            EnsurePatchAt(patchData.X - 1, patchData.Y);
            EnsurePatchAt(patchData.X, patchData.Y - 1);
        }
        
        /// <summary>
        /// Ensures that a patch field is aviable at the given position.
        /// </summary>
        private void EnsurePatchAt(int x, int y)
        {
            PatchEntity patch = Data.Lookup(x, y);
            if (patch == null)
            {
                Data.CreatePatchAt((short)x, (short)y);
                patch = Data.Lookup(x, y);
                SfcPatchControl control = new SfcPatchControl(patch, this);
                _controlMap.Add(patch.Key, control);
            }
        }
        #endregion
    }
}