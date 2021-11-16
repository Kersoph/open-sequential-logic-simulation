using Godot;
using Osls.SfcEditor;
using Osls.SfcSimulation.Viewer;


namespace Osls.Plants.CircularSaw
{
    /// <summary>
    /// Minimal example class for a test viewer.
    /// </summary>
    public class CircularSawTest : TestPage
    {
        #region ==================== Fields / Properties ====================
        private bool _isExecutable;
        private Master _simulationMaster;
        private CircularSawModel _simulation;
        private enum TestStep { Deploy, Power, FirstOff, FirstOn, SecondOff, SecondOn, Both, End, Report }
        private TestStep _testStep;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole twat viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _simulation = GetNode<CircularSawModel>("ViewportContainer/Viewport/CircularSawModel");
            string filepath = openedLesson.TemporaryDiagramFilePath;
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
            if (sfcEntity != null)
            {
                _simulation.InitialiseWith(mainNode, openedLesson);
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
            int simulationSteps = LookupTargetSimulationCycles();
            for (int i = 0; i < simulationSteps; i++)
            {
                switch (_testStep)
                {
                    case TestStep.Deploy:
                        TestStepDeploy();
                        break;
                    case TestStep.Power:
                        TestStepPower();
                        break;
                    case TestStep.FirstOff:
                        TestStepFirstOff();
                        break;
                    case TestStep.FirstOn:
                        TestStepFirstOn();
                        break;
                    case TestStep.SecondOff:
                        TestStepSecondOff();
                        break;
                    case TestStep.SecondOn:
                        TestStepSecondOn();
                        break;
                    case TestStep.Both:
                        TestStepBoth();
                        break;
                    case TestStep.End:
                        TestStepEnd();
                        break;
                    case TestStep.Report:
                        TestStepReport();
                        break;
                }
                if (_isExecutable)
                {
                    _simulationMaster.UpdateSimulation(16);
                }
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void TestStepDeploy()
        {
            if (_isExecutable)
            {
                _testStep = TestStep.Report;
            }
            else
            {
                GetNode<Label>("Panel/VBC/Deploy").Text += "FAILED: SFC error.";
                _testStep = TestStep.Power;
            }
            
        }
        
        private void TestStepPower()
        {
        }
        
        private void TestStepFirstOff()
        {
        }
        
        private void TestStepFirstOn()
        {
        }
        
        private void TestStepSecondOff()
        {
        }
        
        private void TestStepSecondOn()
        {
        }
        
        private void TestStepBoth()
        {
        }
        
        private void TestStepEnd()
        {
        }
        
        private void TestStepReport()
        {
        }
        #endregion
    }
}
