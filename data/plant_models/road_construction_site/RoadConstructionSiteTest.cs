using Godot;
using System.Collections.Generic;
using Osls.SfcEditor;
using Osls.SfcSimulation.Viewer;


namespace Osls.Plants.RoadConstructionSite
{
    /// <summary>
    /// Topmost node for the test viewer scene
    /// </summary>
    public class RoadConstructionSiteTest : TestPage
    {
        #region ==================== Fields / Properties ====================
        private const int SimulatedCycleTime = 16;
        
        private enum TestState { Simulate, CalculateResults, Done };
        private ILessonEntity _openedLesson;
        private TestState _testState;
        private float _lambdaState = 0.01f;
        private bool _isExecutable;
        private IDiagramSimulationMaster _simulationMaster;
        private RoadConstructionSite _simulation;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole twat viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _openedLesson = openedLesson;
            _simulation = GetNode<RoadConstructionSite>("PlantViewportContainer/PlantViewport/RoadConstructionSite");
            string filepath = _openedLesson.TemporaryDiagramFilePath;
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
            if (sfcEntity != null)
            {
                _simulation.InitialiseWith(mainNode, openedLesson);
                _simulationMaster = new Master(sfcEntity, _simulation);
                _isExecutable = _simulationMaster.IsProgramSimulationValid();
            }
            if (sfcEntity == null || !_isExecutable)
            {
                _isExecutable = false;
                _testState = TestState.Done;
                _openedLesson.SetAndSaveStars(0);
            }
            SpawnTimeGenerator.ResetGenerator();
        }
        
        public override void _Process(float delta)
        {
            int simulationSteps = LookupTargetSimulationCycles();
            for (int i = 0; i < simulationSteps; i++)
            {
                switch (_testState)
                {
                    case TestState.Simulate:
                        Simulate();
                        break;
                    case TestState.CalculateResults:
                        CalculateResults();
                        break;
                }
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void Simulate()
        {
            SpawnTimeGenerator.SetLambda(_lambdaState);
            for (int i = 0; i < 300; i++)
            {
                _simulationMaster.UpdateSimulation(SimulatedCycleTime);
            }
            _lambdaState += 0.0002f;
            if (_lambdaState >= 0.1)
            {
                _testState = TestState.CalculateResults;
            }
        }
        
        /// <summary>
        /// The result calculation excepts _lambdaState [0.01, 0.0002, 0.1] with 300 simulation cycles per step.
        /// </summary>
        /// <remarks>
        /// One Line Blocked: 21000 av; 2157900 max
        /// Ok Timing: 7568 av; 12600 max
        /// Ok Sensor: 6032 av; 12600 max
        /// </remarks>
        private void CalculateResults()
        {
            _testState = TestState.Done;
            List<DynamicCarReport> reports = _simulation.CollectReports();
            ResultCollector results = new ResultCollector(reports, SimulatedCycleTime);
            string headline = GetHeadline(results);
            string infoText = GetInfoText(results);
            string quoteText = GetNewsQuote(results);
            int stars = CalculateStars(results);
            _openedLesson.SetAndSaveStars(stars);
            string starQuote = GetStarText(stars);
            GetNode<Newspaper>("Newspaper").SetTexts(headline, infoText, quoteText, starQuote);
            GetNode<Viewport>("PlantViewportContainer/PlantViewport").RenderTargetUpdateMode = Viewport.UpdateMode.Once;
        }
        
        private string GetHeadline(ResultCollector results)
        {
            if (results.HadAnAccident > 1)
            {
                return results.HadAnAccident + " Deaths Due To Faulty Control";
            }
            if (results.HadAnAccident == 1)
            {
                return "A Death Due To Faulty Control";
            }
            if (results.AverageWaitingMs > 10000 || results.MaximumWaitingCycles > 2000)
            {
                return "Annoyance About Horrific Waiting Times";
            }
            if (results.AverageWaitingMs > 7500 || results.MaximumWaitingCycles > 1000)
            {
                return "Annoyance About Long Waiting Times";
            }
            if (results.AverageWaitingMs > 6000)
            {
                return "Ongoing Discussions About Traffic Light";
            }
            return "Local Engineer Receives Award";
        }
        
        private string GetInfoText(ResultCollector results)
        {
            string averageSeconds = ((float)results.AverageWaitingMs * 0.001f).ToString("F1");
            string maximumSeconds = (results.MaximumWaitingCycles * SimulatedCycleTime * 0.001).ToString("F1");
            return "The average waiting time is " + averageSeconds + "s.\n\n"
            + "Some had to wait up to " + maximumSeconds + "s.";
        }
        
        private string GetNewsQuote(ResultCollector results)
        {
            if (results.HadAnAccident > 0)
            {
                return "\"It should never have been put into operation.\"";
            }
            if (results.AverageWaitingMs > 18000)
            {
                return "\"It's Like One Side Is Always Red!\"";
            }
            if (results.AverageWaitingMs > 8500)
            {
                return "\"Any Snail Is Faster!\"";
            }
            if (results.AverageWaitingMs > 7500)
            {
                return "\"I Could Do It Better\"";
            }
            if (results.AverageWaitingMs > 6000)
            {
                return "\"Waiting when there is absolutely no one around...\"";
            }
            return "\"My Most Favourite Signal Light!\"";
        }
        
        private int CalculateStars(ResultCollector results)
        {
            if (results.MaximumWaitingCycles > 100000 || results.HadAnAccident > 0) return 0;
            if (results.AverageWaitingMs > 7400|| results.MaximumWaitingCycles > 1500) return 1;
            if (results.AverageWaitingMs > 6000 || results.MaximumWaitingCycles > 790) return 2;
            return 3;
        }
        
        private string GetStarText(int stars)
        {
            return "\"" + stars + " Star Experience\"\n- Marco Burri";
        }
        #endregion
    }
}
