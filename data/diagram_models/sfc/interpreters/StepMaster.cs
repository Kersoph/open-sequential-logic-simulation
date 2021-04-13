using System.Collections.Generic;

namespace Osls.SfcEditor.Interpreter
{
    public class StepMaster
    {
        #region ==================== Fields Properties ====================
        private readonly Dictionary<string, int> _patchNameMap = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _patchStepTimeMap = new Dictionary<string, int>();
        
        /// <summary>
        /// Contains the Step to patch id dictionary
        /// </summary>
        public IReadOnlyDictionary<string, int> PatchNameMap { get { return _patchNameMap; } }
        /// <summary>
        /// Contains the Step Time to patch id dictionary
        /// </summary>
        public IReadOnlyDictionary<string, int> PatchStepTimeMap { get { return _patchStepTimeMap; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// The step name must be unique to create step references
        /// </summary>
        public void UpdateStepNames(SfcEntity sfcEntity)
        {
            _patchNameMap.Clear();
            _patchStepTimeMap.Clear();
            foreach (PatchEntity entity in sfcEntity.Patches)
            {
                if (entity.ContainsRealStep())
                {
                    EnsureUniqueStepKey(entity);
                    int mapKey = entity.Key;
                    _patchNameMap.Add(entity.StepName, mapKey);
                    _patchStepTimeMap.Add(entity.StepName + ".T", mapKey);
                }
            }
        }
        
        /// <summary>
        /// Returns true if the name is a valid Step name
        /// </summary>
        public bool ContainsStep(string name)
        {
            return _patchNameMap.ContainsKey(name);
        }
        
        /// <summary>
        /// Returns the numeric key in the map.
        /// </summary>
        public int GetNameKey(string name)
        {
            return _patchNameMap[name];
        }
        
        /// <summary>
        /// Returns true if a STEP.T exists.
        /// </summary>
        public bool ContainsStepTime(string name)
        {
            return PatchStepTimeMap.ContainsKey(name);
        }
        
        /// <summary>
        /// Returns numeric key of the STEP.T.
        /// </summary>
        public int GetStepTimeKey(string name)
        {
            return PatchStepTimeMap[name];
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// The step name must be unique to create step references
        /// </summary>
        private void EnsureUniqueStepKey(PatchEntity data)
        {
            if (_patchNameMap.ContainsKey(data.StepName))
            {
                data.StepName += "?";
                EnsureUniqueStepKey(data);
            }
        }
        #endregion
    }
}