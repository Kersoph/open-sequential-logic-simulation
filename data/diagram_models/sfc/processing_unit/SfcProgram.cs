using Osls.SfcEditor;
using System.Collections.Generic;


namespace Osls.SfcSimulation.Engine
{
    public class SfcProgram
    {
        #region ==================== Fields / Properties ====================
        public ProgrammableLogicController Plc { get; private set; }
        public SfcProgramData Data { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public SfcProgram(ProgrammableLogicController plc, SfcProgramData data)
        {
            Plc = plc;
            Data = data;
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Is called to calculate the next steps of the simulation.
        /// </summary>
        public void UpdateProcess(int deltaTimeMs)
        {
            Data.InactiveSteps.UnionWith(Data.SoonInactiveSteps);
            Data.ActiveSteps.ExceptWith(Data.SoonInactiveSteps);
            Data.SoonInactiveSteps.Clear();
            foreach (var sfcStep in Data.SoonActiveSteps)
            {
                sfcStep.ExecuteActions(this, ActionQualifier.PPlus, deltaTimeMs);
            }
            Data.ActiveSteps.UnionWith(Data.SoonActiveSteps);
            Data.InactiveSteps.ExceptWith(Data.SoonActiveSteps);
            Data.SoonActiveSteps.Clear();
            
            foreach (var sfcStep in Data.ActiveSteps)
            {
                sfcStep.ExecuteActions(this, ActionQualifier.N, deltaTimeMs);
            }
            
            foreach (var sfcStep in Data.ActiveSteps)
            {
                sfcStep.CalculateTransition(this);
            }
            
            foreach (var sfcStep in Data.SoonInactiveSteps)
            {
                sfcStep.ExecuteActions(this, ActionQualifier.PMinus, deltaTimeMs);
            }
        }
        
        /// <summary>
        /// Called when a transition fires and a set needs to be updated
        /// </summary>
        public void UpdateStepStatus(List<SfcStep> toActive, List<SfcStep> toInactive)
        {
            Data.SoonActiveSteps.UnionWith(toActive);
            Data.SoonInactiveSteps.UnionWith(toInactive);
        }
        
        /// <summary>
        /// Returns true if the simulation can be executed
        /// </summary>
        public bool IsProgramLogicValid()
        {
            return Data.AreStepsValid();
        }
        #endregion
    }
}
