using System.Collections.Generic;


namespace Osls.Plants.ElectricalBarrier
{
    /// <summary>
    /// Boundary class between the simulation and the SFC Controllers
    /// </summary>
    public class Lights : SimulationPage
    {
        #region ==================== Fields / Properties ====================
        private const int PressDuration = 300;
        private const int ReleaseDuration = 1700;
        private const string ButtonKey = "S101";
        private const string LightsKey = "H102";
        private enum State { Idle, Pressing, Releasing }
        private State _state;
        private int _pressTime;
        
        /// <summary>
        /// Links the simulated barrier node
        /// </summary>
        public ElectricalBarrier Subscene { get { return GetNode<ElectricalBarrier>("ElectricalBarrier"); } }
        
        /// <summary>
        /// Links the simulated barrier node
        /// </summary>
        public ElectricalBarrierController Subcontroller { get { return GetNode<ElectricalBarrierController>("ElectricalBarrierController"); } }
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
                    { new StateEntry<bool>(LightsKey, false, "Tunnel lights", "A true signal of will turn on the lights in the tunnel.") },
                },
                new List<StateEntry<int>>()
                {
                }
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
                    { new StateEntry<bool>(ButtonKey, false, "Lights ON/OFF", "True if the guard pushed the button") },
                },
                new List<StateEntry<int>>()
                {
                }
            );
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int deltaTime)
        {
            Subcontroller.UpdateStep(deltaTime);
            UpdateLights();
            CheckActions(deltaTime);
        }
        
        private void UpdateLights()
        {
            Subscene.TunnelLights.UpdateLightSignal(SimulationInput.PollBoolean(LightsKey));
        }
        
        private void CheckActions(int deltaTime)
        {
            switch (_state)
            {
                case State.Idle:
                    if (ShouldPressButton())
                    {
                        _state = State.Pressing;
                        _pressTime = PressDuration;
                    }
                    break;
                case State.Pressing:
                    SimulationOutput.SetValue(ButtonKey, true);
                    _pressTime -= deltaTime;
                    if (_pressTime < 0)
                    {
                        _state = State.Releasing;
                        _pressTime = ReleaseDuration;
                    }
                    break;
                case State.Releasing:
                    SimulationOutput.SetValue(ButtonKey, false);
                    _pressTime -= deltaTime;
                    if (_pressTime < 0) _state = State.Idle;
                    break;
            }
        }
        
        private bool ShouldPressButton()
        {
            bool lightSignalOn = SimulationInput.PollBoolean(LightsKey);
            float carUnitOffset = Subscene.Vehicle.CarUnitOffset;
            if (lightSignalOn)
            {
                if (carUnitOffset > 0.9)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (carUnitOffset > VehicleAgentController.PathCheckpointPassed && carUnitOffset < 0.8)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
