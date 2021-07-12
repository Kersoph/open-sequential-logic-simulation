using Godot;
using Osls.SfcEditor;
using System.Text;
using Osls.SfcSimulation.Viewer;


namespace Osls.Plants.ElectricalBarrier
{
    public class StartPassing : ViewportContainer
    {
        #region ==================== Fields / Properties ====================
        private bool _isExecutable;
        private Master _simulationMaster;
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
        public void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson, SfcEntity sfcEntity)
        {
            _simulation = GetNode<ElectricalBarrier>("Viewport/ElectricalBarrier");
            if (sfcEntity != null)
            {
                _simulation.InitialiseWith(mainNode, openedLesson);
                _simulationMaster = new Master(sfcEntity, _simulation);
                _isExecutable = _simulationMaster.IsProgramSimulationValid();
                _simulation.Barrier.SetAsOpened();
                _simulation.Vehicle.CarSpeed = 0;
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
                if (_simulatedSteps == 0) _simulation.Vehicle.PlaceCarAt(VehicleAgentController.PathCheckpointCollisionStart + 0.02f);
                _simulationMaster.UpdateSimulation(16);
                _simulatedSteps++;
                if (_simulatedSteps >= 600)
                {
                    CollectResults();
                }
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
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
            
            if (builder.Length == 0)
            {
                Result = 1;
                GetNode<Label>("Label").Text += "\nOk!";
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
