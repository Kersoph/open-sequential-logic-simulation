using WAT;
using System.Collections.Generic;
using Osls.SfcEditor;
using Osls.SfcSimulation.Engine;
using Tests.Core;


namespace Tests.SfcEditor.Interpreters
{
    public class SfcTestHelper
    {
        #region ==================== Fields Properties ====================
        private readonly ProgrammableLogicController _controller;
        #endregion
        
        
        #region ==================== Constructor ====================
        public SfcTestHelper(List<PatchEntity> stepEntities, Test testContext)
        {
            SfcEntity sfcEntity = new SfcEntity();
            foreach (PatchEntity step in stepEntities)
            {
                sfcEntity.AddPatch(step);
            }
            SimulationPageMock simulationPageMock = new SimulationPageMock();
            _controller = new ProgrammableLogicController(simulationPageMock, sfcEntity);
            _controller.Startup();
            testContext.Assert.IsTrue(_controller.IsLogicValid(), "ProgrammableLogicController->IsLogicValid");
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Returns true if the given step is in the active list
        /// (Remember: Soon active objects are not in the active list)
        /// </summary>
        public bool IsStepActive(int x, int y)
        {
            return _controller.SfcProgramData.IsStepActive(SfcEntity.CalculateMapKey(x, y));
        }
        
        public void UpdateStep()
        {
            _controller.Update(20);
        }
        #endregion
    }
}