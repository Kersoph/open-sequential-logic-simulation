using System.Collections.Generic;


namespace Osls.Plants.RoadConstructionSite
{
    /// <summary>
    /// Boundary class between the simulation and the SFC Controllers
    /// </summary>
    public class RoadConstructionSite : SimulationPage
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
        protected override StateTable DefineInputs()
        {
            return new StateTable(
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
        protected override StateTable DefineOutput()
        {
            return new StateTable(
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
            _topTrafficLight = GetNode<TrafficControlSystem>("Signalisation/TrafficControlSystemTop");
            _topTrafficLight.SetToGreen(SimulationInput.PollBoolean(_topTrafficLightKey));
            _botTrafficLight = GetNode<TrafficControlSystem>("Signalisation/TrafficControlSystemBot");
            _botTrafficLight.SetToGreen(SimulationInput.PollBoolean(_botTrafficLightKey));
            PathController topPath = GetNode<PathController>("DynamicCars/TopPath");
            topPath.Setup(_topTrafficLight);
            PathController botPath = GetNode<PathController>("DynamicCars/BotPath");
            botPath.Setup(_botTrafficLight);
            _trafficController = new TrafficController(topPath, botPath);
            SpawnTimeGenerator.ResetGenerator();
        }
        #endregion
        
        
        #region ==================== Public ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int timeMs)
        {
            _topTrafficLight.SetToGreen(SimulationInput.PollBoolean(_topTrafficLightKey));
            _botTrafficLight.SetToGreen(SimulationInput.PollBoolean(_botTrafficLightKey));
            _trafficController.CalculateNextStep(timeMs);
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