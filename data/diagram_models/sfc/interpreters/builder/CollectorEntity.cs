using System.Collections.Generic;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.Engine.Builder
{
    public class CollectorEntity
    {
        #region ==================== Fields Properties ====================
        public SfcProgrammData Data { get; }
        public List<int> CollectedSteps { get; }
        public BranchType TargetType { get; }
        public bool UpperBranch { get; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates a new collextor entity to hold the data
        /// </summary>
        /// <param name="data">program reference</param>
        /// <param name="targetType">whoch branch type to search</param>
        /// <param name="upperBranch">true for upper, false for lower</param>
        public CollectorEntity(SfcProgrammData data, BranchType targetType, bool upperBranch)
        {
            Data = data;
            CollectedSteps = new List<int>();
            TargetType = targetType;
            UpperBranch = upperBranch;
        }
        #endregion
    }
}