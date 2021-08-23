using Godot;
using System.Collections.Generic;
using Osls.Bubbles;


namespace Osls.Plants.MassTestChamber
{
    public class EmergencySystem : SimulationPage
    {
        #region ==================== Fields / Properties ====================
        public const string SafeguardKey = "F013";
        public const string SafeguardResetKey = "K013";
        public const string AlarmLightKey = "P401";
        public const string AcknowledgeButtonKey = "S401";
        public const string ChargingKey = "K400";
        public const string FillingKey = "K401";
        public const string BatteryKey = "G400";
        public const string PressureKey = "C401";
        
        private int _particleTime;
        private DecorationNode _decoration;
        
        /// <summary>
        /// Possible states of the system
        /// </summary>
        public enum State { BuildUp, Release, Reset, Failure }
        /// <summary>
        /// The state of the emergency system
        /// </summary>
        public State SystemState { get; private set; }
        
        /// <summary>
        /// True if the controller set the signal to active
        /// </summary>
        public bool SafeguardResetSignal { get; private set; }
        /// <summary>
        /// True if the controller set the signal to active
        /// </summary>
        public bool AlarmLightSignal { get; private set; }
        /// <summary>
        /// True if the controller set the signal to active
        /// </summary>
        public bool ChargingSignal { get; private set; }
        /// <summary>
        /// True if the controller set the signal to active
        /// </summary>
        public bool FillingSignal { get; private set; }
        
        /// <summary>
        /// Links the central particle controller
        /// </summary>
        public CentralParticles CentralParticles { get; private set; }
        /// <summary>
        /// Links the central particle controller
        /// </summary>
        public EmergencyActors EmergencyActors { get; private set; }
        
        /// <summary>
        /// True if the safeguard was triggered and not yet reset
        /// </summary>
        public bool SafeguardSignal { get; private set; }
        /// <summary>
        /// True if someone wants to ack the error
        /// </summary>
        public bool AcknowledgeButtonSignal { get; private set; }
        /// <summary>
        /// The measured battery voltage
        /// </summary>
        public float BatterySignal { get; private set; } = 41f;
        /// <summary>
        /// The measured pressure in hPa
        /// 1013 is environment pressure
        /// </summary>
        public int PressureSignal { get; private set; } = 1802;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole page. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            CentralParticles = GetNode<CentralParticles>("CentralParticles");
            CentralParticles.Setup();
            EmergencyActors = GetNode<EmergencyActors>("EmergencyActors");
            EmergencyActors.Setup();
            _decoration = GetNode<DecorationNode>("DecorationNode");
            _decoration.Initialise();
        }
        
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public override void SetupUi()
        {
            GetNode<Control>("UI").Visible = true;
            GetNode<Button>("UI/Ack").Connect("button_down", this, nameof(OnAcknowledgeButtonChange), new Godot.Collections.Array { true });
            GetNode<Button>("UI/Ack").Connect("button_up", this, nameof(OnAcknowledgeButtonChange), new Godot.Collections.Array { false });
        }
        
        /// <summary>
        /// Keeps the Acknowledge Button status according to the last pressed signal
        /// </summary>
        public void OnAcknowledgeButtonChange(bool pressed)
        {
            AcknowledgeButtonSignal = pressed;
        }
        
