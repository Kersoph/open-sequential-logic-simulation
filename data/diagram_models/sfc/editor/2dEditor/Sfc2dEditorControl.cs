using Godot;
using System.Collections.Generic;
using System.Linq;
using SfcSandbox.Data.Model.SfcEditor.Interpreter;

namespace SfcSandbox.Data.Model.SfcEditor
{
    /// <summary>
    /// Control class to the Sfc2dEditorNode
    /// </summary>
    public class Sfc2dEditorControl
    {
        #region ==================== Fields Properties ====================
        public const int XKeyShift = 16;
        public ReferenceRect ReferenceRect { get; private set; }
        
        private readonly Dictionary<int, SfcPatchControl> _patchControlMap;
        #endregion
        
        
        #region ==================== Public ====================
        public Sfc2dEditorControl(ReferenceRect referenceRect)
        {
            ReferenceRect = referenceRect;
            _patchControlMap = new Dictionary<int, SfcPatchControl>();
        }
        
        /// <summary>
        /// Creates a new patch at the given position and adds it to the map.
        /// </summary>
        public void CreatePatchAt(int x, int y)
        {
            SfcPatchEntity entity = new SfcPatchEntity(x, y);
            CreatePatchAt(entity);
        }
        
        /// <summary>
        /// Creates a new patch at the given position and adds it to the map.
        /// </summary>
        public void CreatePatchAt(SfcPatchEntity entity)
        {
            SfcPatchControl control = new SfcPatchControl(entity, this);
            AddPatchControlToMap(control);
        }
        
        /// <summary>
        /// Recalculates the whole model and updates the visible nodes.
        /// </summary>
        public void UpdateGrid()
        {
            UpdateLogicalModel();
            foreach (SfcPatchControl patchControl in _patchControlMap.Values)
            {
                patchControl.UpdatePatchNodes();
            }
        }
        
        /// <summary>
        /// Clones the control map into a new dicionary.
        /// </summary>
        public Dictionary<int, SfcPatchControl> CloneControlMap()
        {
            return new Dictionary<int, SfcPatchControl>(_patchControlMap);
        }
        
        /// <summary>
        /// Calculates the control map key from the given position.
        /// </summary>
        public static int CalculateMapKey(int x, int y)
        {
            return checked((x << XKeyShift) + y);
        }
        #endregion
        
        
        #region ==================== Private ====================
        private void AddPatchControlToMap(SfcPatchControl control)
        {
            int key = CalculateMapKey(control.Data);
            _patchControlMap.Add(key, control);
        }
        
        private int CalculateMapKey(SfcPatchEntity entity)
        {
            return CalculateMapKey(entity.X, entity.Y);
        }
        #endregion
        
        
        #region ==================== Update Logical Model ====================
        /// <summary>
        /// Updates the whole logical data model.
        /// </summary>
        private void UpdateLogicalModel()
        {
            // We want a copy because we will add empty forward patches to the dict
            SfcPatchControl[] controls = _patchControlMap.Values.ToArray();
            foreach (SfcPatchControl patchControl in controls)
            {
                if (patchControl.Data.ContainsRelevantData())
                {
                    EnsureNeighbours(patchControl.Data);
                    if (patchControl.Data.UpperBranch != SfcBranchLineType.Unused)
                    {
                        CalculateUpperBranchType(patchControl.Data);
                    }
                    if (patchControl.Data.LowerBranch != SfcBranchLineType.Unused)
                    {
                        CalculateLowerBranchType(patchControl.Data);
                    }
                }
            }
            StepMaster.UpdateStepNames(controls);
        }
        
        /// <summary>
        /// Ensures that a relevant patch has at least an empty patch on each side.
        /// </summary>
        private void EnsureNeighbours(SfcPatchEntity patchData)
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
            int mapKey = CalculateMapKey(x, y);
            if (!_patchControlMap.ContainsKey(mapKey))
            {
                CreatePatchAt(x, y);
            }
        }
        
        #region Upper Branch
        /// <summary>
        /// To determinate the type of the branch we will see if a parallel type is necessary with:
        /// - More than one upper step is connected to this or a neighbouring branch
        /// Otherwise we will set an OR branch
        /// </summary>
        private void CalculateUpperBranchType(SfcPatchEntity patchData)
        {
            int stepInfluencers = CountUpperStepInfluencers(patchData.X, patchData.Y);
            if (stepInfluencers > 1)
            {
                patchData.UpperBranch = SfcBranchLineType.Double;
            }
            else
            {
                patchData.UpperBranch = SfcBranchLineType.Single;
            }
        }
        
        /// <summary>
        /// Traverses all connected steps from left to right and counts the relevant upper steps.
        /// </summary>
        private int CountUpperStepInfluencers(int x, int y)
        {
            bool branchesLeft = true;
            int observedX = CalculateUpperRootNode(x, y);
            int numberOfBranches = 0;
            while (branchesLeft)
            {
                SfcPatchControl controler;
                if (!_patchControlMap.TryGetValue(CalculateMapKey(observedX, y), out controler))
                {
                    GD.Print("Reached side in branched/merged paths");
                    break;
                }
                if (HasInfluencingUpperStep(controler.Data))
                {
                    numberOfBranches++;
                }
                if (controler.Data.UpperBranch != SfcBranchLineType.Unused)
                {
                    observedX++;
                }
                else
                {
                    branchesLeft = false;
                }
            }
            return numberOfBranches;
        }
        
