using Godot;
using System.Collections.Generic;
using Osls.Bubbles;


namespace Osls.Plants.MassTestChamber
{
    public class EmergencySystem : SimulationPage
    {
        #region ==================== Fields / Properties ====================
        public const string AlarmLightKey = "=A4-H1";
        public const string AlarmHornKey = "=A4-P1";
        public const string ParticleSensorKey = "=A4-B1";
        public const string AcknowledgeButtonKey = "=A4-S1";
        public const string MuteButtonKey = "=A4-S2";
        
        private enum ParticleState { BuildUp, Release, Idle }
        private ParticleState _particleState;
        private int _particleTime;
        
        public bool AlarmLightSignal { get; private set; }
        public bool AlarmHornSignal { get; private set; }
        
        public CentralParticles CentralParticles { get; private set; }
        public EmergencyActors EmergencyActors { get; private set; }
        
        public bool ParticleSensorSignal { get; private set; }
        public bool AcknowledgeButtonSignal { get; private set; }
        public bool MuteButtonSignal { get; private set; }
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
            GetNode<Button>("UI/Mute").Connect("button_down", this, nameof(OnMuteButtonChange), new Godot.Collections.Array { true });
            GetNode<Button>("UI/Mute").Connect("button_up", this, nameof(OnMuteButtonChange), new Godot.Collections.Array { false });
        }
        
        /// <summary>
        /// Keeps the Acknowledge Button status according to the last pressed signal
        /// </summary>
        public void OnAcknowledgeButtonChange(bool pressed)
        {
            AcknowledgeButtonSignal = pressed;
        }
        
        /// <summary>
        /// Keeps the Mute Button status according to the last pressed signal
        /// </summary>
        public void OnMuteButtonChange(bool pressed)
        {
            MuteButtonSignal = pressed;
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
                    { new StateEntry<bool>(AlarmLightKey, false, "Alarm Light", "True will activate the alarm light in the chamber,\nfalse will turn it off.") },
                    { new StateEntry<bool>(AlarmHornKey, false, "Alarm Horn", "True will turn on the alarm horn,\nfalse will turn it off.") },
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
                    { new StateEntry<bool>(ParticleSensorKey, false, "Particle Sensor", "True if roaming particles were detected,\nFalse otherwise.") },
                    { new StateEntry<bool>(AcknowledgeButtonKey, false, "Acknowledge Button", "True if someone is acknowledges the alarm,\nFalse otherwise.") },
                    { new StateEntry<bool>(MuteButtonKey, false, "Alarm Mute Button", "True if someone wants to mute the alarm horn,\nFalse otherwise.") },
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
            UpdateInputs(deltaTime);
            UpdateExperimentSimulation(deltaTime);
            UpdateOutputs();
        }
        
        private void UpdateInputs(int deltaTime)
        {
            AlarmLightSignal = SimulationInput.PollBoolean(AlarmLightKey);
            AlarmHornSignal = SimulationInput.PollBoolean(AlarmHornKey);
            EmergencyActors.EmergencyLight.Update(AlarmLightSignal, deltaTime);
            if (AlarmHornSignal)
            {
                EmergencyActors.Alarm.ShowAs(BubbleSprite.Bubble.Shout, BubbleSprite.Expression.Exclamation, 1f, true);
            }
        }
        
        /// <summary>
        /// Procedure:
        /// 1. Build up particles
        /// 2. Release particles (experiment failure) -> update detector
        /// 3. Stop particle emission, wait
        /// 4. Back to normal -> update detector
        /// </summary>
        private void UpdateExperimentSimulation(int deltaTime)
        {
            switch (_particleState)
            {
                case ParticleState.BuildUp:
                    CentralParticles.ShowAs(true, false, false);
                    _particleTime += deltaTime;
                    if (_particleTime >= 8000)
                    {
                        _particleTime = 0;
                        _particleState = ParticleState.Release;
                        ParticleSensorSignal = true;
                    }
                    break;
                case ParticleState.Release:
                    CentralParticles.ShowAs(false, false, false);
                    _particleTime += deltaTime;
                    if (_particleTime >= 10000)
                    {
                        _particleTime = 0;
                        _particleState = ParticleState.Release;
                        ParticleSensorSignal = false;
                    }
                    break;
                case ParticleState.Idle:
                    break;
            }
        }
        
        private void UpdateOutputs()
        {
            SimulationOutput.SetValue(ParticleSensorKey, ParticleSensorSignal);
            SimulationOutput.SetValue(AcknowledgeButtonKey, AcknowledgeButtonSignal);
            SimulationOutput.SetValue(MuteButtonKey, MuteButtonSignal);
        }
        #endregion
    }
}
