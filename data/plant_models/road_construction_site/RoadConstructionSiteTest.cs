using Godot;
using System.Collections.Generic;
using Osls.SfcEditor;


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
        private LessonEntity _openedLesson;
        private TestState _testState;
        private float _lambdaState = 0.01f;
        private bool _isExecutable;
        private Master _simulationMaster;
        private RoadConstructionSite _simulation;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole twat viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(MainNode mainNode, LessonEntity openedLesson)
        {
            _openedLesson = openedLesson;
            _simulation = GetNode<RoadConstructionSite>("PlantViewportContainer/PlantViewport/RoadConstructionSite");
            string filepath = openedLesson.FolderPath + "/User/Diagram.sfc";
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
            if (sfcEntity != null)
            {
                _simulationMaster = new Master(sfcEntity, _simulation);
                _isExecutable = _simulationMaster.IsProgramSimulationValid();
            }
            else
            {
                _isExecutable = false;
            }
            SpawnTimeGenerator.ResetGenerator();
        }
        
        public override void _Process(float delta)
        {
            if (!_isExecutable) return;
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
            
            int hadAnAccident = 0;
            int minimumWaitingCycles = 999999;
            ulong totalWaitingCycles = 0;
            int maximumWaitingCycles = 0;
            int completedSimulations = 0;
            
            for (int i = 0; i < reports.Count; i++)
            {
                if (reports[i].HadAnAccident)
                {
                    hadAnAccident++;
                }
                else
                {
                    int cycles = reports[i].WaitingCycles;
                    if(maximumWaitingCycles < cycles) maximumWaitingCycles = cycles;
                    if (reports[i].SimulationCompleted && minimumWaitingCycles > cycles)
                    {
                        minimumWaitingCycles = cycles;
                    }
                    totalWaitingCycles = checked(totalWaitingCycles + (ulong)cycles);
                    completedSimulations++;
                }
            }
            int averageWaitingMs = (int)(totalWaitingCycles / (ulong)completedSimulations) * SimulatedCycleTime;
            string headline = GetHeadline(hadAnAccident, averageWaitingMs);
            string infoText = GetInfoText(averageWaitingMs, maximumWaitingCycles);
            string quoteText = GetNewsQuote(hadAnAccident, averageWaitingMs);
            int stars = CalculateStars(hadAnAccident, averageWaitingMs, maximumWaitingCycles);
            _openedLesson.SetAndSaveStars(stars);
            string starQuote = GetStarText(stars);
            GetNode<Newspaper>("Newspaper").SetTexts(headline, infoText, quoteText, starQuote);
            GetNode<Viewport>("PlantViewportContainer/PlantViewport").RenderTargetUpdateMode = Viewport.UpdateMode.Once;
        }
        
        private string GetHeadline(int hadAnAccident, int averageWaitingMs)
        {
            if (hadAnAccident > 1)
            {
                return hadAnAccident + " Deaths Due To Faulty Control";
            }
            if (hadAnAccident == 1)
            {
                return "A Death Due To Faulty Control";
            }
            if (averageWaitingMs > 10000)
            {
                return "Annoyance About Horrific Waiting Times";
            }
            if (averageWaitingMs > 7500)
            {
                return "Annoyance About Long Waiting Times";
            }
            if (averageWaitingMs > 6000)
            {
                return "Ongoing Discussions About Traffic Light";
            }
            return "Local Engineer Receives Award";
        }
        
        private string GetInfoText(int averageWaitingMs, int maximumWaitingCycles)
        {
            string averageSeconds = ((float)averageWaitingMs * 0.001f).ToString("F1");
            string maximumSeconds = (maximumWaitingCycles * SimulatedCycleTime * 0.001).ToString("F1");
            return "The average waiting time is " + averageSeconds + "s.\n\n"
            + "Some had to wait up to " + maximumSeconds + "s.";
        }
        
        private string GetNewsQuote(int hadAnAccident, int averageWaitingMs)
        {
            if (hadAnAccident > 0)
            {
                return "\"It should never have been put into operation.\"";
            }
            if (averageWaitingMs > 18000)
            {
                return "\"It's Like One Side Is Always Red!\"";
            }
            if (averageWaitingMs > 8500)
            {
                return "\"Any Snail Is Faster!\"";
            }
            if (averageWaitingMs > 7500)
            {
                return "\"I Could Do It Better\"";
            }
            if (averageWaitingMs > 6000)
            {
                return "\"It's Not That Bad\"";
            }
            return "\"My Most Favourite Signal Light!\"";
        }
        
        private int CalculateStars(int hadAnAccident, float averageWaitingCycles, int maximumWaitingCycles)
        {
            if (maximumWaitingCycles > 100000 || hadAnAccident > 0) return 0;
            if (averageWaitingCycles > 7400) return 1;
            if (averageWaitingCycles > 6000 || maximumWaitingCycles > 790) return 2;
            return 3;
        }
        
        private string GetStarText(int stars)
        {
            return "\"" + stars + " Star Experience\"\n- Marco Burri";
        }
        #endregion
    }
}