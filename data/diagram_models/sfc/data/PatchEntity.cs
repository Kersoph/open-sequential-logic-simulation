using System.Collections.Generic;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Entity class to store the informations for a SFC patch
    /// </summary>
    public class PatchEntity
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// The logical x position in the grid
        /// </summary>
        public short X { get; private set; }
        
        /// <summary>
        /// The logical y position in the grid
        /// </summary>
        public short Y { get; private set; }
        
        /// <summary>
        /// returns an unique key for this patch
        /// </summary>
        public int Key { get { return SfcEntity.CalculateMapKey(X, Y); } }
        
        /// <summary>
        /// The logical step type. "eg Square"
        /// </summary>
        public StepType SfcStepType { get; set; }
        
        /// <summary>
        /// The displayed name of this step
        /// </summary>
        public string StepName { get; set; }
        
        /// <summary>
        /// The logical type of the upper branch.
        /// </summary>
        public BranchType UpperBranch { get; set; }
        
        /// <summary>
        /// The logical type of the lower branch.
        /// </summary>
        public BranchType LowerBranch { get; set; }
        
        /// <summary>
        /// The raw condition text when the transition should fire.
        /// It is interpreted and formed to a logical model (Can be invaild).
        /// </summary>
        public string TransitionText { get; set; }
        
        /// <summary>
        /// Contains the action entries for the step in this patch.
        /// </summary>
        public List<ActionEntity> ActionEntries { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public PatchEntity(short x, short y)
        {
            X = x;
            Y = y;
            TransitionText = string.Empty;
            StepName = "StepName";
            ActionEntries = new List<ActionEntity>();
        }
        
        /// <summary>
        /// Returns true if any element is used in this patch.
        /// </summary>
        public bool ContainsRelevantData()
        {
            bool relevantStepType = SfcStepType != StepType.Unused;
            bool relevantBranchType = UpperBranch != BranchType.Unused || LowerBranch != BranchType.Unused;
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
            return SfcStepType == StepType.Step || SfcStepType == StepType.StartingStep;
        }
        
        /// <summary>
        /// Loads the data from the stream. Written in "WriteTo".
        /// </summary>
        public static PatchEntity CreateFrom(System.IO.BinaryReader reader)
        {
            short x = reader.ReadInt16();
            short y = reader.ReadInt16();
            PatchEntity entity = new PatchEntity(x, y)
            {
                SfcStepType = (StepType)reader.ReadInt32(),
                StepName = reader.ReadString(),
                UpperBranch = (BranchType)reader.ReadInt32(),
                LowerBranch = (BranchType)reader.ReadInt32(),
                TransitionText = reader.ReadString()
            };
            int entryCount = reader.ReadInt32();
            for (int i = 0; i < entryCount; i++)
            {
                ActionEntity entry = ActionEntity.CreateFrom(reader);
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
}