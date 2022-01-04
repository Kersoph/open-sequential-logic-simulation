using Osls.SfcEditor;
using System.Collections.Generic;


namespace Osls.SfcSimulation.Engine
{
    public class SfcProgram
    {
        #region ==================== Fields / Properties ====================
        public ProgrammableLogicController Plc { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public SfcProgram(ProgrammableLogicController plc)
        {
            Plc = plc;
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Is called to calculate the next steps of the simulation.
        /// </summary>
        public void UpdateProcess(int deltaTimeMs)
        {
            Plc.SfcProgramData.InactiveSteps.UnionWith(Plc.SfcProgramData.SoonInactiveSteps);
            Plc.SfcProgramData.ActiveSteps.ExceptWith(Plc.SfcProgramData.SoonInactiveSteps);
            Plc.SfcProgramData.SoonInactiveSteps.Clear();
            foreach (var sfcStep in Plc.SfcProgramData.SoonActiveSteps)
            {
                sfcStep.ExecuteActions(this, ActionQualifier.PPlus, deltaTimeMs);
            }
            Plc.SfcProgramData.ActiveSteps.UnionWith(Plc.SfcProgramData.SoonActiveSteps);
            Plc.SfcProgramData.InactiveSteps.ExceptWith(Plc.SfcProgramData.SoonActiveSteps);
            Plc.SfcProgramData.SoonActiveSteps.Clear();
            
            foreach (var sfcStep in Plc.SfcProgramData.ActiveSteps)
            {
                sfcStep.ExecuteActions(this, ActionQualifier.N, deltaTimeMs);
            }
            
            foreach (var sfcStep in Plc.SfcProgramData.ActiveSteps)
            {
                sfcStep.CalculateTransition(this);
            }
            
            foreach (var sfcStep in Plc.SfcProgramData.SoonInactiveSteps)
            {
                sfcStep.ExecuteActions(this, ActionQualifier.PMinus, deltaTimeMs);
            }
        }
        
        /// <summary>
        /// Called when a transition fires and a set needs to be updated
        /// </summary>
        public void UpdateStepStatus(List<SfcStep> toActive, List<SfcStep> toInactive)
        {
            Plc.SfcProgramData.SoonActiveSteps.UnionWith(toActive);
            Plc.SfcProgramData.SoonInactiveSteps.UnionWith(toInactive);
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsProgramLogicValid()
        {
            return Plc.SfcProgramData.AreStepsValid();
        }
        #endregion
    }
}
