using Godot;
using System.Collections.Generic;
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
        public enum TestStep { Deploy, Power, FirstOff, FirstOn, SecondOff, SecondOn, Both, Wait, Report, Done }
        private Dictionary<TestStep, TestCase> _testCases;
        private ILessonEntity _openedLesson;
        private bool _isExecutable;
        private int _testStepCounter;
        private TestStep _testStep;
        private bool _pressingBothOk;
        
        /// <summary>
        /// The CPU / actor in this simulation
        /// </summary>
        public Master SimulationMaster { get; private set; }
        
        /// <summary>
        /// The simulated plant
        /// </summary>
        public CircularSawModel Simulation { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole twat viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _openedLesson = openedLesson;
            Simulation = GetNode<CircularSawModel>("ViewportContainer/Viewport/CircularSawModel");
            string filepath = openedLesson.TemporaryDiagramFilePath;
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
            if (sfcEntity != null)
            {
                Simulation.InitialiseWith(mainNode, openedLesson);
                SimulationMaster = new Master(sfcEntity, Simulation);
                _isExecutable = SimulationMaster.IsProgramSimulationValid();
                SetupTestSteps();
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
                    case TestStep.FirstOff:
                    case TestStep.FirstOn:
                    case TestStep.SecondOff:
                    case TestStep.SecondOn:
                    case TestStep.Wait:
                        _testStep = _testCases[_testStep].Check(_testStep);
                        break;
                    case TestStep.Both:
                        TestStepBoth();
                        break;
                    case TestStep.Report:
                        TestStepReport();
                        break;
                }
                if (_isExecutable)
                {
                    SimulationMaster.UpdateSimulation(16);
                }
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void SetupTestSteps()
        {
            _testCases = new Dictionary<TestStep, TestCase>()
            {
                { TestStep.Power, new TestCase(this, false, false, false, "Panel/VBC/Power",
                "OK!", "Immediately turned on?!", TestStep.FirstOff, TestStep.FirstOff) },
                { TestStep.FirstOff, new TestCase(this, false, true, false, "Panel/VBC/FirstOff",
                "OK!", "Turned on with the OFF button?!", TestStep.FirstOn, TestStep.FirstOn) },
                { TestStep.FirstOn, new TestCase(this, true, false, true, "Panel/VBC/FirstOn",
                "OK!", "It is not turning on.", TestStep.SecondOff, TestStep.SecondOff) },
                { TestStep.SecondOff, new TestCase(this, false, true, false, "Panel/VBC/SecondOff",
                "OK.", "It is not turning off!", TestStep.SecondOn, TestStep.SecondOn) },
                { TestStep.SecondOn, new TestCase(this, true, false, true, "Panel/VBC/SecondOn",
                "OK.", "It is not turning on!", TestStep.Both, TestStep.Both) },
                { TestStep.Wait, new TestCase(this, false, true, false, "Panel/VBC/Wait",
                "Done", "The motor is still on...", TestStep.Report, TestStep.Report) },
            };
        }
        
        private void TestStepDeploy()
        {
            if (_isExecutable)
            {
                GetNode<Label>("Panel/VBC/Deploy").Text += "OK!";
                _testStep = TestStep.Power;
            }
            else
            {
                GetNode<Label>("Panel/VBC/Deploy").Text += "FAILED: SFC error.";
                _testStep = TestStep.Report;
            }
        }
        
        private void TestStepBoth()
        {
            if (_testStepCounter < 5)
            {
                Simulation.ONButtonState = true;
                Simulation.OFFButtonState = true;
                _testStepCounter++;
            }
            else if (_testStepCounter < 60)
            {
                Simulation.ONButtonState = true;
                Simulation.OFFButtonState = true;
                if (Simulation.CircularSawNode.MotorSwitchedOn)
                {
                    Simulation.ONButtonState = false;
                    Simulation.OFFButtonState = false;
                    GetNode<Label>("Panel/VBC/Both").Text += "It is not reset dominant!";
                    _pressingBothOk = false;
                    _testStep = TestStep.Wait;
                }
                _testStepCounter++;
            }
            else
            {
                Simulation.ONButtonState = false;
                Simulation.OFFButtonState = false;
                _testStepCounter = 0;
                GetNode<Label>("Panel/VBC/Both").Text += "Good";
                _pressingBothOk = true;
                _testStep = TestStep.Wait;
            }
        }
        
        private void TestStepReport()
        {
            if (HadHeavyErrors())
            {
                GetNode<Label>("Panel/VBC/Report").Text = "0 Stars";
                _openedLesson.SetAndSaveStars(0);
            }
            else if (!_pressingBothOk)
            {
                GetNode<Label>("Panel/VBC/Report").Text = "2 Stars";
                _openedLesson.SetAndSaveStars(2);
            }
            else
            {
                GetNode<Label>("Panel/VBC/Report").Text = "3 Stars! Nice";
                _openedLesson.SetAndSaveStars(3);
            }
            _testStep = TestStep.Done;
        }
        
        private bool HadHeavyErrors()
        {
            if (!_isExecutable) return true;
            foreach (var testCase in _testCases)
            {
                if (testCase.Value.HadError) return true;
            }
            return false;
        }
        #endregion
    }
}
