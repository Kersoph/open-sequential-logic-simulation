using Godot;


namespace Osls.Plants.MassTestChamber
{
    public class Chamber : Spatial
    {
        #region ==================== Fields / Properties ====================
        public const float MassNeeded = 0.4f;
        public const float BurnOutMass = 0.002f;
        public const int TemperatureNeeded = 2000;
        public const int TurnOffTemperature = 1700;
        public bool IsDischarging { get; private set; }
        public int RecordedDischargedTime { get; private set; }
        public Cart EmitterCart { get; private set; }
        public EmitterParticles Emitter { get; private set; }
        public Cart FocusCart { get; private set; }
        public CentralParticles Central { get; private set; }
        public Particles Discharge { get; private set; }
        public Particles Laser { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called before the simulation is added to the  main tree
        /// </summary>
        public void Setup()
        {
            EmitterCart = GetNode<Cart>("EmitterCart");
            EmitterCart.Setup();
            Emitter = GetNode<EmitterParticles>("EmitterCart/EmitterParticles");
            Emitter.Setup();
            FocusCart = GetNode<Cart>("FocusCart");
            FocusCart.Setup();
            Central = GetNode<CentralParticles>("CentralParticles");
            Central.Setup();
            Discharge = GetNode<Particles>("DischargeParticles");
            Laser = GetNode<Particles>("LaserParticles");
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void Update(MassTestChamber master, int deltaTime)
        {
            UpdateEmitter(master, deltaTime);
            UpdateFocus(master, deltaTime);
            UpdateCentralMass(master, deltaTime);
            UpdateDischarge(master, deltaTime);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void UpdateEmitter(MassTestChamber master, int deltaTime)
        {
            int direction = master.SimulationInput.PollInteger(MassTestChamber.EmitterMotorKey);
            if (direction != 0)
            {
                EmitterCart.Drive(direction > 0, deltaTime);
            }
            bool emitting = master.SimulationInput.PollBoolean(MassTestChamber.EmitterKey);
            if (emitting)
            {
                bool field = master.SimulationInput.PollBoolean(MassTestChamber.FieldGeneratorKey);
                bool focusOn = master.SimulationInput.PollBoolean(MassTestChamber.FocusKey);
                bool focusValid = FocusCart.FrontPositionReached && focusOn;
                Emitter.ShowAsActiveWith(EmitterCart, field, focusValid);
            }
            else
            {
                Emitter.ShowAsOff(EmitterCart);
            }
            master.SimulationOutput.SetValue(MassTestChamber.EmitterFrontKey, EmitterCart.FrontPositionReached);
            master.SimulationOutput.SetValue(MassTestChamber.EmitterBackKey, EmitterCart.BackPositionReached);
        }
        
        private void UpdateFocus(MassTestChamber master, int deltaTime)
        {
            int direction = master.SimulationInput.PollInteger(MassTestChamber.FocusMotorKey);
            if (direction != 0)
            {
                FocusCart.Drive(direction > 0, deltaTime);
            }
            bool field = master.SimulationInput.PollBoolean(MassTestChamber.FieldGeneratorKey);
            Central.ShowAs(Emitter.IsProvidingMass, field, IsDischarging);
            master.SimulationOutput.SetValue(MassTestChamber.FocusFrontKey, FocusCart.FrontPositionReached);
            master.SimulationOutput.SetValue(MassTestChamber.FocusBackKey, FocusCart.BackPositionReached);
        }
        
        /// <summary>
        /// https://en.wikipedia.org/wiki/Black_body
        /// https://en.wikipedia.org/wiki/Color_temperature
        /// </summary>
        private void UpdateCentralMass(MassTestChamber master, int deltaTime)
        {
            bool field = master.SimulationInput.PollBoolean(MassTestChamber.FieldGeneratorKey);
            bool focusOn = master.SimulationInput.PollBoolean(MassTestChamber.FocusKey);
            bool cagedParticles = field || focusOn;
            bool laserActive = master.SimulationInput.PollBoolean(MassTestChamber.LaserKey);
            Central.ProcessState(Emitter.IsProvidingMass, cagedParticles, IsDischarging, laserActive, deltaTime);
            Laser.Visible = laserActive;
            int measuredNoise = Mathf.RoundToInt(GD.Randf() * 16f - 8f);
            int measuredTemperature = Central.CentralTemperature + measuredNoise;
            master.SimulationOutput.SetValue(MassTestChamber.TemperatureSensorKey, measuredTemperature);
            bool massOK = Central.CollectedMass > MassNeeded;
            master.SimulationOutput.SetValue(MassTestChamber.MassSufficientKey, massOK);
        }
        
        /// <summary>
        /// According to the description, we need the right amount of mass, heat and trapped particles
        /// </summary>
        private void UpdateDischarge(MassTestChamber master, int deltaTime)
        {
            bool field = master.SimulationInput.PollBoolean(MassTestChamber.FieldGeneratorKey);
            int centralTemperature = Central.CentralTemperature;
            float centralMass = Central.CollectedMass;
            if (IsDischarging)
            {
                RecordedDischargedTime += deltaTime;
                if (!field || centralTemperature < TurnOffTemperature || centralMass < BurnOutMass)
                {
                    IsDischarging = false;
                    Discharge.Emitting = false;
                    Finish();
                }
            }
            else
            {
                if (field && centralTemperature > TemperatureNeeded && centralMass > MassNeeded)
                {
                    IsDischarging = true;
                    Discharge.Emitting = true;
                }
            }
            master.SimulationOutput.SetValue(MassTestChamber.DetectorKey, IsDischarging);
        }
        
        private void Finish()
        {
            Central.Visible = false;
        }
        #endregion
    }
}
