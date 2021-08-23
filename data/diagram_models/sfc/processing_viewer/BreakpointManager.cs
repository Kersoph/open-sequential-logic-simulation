using System.Collections.Generic;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.Viewer
{
    public class BreakpointManager
    {
        #region ==================== Fields / Properties ====================
        private readonly Master _master;
        
        /// <summary>
        /// Links the hash set with the active breakpoints
        /// </summary>
        public HashSet<int> CheckSet { get; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public BreakpointManager(Master master, Sfc2dEditorNode _sfc2dEditorNode)
        {
            CheckSet = new HashSet<int>();
            _master = master;
            IReadOnlyDictionary<int, SfcPatchControl> map = _sfc2dEditorNode.Sfc2dEditorControl.ControlMap;
            foreach (var entity in map)
            {
                if (entity.Value.Data.ContainsRealStep())
                {
                    BreakpointOverlayButton.CreateAndSetup(entity.Key, entity.Value.SfcPatchNode, this);
                }
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Returns true if one breakpoint was hit.
        /// (When a step gets active)
        /// </summary>
        public bool CheckBreakpointHit()
        {
            return _master.HasSoonActiveSteps(CheckSet);
        }
        #endregion
    }
}
