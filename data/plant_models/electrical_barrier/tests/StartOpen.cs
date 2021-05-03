using Godot;
using Osls.SfcEditor;
using System.Text;


namespace Osls.Plants.ElectricalBarrier
{
    public class StartOpen : ViewportContainer
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
        public void InitialiseWith(SfcEntity sfcEntity)
        {
            _simulation = GetNode<ElectricalBarrier>("Viewport/ElectricalBarrier");
            if (sfcEntity != null)
            {
                _simulationMaster = new Master(sfcEntity, _simulation);
                _isExecutable = _simulationMaster.IsProgramSimulationValid();
                _simulation.Guard.AllowVehiclePass = false;
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
                _simulationMaster.UpdateSimulation(16);
                _simulatedSteps++;
                if (_simulatedSteps == 300) _simulation.Vehicle.CarSpeed = VehicleAgentController.RegularCarSpeed;
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
            if (_simulation.Vehicle.TimesPassedTrack > 0)
            {
                builder.AppendLine("At least one invalid car could pass");
            }
            
            if (builder.Length == 0)
            {
                Result = 1;
                GetNode<Label>("Label").Text = "Ok!";
            }
            else
            {
                Result = 0;
                GetNode<Label>("Label").Text = builder.ToString();
            }
        }
        #endregion
    }
}
