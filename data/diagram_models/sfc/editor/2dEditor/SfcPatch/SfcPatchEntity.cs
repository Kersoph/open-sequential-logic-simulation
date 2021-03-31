using System.Collections.Generic;

namespace SfcSandbox.Data.Model.SfcEditor
{
    /// <summary>
    /// Entity calss to store the informations for a SFC patch
    /// </summary>
    public class SfcPatchEntity
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// The logical x position in the grid
        /// </summary>
        public int X { get; private set; }
        /// <summary>
        /// The logical y position in the grid
        /// </summary>
        public int Y { get; private set; }
        /// <summary>
        /// The logical step type. "eg Square"
        /// </summary>
        public SfcStepType SfcStepType { get; set; }
        /// <summary>
        /// The displayed name of this step
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// The logical type of the upper branch.
        /// Either an OR branch or a parallel merge.
        /// </summary>
        public SfcBranchLineType UpperBranch { get; set; }
        /// <summary>
        /// The logical type of the lower branch.
        /// Either an OR merge or a parallel branch.
        /// </summary>
        public SfcBranchLineType LowerBranch { get; set; }
        /// <summary>
        /// The raw condition text when the transition should fire.
        /// It is interpreted and formed to a logical model. (Can be invaild)
        /// </summary>
        public string TransitionText { get; set; }
        /// <summary>
        /// Contains the action entries for the step in this patch.
        /// </summary>
        public List<SfcActionEntity> ActionEntries { get; private set; }
        #endregion;
        
        
        #region ==================== Public ====================
        public SfcPatchEntity(int x, int y)
        {
            X = x;
            Y = y;
            TransitionText = string.Empty;
            StepName = "StepName";
            ActionEntries = new List<SfcActionEntity>();
        }
        
        /// <summary>
        /// Returns true if any element is used in this patch.
        /// </summary>
        public bool ContainsRelevantData()
        {
            bool relevantStepType = SfcStepType != SfcStepType.Unused;
            bool relevantBranchType = UpperBranch != SfcBranchLineType.Unused || LowerBranch != SfcBranchLineType.Unused;
            return relevantStepType || relevantBranchType || ContainsTransition();
        }
        
        /// <summary>
        /// True if the transition is not empty.
        /// </summary>
        public bool ContainsTransition()
        {
            return !string.IsNullOrEmpty(TransitionText);
        }
        
        /// <summary>
        /// True, if this patch contains a real step (Step or StartingStep)
        /// and is not just connecting patches.
        /// </summary>
        public bool ContainsRealStep()
        {
            return SfcStepType == SfcStepType.Step || SfcStepType == SfcStepType.StartingStep;
        }
        #endregion
        
        
        #region ==================== Persistence ====================
        /// <summary>
        /// Loads the data from the stream. Written in "WriteTo".
        /// </summary>
        public static SfcPatchEntity CreateFrom(System.IO.BinaryReader reader)
        {
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            SfcPatchEntity entity = new SfcPatchEntity(x, y);
            entity.SfcStepType = (SfcStepType)reader.ReadInt32();
            entity.StepName = reader.ReadString();
            entity.UpperBranch = (SfcBranchLineType)reader.ReadInt32();
            entity.LowerBranch = (SfcBranchLineType)reader.ReadInt32();
            entity.TransitionText = reader.ReadString();
            int entryCount = reader.ReadInt32();
            for (int i = 0; i < entryCount; i++)
            {
                SfcActionEntity entry = SfcActionEntity.CreateFrom(reader);
                entity.ActionEntries.Add(entry);
            }
            return entity;
        }
        
        /// <summary>
        /// Writes the data from the stream. Read by "ReadFrom".
        /// </summary>
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write((int)SfcStepType);
            writer.Write(StepName);
            writer.Write((int)UpperBranch);
            writer.Write((int)LowerBranch);
            writer.Write(TransitionText);
            writer.Write(ActionEntries.Count);
            for (int i = 0; i < ActionEntries.Count; i++)
            {
                ActionEntries[i].WriteTo(writer);
            }
        }
        #endregion
    }
    
    public enum SfcStepType
    {
        /// <summary>
        /// This step cell is not used, except for bypassing branches to neighbouring cells.
        /// </summary>
        Unused,
        /// <summary>
        /// This step cell is skipped and bridging down the logic.
        /// </summary>
        Pass,
        /// <summary>
        /// This step cell is a standard step.
        /// </summary>
        Step,
        /// <summary>
        /// This step cell is a starting step, indicated with a double lined frame.
        /// </summary>
        StartingStep,
        /// <summary>
        /// This step cell is a jump, connectiong it to another cell.
        /// </summary>
        Jump
    }
    
    public enum SfcBranchLineType
    {
        /// <summary>
        /// This branch line is not used.
        /// </summary>
        Unused,
        /// <summary>
        /// This branch line is used as an OR branch/merge
        /// </summary>
        Single,
        /// <summary>
        /// This branch line is used as a aprallel branch/merge
        /// </summary>
        Double,
    }
}