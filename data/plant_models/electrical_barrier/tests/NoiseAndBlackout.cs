using Godot;
using Osls.SfcEditor;
using System.Text;
using Osls.SfcSimulation.Viewer;


namespace Osls.Plants.ElectricalBarrier
{
    public class NoiseAndBlackout : ViewportContainer
    {
        #region ==================== Fields / Properties ====================
        private bool _isExecutable;
        private IDiagramSimulationMaster _simulationMaster;
        private ElectricalBarrier _simulation;
        private int _simulatedSteps;
        
        /// <summary>
        /// Returns a score from 0 (bad) to 1 (good) for this test. -1 if it is not done yet
        /// </summary>
        public int Result { get; private set; } = -1;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole test viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _simulation = GetNode<ElectricalBarrier>("Viewport/ElectricalBarrier");
            _simulationMaster = DiagramSimulationLoader.LoadTemp(openedLesson, _simulation);
            if (_simulationMaster != null)
            {
                _simulation.InitialiseWith(mainNode, openedLesson);
                _isExecutable = _simulationMaster.IsProgramSimulationValid();
            }
            else
            {
                _isExecutable = false;
                Result = 0;
                GetNode<Label>("Label").Text = "Program can not be executed";
            }
        }
        
        /// <summary>
        /// Updates the step and collects the results
        /// </summary>
        public void UpdateStep()
        {
            if (_isExecutable && Result == -1)
            {
                for (int i = 0; i < 5; i++)
                {
                    SimulateStep();
                }
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void SimulateStep()
        {
            if ((_simulatedSteps % 70) == 0)
            {
                _simulation.SimulationOutput.SetValue(GuardAgent.OpenGateSwitchKey, false);
            }
            if ((_simulatedSteps % 99) == 0)
            {
                _simulationMaster.Reset();
            }
            _simulationMaster.UpdateSimulation(50);
            if (_simulation.Vehicle.TimesPassedTrack == 5) _simulation.Guard.AllowVehiclePass = false;
            _simulatedSteps++;
            if (_simulatedSteps >= 600 * 5)
            {
                CollectResults();
            }
        }
        
        public void CollectResults()
        {
            StringBuilder builder = new StringBuilder();
            if (_simulation.Barrier.IsBroken)
            {
                builder.AppendLine("The barrier broke down");
            }
            if (_simulation.Vehicle.Damaged)
            {
                builder.AppendLine("The car got scratched");
            }
            if (_simulation.Vehicle.TimesPassedTrack == 0)
            {
                builder.AppendLine("No car could pass");
            }
            else if (_simulation.Vehicle.TimesPassedTrack < 5)
            {
                builder.AppendLine("Not enough cars could pass");
            }
            if (_simulation.Vehicle.TimesPassedTrack > 5)
            {
                builder.AppendLine("At least one invalid car could pass");
            }
            if (_simulation.Vehicle.WaitingTime > 6000)
            {
                builder.AppendLine("Customers complain about the behavior of the barrier.");
            }
            
            if (builder.Length == 0)
            {
                Result = 1;
                GetNode<Label>("Label").Text += "\nPassed!";
            }
            else
            {
                Result = 0;
                GetNode<Label>("Label").Text += "\n" + builder.ToString();
            }
        }
        #endregion
    }
}
