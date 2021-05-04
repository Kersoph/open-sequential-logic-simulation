using Osls.SfcEditor;


namespace Osls.Plants.ElectricalBarrier
{
    /// <summary>
    /// Minimal example class for a test viewer.
    /// </summary>
    public class LightsTest : TestPage
    {
        #region ==================== Fields / Properties ====================
        private enum Stages { ExecuteTests, DisplayResults };
        private Stages _stage = Stages.ExecuteTests;
        private LessonEntity _openedLesson;
        
        private bool _isExecutable;
        private Master _simulationMaster;
        private Lights _simulation;
        private int _simulatedSteps;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole test viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(MainNode mainNode, LessonEntity openedLesson)
        {
            _openedLesson = openedLesson;
            string filepath = openedLesson.FolderPath + "/User/Diagram.sfc";
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
            
            _simulation = GetNode<Lights>("Lights");
            if (sfcEntity != null)
            {
                _simulationMaster = new Master(sfcEntity, _simulation);
                _isExecutable = _simulationMaster.IsProgramSimulationValid();
            }
            else
            {
                _isExecutable = false;
            }
        }
        
        public override void _Process(float delta)
        {
            switch (_stage)
            {
                case Stages.ExecuteTests:
                    UpdateTests();
                    break;
                case Stages.DisplayResults:
                    break;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void UpdateTests()
        {
            if (_isExecutable)
            {
                _simulationMaster.UpdateSimulation(16);
                _simulatedSteps++;
                if (_simulatedSteps >= 1200)
                {
                    CreateResult();
                    _stage = Stages.DisplayResults;
                }
            }
        }
        
        private void CreateResult()
        {
        }
        #endregion
    }
}