        /// <summary>
        /// Resets the simulation back to the start
        /// </summary>
        public void ResetSimulation()
        {
            SystemState = State.BuildUp;
            _particleTime = 0;
            SafeguardResetSignal = false;
            AlarmLightSignal = false;
            ChargingSignal = false;
            FillingSignal = false;
            CentralParticles.Reset();
            SafeguardSignal = false;
            AcknowledgeButtonSignal = false;
            BatterySignal = 46f;
            PressureSignal = 2012;
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
                    { new StateEntry<bool>(SafeguardResetKey, false, "Reset protection device", "True will tell the protection device to reset.") },
                    { new StateEntry<bool>(AlarmLightKey, false, "Alarm light", "True will activate the alarm light in the chamber.\nFalse will turn it off.") },
                    { new StateEntry<bool>(ChargingKey, false, "Charge batteries", "True will charge the batteries with a constant current.\nFalse will stop charging them.") },
                    { new StateEntry<bool>(FillingKey, false, "Build pressure", "True will turn on the air compressor.\nFalse will turn it off.") },
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
                    { new StateEntry<bool>(SafeguardKey, false, "Protection device signal", "True after a system error has been detected\nand not yet reset.") },
                    { new StateEntry<bool>(AcknowledgeButtonKey, false, "Acknowledge alarm button", "True if someone acknowledges the alarm.\nFalse otherwise.") },
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>(BatteryKey, 45, "Battery voltage", "The measured voltage on the battery bank.") },
                    { new StateEntry<int>(PressureKey, 2000, "Pressure [hPa]", "The measured pressure in the tank in hectopascal.") },
                }
            );
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int deltaTime)
        {
            UpdateInputs(deltaTime);
            UpdateExperimentSimulation(deltaTime);
            UpdateOutputs();
        }
        
        private void UpdateInputs(int deltaTime)
        {
            SafeguardResetSignal = SimulationInput.PollBoolean(SafeguardResetKey);
            AlarmLightSignal = SimulationInput.PollBoolean(AlarmLightKey);
            ChargingSignal = SimulationInput.PollBoolean(ChargingKey);
            FillingSignal = SimulationInput.PollBoolean(FillingKey);
            EmergencyActors.EmergencyLight.Update(AlarmLightSignal, deltaTime);
            if (SafeguardResetSignal)
            {
                if (BatterySignal < 40 || PressureSignal < 1900)
                {
                    FailSystem();
                }
                else
                {
                    SafeguardSignal = false;
                }
            }
            if (ChargingSignal)
            {
                BatterySignal += 0.1f;
                EmergencyActors.Battery.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Waiting, 0.1f);
                if (BatterySignal > 50f)
                {
                    BatterySignal = 0;
                    FailSystem();
                }
            }
            if (FillingSignal)
            {
                PressureSignal += 2;
                EmergencyActors.Pressure.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Waiting, 0.1f);
                if (PressureSignal > 2400)
                {
                    PressureSignal = 0;
                    FailSystem();
                }
            }
        }
        
        /// <summary>
        /// Called if there is a problem in the emergency system, which leads to a total system failure
        /// </summary>
        private void FailSystem()
        {
            GetNode<Control>("UI/Failure").Visible = true;
            SystemState = State.Failure;
        }
        
        /// <summary>
        /// Procedure:
        /// 1. Build up particles
        /// 2. Release particles (experiment failure), discharge capacitor banks and preassure -> update detector
        /// 3. Stop particle emission, wait
        /// 4. Back to normal -> update detector
        /// </summary>
        private void UpdateExperimentSimulation(int deltaTime)
        {
            switch (SystemState)
            {
                case State.BuildUp:
                    CentralParticles.ShowAs(true, false, false);
                    _particleTime += deltaTime;
                    _decoration.UpdateToFailure(8000 - _particleTime);
                    if (_particleTime >= 8000)
                    {
                        _particleTime = 0;
                        SystemState = State.Release;
                        SafeguardSignal = true;
                        BatterySignal = 12f;
                        PressureSignal = 1021;
                    }
                    break;
                case State.Release:
                    CentralParticles.ShowAs(false, false, false);
                    _particleTime += deltaTime;
                    if (_particleTime >= 10000)
                    {
                        _particleTime = 0;
                        SystemState = State.Reset;
                    }
                    break;
                case State.Reset:
                    if (!SafeguardSignal)
                    {
                        SystemState = State.BuildUp;
                    }
                    break;
                case State.Failure:
                    SafeguardSignal = true;
                    break;
            }
        }
        
        private void UpdateOutputs()
        {
            SimulationOutput.SetValue(SafeguardKey, SafeguardSignal);
            SimulationOutput.SetValue(AcknowledgeButtonKey, AcknowledgeButtonSignal);
            SimulationOutput.SetValue(BatteryKey, (int)BatterySignal);
            SimulationOutput.SetValue(PressureKey, PressureSignal);
        }
        #endregion
    }
}
