using Osls.SfcEditor;
using System.Collections.Generic;
using Osls.SfcEditor.Interpreters;


namespace Osls.SfcSimulation.Engine
{
    public class SfcProgramData
    {
        #region ==================== Fields / Properties ====================
        public IReadOnlyDictionary<int, SfcStep> ControlMap { get; private set; }
        public SfcEntity SfcEntity { get; private set; }
        public StepMaster StepMaster { get; private set; }
        
        public HashSet<SfcStep> SoonActiveSteps { get; private set; }
        public HashSet<SfcStep> ActiveSteps { get; private set; }
        public HashSet<SfcStep> SoonInactiveSteps { get; private set; }
        public HashSet<SfcStep> InactiveSteps { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public SfcProgramData(SfcEntity sfcEntity, ProgrammableLogicController pu)
        {
            SfcEntity = sfcEntity;
            StepMaster = new StepMaster();
            StepMaster.UpdateStepNames(sfcEntity);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// True if the step is contained in the active steps set.
        /// Used while checking the sfc transitions.
        /// </summary>
        public bool IsStepActive(SfcStep step)
        {
            return ActiveSteps.Contains(step);
        }
        
        public SfcStep GetStepFromMapKey(int key)
        {
            return ControlMap[key];
        }
        
        /// <summary>
        /// Returns true if the simulation steps can be executed
        /// </summary>
        public bool AreStepsValid()
        {
            foreach (SfcStep step in ControlMap.Values)
            {
                if (!step.IsStepValid()) return false;
            }
            return true;
        }
        
        public void InitializeSfcSteps(ProgrammableLogicController pu)
        {
            SoonActiveSteps = new HashSet<SfcStep>();
            ActiveSteps = new HashSet<SfcStep>();
            SoonInactiveSteps = new HashSet<SfcStep>();
            InactiveSteps = new HashSet<SfcStep>();
            
            var controlMap = new Dictionary<int, SfcStep>(SfcEntity.Patches.Count);
            foreach (PatchEntity entry in SfcEntity.Patches)
            {
                if (entry.ContainsRealStep())
                {
                    SfcStep step = new SfcStep(entry);
                    InactiveSteps.Add(step);
                    if (entry.SfcStepType == StepType.StartingStep)
                    {
                        SoonActiveSteps.Add(step);
                    }
                    controlMap.Add(entry.Key, step);
                }
            }
            ControlMap = controlMap;
            
            foreach (var step in InactiveSteps)
            {
                step.Initialise(pu);
            }
        }
        #endregion
    }
}