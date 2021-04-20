using System.Collections.Generic;


namespace Osls.SfcEditor.Interpreters
{
    public class StepMaster
    {
        #region ==================== Fields Properties ====================
        private readonly Dictionary<string, int> _patchNameMap = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _patchStepTimeMap = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _patchStepStateMap = new Dictionary<string, int>();
        
        /// <summary>
        /// Contains the Step to patch id dictionary
        /// </summary>
        public IReadOnlyDictionary<string, int> PatchNameMap { get { return _patchNameMap; } }
        /// <summary>
        /// Contains the key to the Step Time
        /// </summary>
        public IReadOnlyDictionary<string, int> PatchStepTimeMap { get { return _patchStepTimeMap; } }
        /// <summary>
        /// Contains the key to the Step State
        /// </summary>
        public IReadOnlyDictionary<string, int> PatchStepStateMap { get { return _patchStepStateMap; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// The step name must be unique to create step references
        /// </summary>
        public void UpdateStepNames(SfcEntity sfcEntity)
        {
            _patchNameMap.Clear();
            _patchStepTimeMap.Clear();
            _patchStepStateMap.Clear();
            foreach (PatchEntity entity in sfcEntity.Patches)
            {
                if (entity.ContainsRealStep())
                {
                    EnsureUniqueStepKey(entity);
                    int mapKey = entity.Key;
                    _patchNameMap.Add(entity.StepName, mapKey);
                    _patchStepTimeMap.Add(entity.StepName + ".T", mapKey);
                    _patchStepStateMap.Add(entity.StepName + ".Q", mapKey);
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
        /// Returns true if a intern variable/entity exists with the given name. For example STEP.T in SFC.
        /// </summary>
        public bool ContainsInternalNumeric(string name)
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
        
        /// <summary>
        /// Returns true if a intern variable/entity exists with the given name. For example STEP.Q in SFC.
        /// </summary>
        public bool ContainsInternalBoolean(string name)
        {
            return PatchStepStateMap.ContainsKey(name);
        }
        
        /// <summary>
        /// Returns numeric key of the STEP.Q.
        /// </summary>
        public int GetStepStateKey(string name)
        {
            return PatchStepStateMap[name];
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