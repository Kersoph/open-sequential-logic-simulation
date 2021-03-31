using System.Collections.Generic;
using SfcSandbox.Data.Model.SfcSimulation.Engine;


namespace SfcSandbox.Data.Model.SfcSimulation.PlantModels.RoadConstructionSite
{
    /// <summary>
    /// Topmost node for the test viewer scene
    /// </summary>
    public class RoadConstructionSiteTest : SfcTestViewer
    {
        #region ==================== Public ====================
        private TestState _testState;
        private enum TestState {Simulate, CalculateResults, Done};
        private float _lambdaState = 0.01f;
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Initializes the whole viewer. Called bevore the node is added to the tree by the lesson controller.
        /// </summary>
        public override bool InitializeEditor(LessonEntity opendLesson)
        {
            bool result = base.InitializeEditor(opendLesson);
            GetNode<Godot.Viewport>("PlantViewportContainer/PlantViewport").AddChild(LoadedSimulationNode);
            GetNode<Godot.Viewport>("PlantViewportContainer/PlantViewport").SetUpdateMode(Godot.Viewport.UpdateMode.WhenVisible);
            SpawnTimeGenerator.ResetGenerator();
            return result;
        }
        
        public override void _Process(float delta)
        {
            if(!IsExecutable) return;
            switch(_testState)
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
            for(int i = 0; i < 300; i++)
            {
                SimulationMaster.UpdateSimulation();
            }
            _lambdaState += 0.0002f;
            if(_lambdaState >= 0.1)
            {
                _testState = TestState.CalculateResults;
            }
        }
        
        /// <summary>
        /// The result calculation excepts _lambdaState [0.01, 0.0002, 0.1] with 300 simulation cycles per step.
        /// </summary>
        private void CalculateResults()
        {
            _testState = TestState.Done;
            RoadConstructionSite simulation = (RoadConstructionSite)SimulationMaster.SimulationControlNode;
            List<DynamicCarReport> reports = simulation.CollectReports();
            
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
            float averageTime = (float)totalWaitingCycles / (float)completedSimulations;
            string headline = GetHeadline(hadAnAccident, averageTime);
            string infoText = GetInfoText(averageTime, maximumWaitingCycles);
            string quoteText = GetNewsQuote(hadAnAccident, averageTime);
            int stars = CalculateStars(hadAnAccident, averageTime, maximumWaitingCycles, completedSimulations);
            OpendLesson.SetAndSaveStars(stars);
            string starQuote = GetStarText(stars);
            GetNode<Godot.Viewport>("PlantViewportContainer/PlantViewport").SetUpdateMode(Godot.Viewport.UpdateMode.Once);
            GetNode<Newspaper>("Newspaper").SetTexts(headline, infoText, quoteText, starQuote);
        }
        
        private string GetHeadline(int hadAnAccident, float waitingTime)
        {
            if (hadAnAccident > 1)
            {
                return hadAnAccident + " Deaths Due To Faulty Control";
            }
            if (hadAnAccident == 1)
            {
                return "A Death Due To Faulty Control";
            }
            if (waitingTime > 600)
            {
                return "Annoyance About Horrific Waiting Times";
            }
            if (waitingTime > 400)
            {
                return "Annoyance About Long Waiting Times";
            }
            if (waitingTime > 330)
            {
                return "Ongoing Disscusions About Traffic Light";
            }
            return "Local Engineer Receives Award";
        }
        
        private string GetInfoText(float averageWaitingTime, int maximumWaitingCycles)
        {
            string averageSeconds = (averageWaitingTime * Master.StepUpdateTime).ToString("F1");
            string maximumSeconds = (maximumWaitingCycles * Master.StepUpdateTime).ToString("F1");
            return "The average waiting time is " + averageSeconds + "s.\n"
            + "Some had to wait up to " + maximumSeconds + "s.";
        }
        
        private string GetNewsQuote(int hadAnAccident, float waitingTime)
        {
            if (hadAnAccident > 0)
            {
                return "\"It should never have been put into operation.\"";
            }
            if (waitingTime > 1000)
            {
                return "\"It's Like One Side Is Always Red!\"";
            }
            if (waitingTime > 600)
            {
                return "\"Any Snail Is Faster!\"";
            }
            if (waitingTime > 400)
            {
                return "\"I Could Do It Better\"";
            }
            if (waitingTime > 330)
            {
                return "\"It's Not That Bad\"";
            }
            return "\"My Most Favourite Signal Light!\"";
        }
        
        private int CalculateStars(int hadAnAccident, float averageWaitingCycles, int maximumWaitingCycles, int completedSimulations)
        {
            if(maximumWaitingCycles > 700 || hadAnAccident > 0) return 0;
            if(averageWaitingCycles > 380) return 1;
            if(averageWaitingCycles > 320 || maximumWaitingCycles > 650 || completedSimulations < 590) return 2;
            return 3;
        }
        
        private string GetStarText(int stars)
        {
            return "\"" + stars + " Star Experience\"\n- Marco Burri";
        }
        #endregion
    }
}