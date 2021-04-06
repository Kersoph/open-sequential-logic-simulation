using Osls.SfcEditor;
using System.Collections.Generic;


namespace Osls.SfcSimulation.Engine
{
    public class SfcProgramm
    {
        #region ==================== Fields / Properties ====================
        public ProgrammableLogicController Plc { get; private set; }
        
        private readonly Dictionary<int, SfcPatchControl> _visualControlMap;
        private readonly Dictionary<int, SfcStep> _controlMap;
        
        private HashSet<SfcStep> _soonActiveSteps;
        private HashSet<SfcStep> _activeSteps;
        private HashSet<SfcStep> _soonInactiveSteps;
        private HashSet<SfcStep> _inactiveSteps;
        #endregion


        #region ==================== Constructor ====================
        public SfcProgramm(ProgrammableLogicController plc)
        {
            Plc = plc;
            _visualControlMap = plc.Master.Sfc2dEditorControl.CloneControlMap();
            _controlMap = new Dictionary<int, SfcStep>(_visualControlMap.Count);
            InitializeSfcSteps();
        }
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Is called to calculate the next steps of the simulation.
        /// </summary>
        public void UpdateProcess()
        {
            _inactiveSteps.UnionWith(_soonInactiveSteps);
            _activeSteps.ExceptWith(_soonInactiveSteps);
            _soonInactiveSteps.Clear();
            foreach (var SfcStep in _soonActiveSteps)
            {
                SfcStep.ExecuteActions(ActionQualifier.PPlus);
            }
            _activeSteps.UnionWith(_soonActiveSteps);
            _inactiveSteps.ExceptWith(_soonActiveSteps);
            _soonActiveSteps.Clear();
            
            foreach (var SfcStep in _activeSteps)
            {
                SfcStep.ExecuteActions(ActionQualifier.N);
            }
            
            foreach (var SfcStep in _activeSteps)
            {
                SfcStep.CalculateTransition();
            }
            
            foreach (var SfcStep in _soonInactiveSteps)
            {
                SfcStep.ExecuteActions(ActionQualifier.PMinus);
            }
        }
        
        /// <summary>
        /// True if the step is contained in the active steps set.
        /// Used while checking the sfc transitions.
        /// </summary>
        public bool IsStepActive(SfcStep step)
        {
            return _activeSteps.Contains(step);
        }
        
        /// <summary>
        /// Called when a transition fires and a set needs to be updated
        /// </summary>
        public void UpdateStepStatus(List<SfcStep> toActive, List<SfcStep> toInactive)
        {
            _soonActiveSteps.UnionWith(toActive);
            _soonInactiveSteps.UnionWith(toInactive);
        }
        
        /// <summary>
        /// Visualises the achtive or inactive status of the step.
        /// </summary>
        public void VisualiseStatus()
        {
            foreach (var SfcStep in _activeSteps)
            {
                _visualControlMap[SfcStep.Id].SfcPatchNode.MarkStep(true);
            }
            foreach (var SfcStep in _inactiveSteps)
            {
                 _visualControlMap[SfcStep.Id].SfcPatchNode.MarkStep(false);
            }
        }
        
        public SfcStep GetStepFromMapKey(int key)
        {
            return _controlMap[key];
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsProgrammLogicValid()
        {
            foreach(SfcStep step in _controlMap.Values)
            {
                if(!step.IsStepValid()) return false;
            }
            return true;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void InitializeSfcSteps()
        {
            _soonActiveSteps = new HashSet<SfcStep>();
            _activeSteps = new HashSet<SfcStep>();
            _soonInactiveSteps = new HashSet<SfcStep>();
            _inactiveSteps = new HashSet<SfcStep>();
            
            foreach (var entry in _visualControlMap)
            {
                SfcStep step = new SfcStep(entry.Value.Data, this, entry.Key);
                _inactiveSteps.Add(step);
                _controlMap.Add(entry.Key, step);
            }
            foreach (var step in _inactiveSteps)
            {
                if (step.SourceReference.ContainsRealStep())
                {
                    step.InitializeTransitions(_controlMap);
                    if (step.SourceReference.SfcStepType == StepType.StartingStep)
                    {
                        _soonActiveSteps.Add(step);
                    }
                }
            }
        }
        #endregion
    }
}