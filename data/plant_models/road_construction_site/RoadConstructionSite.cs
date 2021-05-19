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
        private const string _topTrafficLightKey = "H100";
        private TrafficControlSystem _botTrafficLight;
        private const string _botTrafficLightKey = "H101";
        private TrafficController _trafficController;
        private const string _topTrafficSensorKey = "B100";
        private const string _botTrafficSensorKey = "B101";
        #endregion
        
        
        #region ==================== Public Methods ====================
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
        
        /// <summary>
        /// Returns the collected reports for the simulated cars
        /// </summary>
        public List<DynamicCarReport> CollectReports()
        {
            return _trafficController.CollectReports();
        }
        
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public override void SetupUi()
        {
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Returns the input definition for the simulation
        /// </summary>
        protected override StateTable DefineInputs()
        {
            return new StateTable(
                new List<StateEntry<bool>>()
                {
                    { new StateEntry<bool>(_topTrafficLightKey, false, "Switches the upper traffic light", "Turns the upper lane light to green\nif the value is set to true.\nOtherwise it will be red.") },
                    { new StateEntry<bool>(_botTrafficLightKey, false, "Switches the lower traffic light", "Turns the lower lane light to green\nif the value is set to true.\nOtherwise it will be red.") }
                },
                new List<StateEntry<int>>()
            );
        }
        
        /// <summary>
        /// Returns the output definition for the simulation
        /// </summary>
        protected override StateTable DefineOutput()
        {
            return new StateTable(
                new List<StateEntry<bool>>()
                {
                    { new StateEntry<bool>(_topTrafficSensorKey, false, "Upper lane proximity sensor", "True if a car is close to the sensor") },
                    { new StateEntry<bool>(_botTrafficSensorKey, false, "Lower lane proximity sensor", "True if a car is close to the sensor") }
                },
                new List<StateEntry<int>>()
            );
        }
        
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
        #endregion
    }
}