        /// <summary>
        /// The most left relevant node is the root node.
        /// </summary>
        private int CalculateUpperRootNode(int x, int y)
        {
            int observedX = x - 1;
            while (true)
            {
                SfcPatchControl controler;
                _patchControlMap.TryGetValue(CalculateMapKey(observedX, y), out controler);
                if (controler == null || controler.Data.UpperBranch == SfcBranchLineType.Unused)
                {
                    return observedX + 1;
                }
                observedX --;
            }
        }
        
        /// <summary>
        /// Returns true if the given patch field has an influencing upper sfc step to the branch/merge.
        /// </summary>
        private bool HasInfluencingUpperStep(SfcPatchEntity patchData)
        {
            return patchData.SfcStepType == SfcStepType.Pass
                || patchData.SfcStepType == SfcStepType.Step
                || patchData.SfcStepType == SfcStepType.StartingStep;
        }
        #endregion
        
        
        #region Lower Branch
        /// <summary>
        /// To determinate the type of the branch we will see if a parallel type is necessary with:
        /// - More than one lower step is connected to this or a neighbouring branch
        /// Otherwise we will do an OR merge
        /// </summary>
        private void CalculateLowerBranchType(SfcPatchEntity patchData)
        {
            int stepInfluencers = CountLowerStepInfluencers(patchData.X, patchData.Y);
            if (stepInfluencers > 1)
            {
                patchData.LowerBranch = SfcBranchLineType.Double;
            }
            else
            {
                patchData.LowerBranch = SfcBranchLineType.Single;
            }
        }
        
        /// <summary>
        /// Traverses all connected steps from left to right and counts the relevant lower steps.
        /// </summary>
        private int CountLowerStepInfluencers(int x, int y)
        {
            bool branchesLeft = true;
            int observedX = CalculateLowerRootNode(x, y);
            int numberOfBranches = 0;
            while (branchesLeft)
            {
                SfcPatchControl controler;
                if (!_patchControlMap.TryGetValue(CalculateMapKey(observedX, y), out controler))
                {
                    GD.Print("Reached side in branched/merged paths");
                    break;
                }
                SfcPatchControl lowerControler;
                if (_patchControlMap.TryGetValue(CalculateMapKey(observedX, y + 1), out lowerControler)
                    && HasInfluencingLowerStep(lowerControler.Data))
                {
                    numberOfBranches++;
                }
                if (controler.Data.LowerBranch != SfcBranchLineType.Unused)
                {
                    observedX++;
                }
                else
                {
                    branchesLeft = false;
                }
            }
            return numberOfBranches;
        }
        
        /// <summary>
        /// The most left relevant node is the root node.
        /// </summary>
        private int CalculateLowerRootNode(int x, int y)
        {
            int observedX = x - 1;
            while (true)
            {
                SfcPatchControl controler;
                _patchControlMap.TryGetValue(CalculateMapKey(observedX, y), out controler);
                if (controler == null || controler.Data.LowerBranch == SfcBranchLineType.Unused)
                {
                    return observedX + 1;
                }
                observedX --;
            }
        }
        
        /// <summary>
        /// Returns true if the given patch field has an influencing lower sfc step to the branch/merge.
        /// </summary>
        private bool HasInfluencingLowerStep(SfcPatchEntity patchData)
        {
            return patchData.SfcStepType == SfcStepType.Pass
                || patchData.SfcStepType == SfcStepType.Step
                || patchData.SfcStepType == SfcStepType.StartingStep
                || patchData.SfcStepType == SfcStepType.Jump;
        }
        #endregion
        #endregion
        
        
        #region ==================== Persistence ====================
        /// <summary>
        /// Loads the data from the stream. Written in "WriteTo".
        /// </summary>
        public void ReadFrom(System.IO.BinaryReader reader)
        {
            ResetPatch();
            PersistenceCheckHelper.CheckSectionNumber(reader, 0x11111111);
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                SfcPatchEntity entity = SfcPatchEntity.CreateFrom(reader);
                CreatePatchAt(entity);
            }
            UpdateGrid();
        }
        
        /// <summary>
        /// Removes all patches in the editor
        /// </summary>
        private void ResetPatch()
        {
            foreach (SfcPatchControl control in _patchControlMap.Values)
            {
                control.RemovePatch();
            }
            _patchControlMap.Clear();
        }
        
        /// <summary>
        /// Writes the data from the stream. Read by "ReadFrom".
        /// </summary>
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write(0x11111111);
            writer.Write(_patchControlMap.Values.Count);
            foreach (SfcPatchControl control in _patchControlMap.Values)
            {
                control.Data.WriteTo(writer);
            }
        }
        #endregion
    }
}