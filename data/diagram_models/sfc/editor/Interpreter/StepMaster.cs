using System.Collections.Generic;

namespace Osls.SfcEditor.Interpreter
{
    public static class StepMaster
    {
        #region ==================== Fields Properties ====================
        private static readonly Dictionary<string, int> _patchNameMap = new Dictionary<string, int>();
        private static readonly Dictionary<string, int> _patchStepTimeMap = new Dictionary<string, int>();
        
        /// <summary>
        /// Contains the Step to patch id dictionary
        /// </summary>
        public static Dictionary<string, int> PatchNameMap { get { return _patchNameMap; } }
        /// <summary>
        /// Contains the Step Time to patch id dictionary
        /// </summary>
        public static Dictionary<string, int> PatchStepTimeMap { get { return _patchStepTimeMap; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// The step name must be unique to create step references
        /// </summary>
        public static void UpdateStepNames(SfcPatchControl[] controls)
        {
            _patchNameMap.Clear();
            _patchStepTimeMap.Clear();
            foreach (SfcPatchControl patchControl in controls)
            {
                PatchEntity data = patchControl.Data;
                if (data.ContainsRealStep())
                {
                    EnsureUniqueStepKey(data);
                    int mapKey = data.Key;
                    _patchNameMap.Add(data.StepName, mapKey);
                    _patchStepTimeMap.Add(data.StepName + ".T", mapKey);
                }
            }
        }
        
        /// <summary>
        /// Returns true if the name is a valid Step name
        /// </summary>
        public static bool ContainsStep(string name)
        {
            return _patchNameMap.ContainsKey(name);
        }
        
        /// <summary>
        /// Returns the numeric key in the map.
        /// </summary>
        public static int GetNameKey(string name)
        {
            return _patchNameMap[name];
        }
        
        /// <summary>
        /// Returns true if a STEP.T exists.
        /// </summary>
        public static bool ContainsStepTime(string name)
        {
            return PatchStepTimeMap.ContainsKey(name);
        }
        
        /// <summary>
        /// Returns true if a STEP.T exists.
        /// </summary>
        public static int GetStepTimeKey(string name)
        {
            return PatchStepTimeMap[name];
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// The step name must be unique to create step references
        /// </summary>
        private static void EnsureUniqueStepKey(PatchEntity data)
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