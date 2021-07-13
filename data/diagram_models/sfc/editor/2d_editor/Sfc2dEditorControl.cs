using Godot;
using System.Collections.Generic;
using System.Linq;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Control class to the Sfc2dEditorNode
    /// </summary>
    public class Sfc2dEditorControl
    {
        #region ==================== Fields Properties ====================
        private readonly Dictionary<int, SfcPatchControl> _controlMap = new Dictionary<int, SfcPatchControl>();
        private bool _isEditable;
        
        /// <summary>
        /// Returns the patch control map
        /// </summary>
        public IReadOnlyDictionary<int, SfcPatchControl> ControlMap { get { return _controlMap; } }
        
        /// <summary>
        /// Reference patch to hold the nodes
        /// </summary>
        public ReferenceRect ReferenceRect { get; private set; }
        
        /// <summary>
        /// Holds the processing data
        /// </summary>
        public ProcessingData Data { get; private set; }
        #endregion
        
        
        #region ==================== Public ====================
        public Sfc2dEditorControl(ReferenceRect referenceRect, ProcessingData data, bool isEditable)
        {
            ReferenceRect = referenceRect;
            Data = data;
            _isEditable = isEditable;
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
        /// Marks or unmarks the step with the given id
        /// </summary>
        public void MarkStep(int id, bool setMark)
        {
            _controlMap[id].SfcPatchNode.MarkStep(setMark);
        }
        
        /// <summary>
        /// Loads the file and builds the SFC diagram if the file exists
        /// Creates a default diagram if it could not be loaded
        /// </summary>
        public void LoadDiagramOrDefault(string filepath)
        {
            if (!System.IO.File.Exists(filepath))
            {
                if (Data.SfcEntity.Lookup(1, 0) == null)
                {
                    PatchEntity entity = new PatchEntity(1, 1)
                    {
                        SfcStepType = StepType.StartingStep
                    };
                    Data.SfcEntity.AddPatch(entity);
                }
            }
            else
            {
                Data.LoadData(filepath);
            }
            InitialiseFromData();
        }
        
        /// <summary>
        /// Saves the SFC diagram to a file
        /// </summary>
        public void SaveDiagram(string filepath)
        {
            Data.SaveData(filepath);
        }
        
        /// <summary>
        /// Forcefully applies all user edits to the data model.
        /// </summary>
        public void ApplyAllEdits()
        {
            foreach (SfcPatchControl patchControl in _controlMap.Values)
            {
                patchControl.SfcPatchNode.ApplyAllEdits();
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Loads the data from the stream. Written in "WriteTo".
        /// </summary>
        private void InitialiseFromData()
        {
            foreach (SfcPatchControl patch in _controlMap.Values)
            {
                patch.RemovePatch();
            }
            _controlMap.Clear();
            IReadOnlyCollection<PatchEntity> patches = Data.SfcEntity.Patches;
            foreach (PatchEntity entity in patches)
            {
                SfcPatchControl control = new SfcPatchControl(entity, this, !_isEditable);
                _controlMap.Add(entity.Key, control);
            }
            UpdateGrid();
        }
        
        /// <summary>
        /// Updates the whole logical data model.
        /// </summary>
        private void UpdateLogicalModel()
        {
            if (_isEditable)
            {
                // We want a copy because we will add empty forward patches to the dict
                PatchEntity[] patches = Data.SfcEntity.Patches.ToArray();
                foreach (PatchEntity patch in patches)
                {
                    if (patch.ContainsRelevantData())
                    {
                        EnsureNeighbours(patch);
                    }
                }
            }
            Data.StepMaster.UpdateStepNames(Data.SfcEntity);
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
            PatchEntity patch = Data.SfcEntity.Lookup(x, y);
            if (patch == null)
            {
                Data.SfcEntity.CreatePatchAt((short)x, (short)y);
                patch = Data.SfcEntity.Lookup(x, y);
                SfcPatchControl control = new SfcPatchControl(patch, this, _isEditable);
                _controlMap.Add(patch.Key, control);
            }
        }
        #endregion
    }
}