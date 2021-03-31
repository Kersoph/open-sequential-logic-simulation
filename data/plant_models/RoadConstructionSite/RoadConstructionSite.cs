using System.Collections.Generic;

namespace Osls.SfcSimulation.PlantModels.RoadConstructionSite
{
    /// <summary>
    /// Boundary class between the simulation and the SFC Controllers
    /// </summary>
    public class RoadConstructionSite : SimulationControlNode
    {
        #region ==================== Fields / Properties ====================
        private TrafficControlSystem _topTrafficLight;
        private const string _topTrafficLightKey = "H_Top";
        private TrafficControlSystem _botTrafficLight;
        private const string _botTrafficLightKey = "H_Bot";
        private TrafficController _trafficController;
        private const string _topTrafficSensorKey = "B_Top";
        private const string _botTrafficSensorKey = "B_Bot";
        #endregion
        
        
        #region ==================== Initializing ====================
        /// <summary>
        /// Returns the input definition for the simulation
        /// </summary>
        protected override InputTable DefineInputs()
        {
            return new InputTable(
                new Dictionary<string, bool>()
                {
                    {_topTrafficLightKey, false},
                    {_botTrafficLightKey, false}
                },
                new Dictionary<string, int>()
            );
        }
        
        /// <summary>
        /// Returns the output definition for the simulation
        /// </summary>
        protected override OutputTable DefineOutput()
        {
            return new OutputTable(
                new Dictionary<string, bool>()
                {
                    {_topTrafficSensorKey, false},
                    {_botTrafficSensorKey, false}
                },
                new Dictionary<string, int>()
            );
        }
        
        public override void _Ready()
        {
            _topTrafficLight = this.GetNode<TrafficControlSystem>("Signalisation/TrafficControlSystemTop");
            _topTrafficLight.SetToGreen(SimulationInput.PollBooleanInput(_topTrafficLightKey));
            _botTrafficLight = this.GetNode<TrafficControlSystem>("Signalisation/TrafficControlSystemBot");
            _botTrafficLight.SetToGreen(SimulationInput.PollBooleanInput(_botTrafficLightKey));
            PathController topPath = GetNode<PathController>("DynamicCars/TopPath");
            topPath.Setup(_topTrafficLight);
            PathController botPath = GetNode<PathController>("DynamicCars/BotPath");
            botPath.Setup(_botTrafficLight);
            _trafficController = new TrafficController(topPath, botPath);
        }
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Claculate sthe next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep()
        {
            _topTrafficLight.SetToGreen(SimulationInput.PollBooleanInput(_topTrafficLightKey));
            _botTrafficLight.SetToGreen(SimulationInput.PollBooleanInput(_botTrafficLightKey));
            _trafficController.CalculateNextStep();
            SimulationOutput.SetValue(_topTrafficSensorKey, _topTrafficLight.IsCarInRange());
            SimulationOutput.SetValue(_botTrafficSensorKey, _botTrafficLight.IsCarInRange());
        }
        
        /// <summary>
        /// Returns the collected Reports for the simulated cars
        /// </summary>
        public List<DynamicCarReport> CollectReports()
        {
            return _trafficController.CollectReports();
        }
        #endregion
    }
}