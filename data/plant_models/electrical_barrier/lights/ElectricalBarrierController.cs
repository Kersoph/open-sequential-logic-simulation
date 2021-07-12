using Godot;


namespace Osls.Plants.ElectricalBarrier
{
    public class ElectricalBarrierController : Node
    {
        #region ==================== Fields / Properties ====================
        private ElectricalBarrier _simulation;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the controller.
        /// </summary>
        public void Initialise()
        {
            _simulation = GetNode<ElectricalBarrier>("../ElectricalBarrier");
        }
        
        /// <summary>
        /// Updates the step and collects the results
        /// </summary>
        public void UpdateStep(int deltaTime)
        {
            _simulation.UpdateModel(deltaTime);
            ControlBarrier();
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Controls the barrier behaviour. Also possible to load and run a sfc solution
        /// </summary>
        public void ControlBarrier()
        {
            if (_simulation.SimulationOutput.PollBoolean(GuardAgent.OpenGateSwitchKey))
            {
                if (!_simulation.SimulationOutput.PollBoolean(ElectricalBarrierNode.SensorOpenedKey))
                {
                    _simulation.SimulationInput.SetValue(ElectricalBarrierNode.MotorKey, 1);
                }
                else
                {
                    _simulation.SimulationInput.SetValue(ElectricalBarrierNode.MotorKey, 0);
                }
            }
            else
            {
                if (!_simulation.SimulationOutput.PollBoolean(ElectricalBarrierNode.SensorClosedKey))
                {
                    _simulation.SimulationInput.SetValue(ElectricalBarrierNode.MotorKey, -1);
                }
                else
                {
                    _simulation.SimulationInput.SetValue(ElectricalBarrierNode.MotorKey, 0);
                }
            }
        }
        #endregion
    }
}
