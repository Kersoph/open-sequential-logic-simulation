using Godot;


namespace Osls.Plants.MassTestChamber
{
    public class Chamber : Spatial
    {
        #region ==================== Fields / Properties ====================
        private const float MassNeeded = 0.4f;
        private const int TemperatureNeeded = 2000;
        private bool _isDischarging;
        private int _recordedDischargedTime;
        
        private Cart _emitterCart;
        private EmitterParticles _emitter;
        private Cart _focusCart;
        private CentralParticles _central;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called before the simulation is added to the  main tree
        /// </summary>
        public void Setup()
        {
            _emitterCart = GetNode<Cart>("EmitterCart");
            _emitterCart.Setup();
            _emitter = GetNode<EmitterParticles>("EmitterCart/EmitterParticles");
            _emitter.Setup();
            _focusCart = GetNode<Cart>("FocusCart");
            _focusCart.Setup();
            _central = GetNode<CentralParticles>("CentralParticles");
            _central.Setup();
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
                _emitterCart.Drive(direction > 0, deltaTime);
            }
            bool emitting = master.SimulationInput.PollBoolean(MassTestChamber.EmitterKey);
            if (emitting)
            {
                bool field = master.SimulationInput.PollBoolean(MassTestChamber.FieldGeneratorKey);
                bool focusOn = master.SimulationInput.PollBoolean(MassTestChamber.FocusKey);
                bool focusValid = _focusCart.FrontPositionReached && focusOn;
                _emitter.ShowAsActiveWith(_emitterCart, field, focusValid);
            }
            else
            {
                _emitter.ShowAsOff(_emitterCart);
            }
            master.SimulationOutput.SetValue(MassTestChamber.EmitterFrontKey, _emitterCart.FrontPositionReached);
            master.SimulationOutput.SetValue(MassTestChamber.EmitterBackKey, _emitterCart.BackPositionReached);
        }
        
        private void UpdateFocus(MassTestChamber master, int deltaTime)
        {
            int direction = master.SimulationInput.PollInteger(MassTestChamber.FocusMotorKey);
            if (direction != 0)
            {
                _focusCart.Drive(direction > 0, deltaTime);
            }
            bool field = master.SimulationInput.PollBoolean(MassTestChamber.FieldGeneratorKey);
            _central.ShowAs(_emitter.IsProvidingMass, field, _isDischarging);
            master.SimulationOutput.SetValue(MassTestChamber.FocusFrontKey, _focusCart.FrontPositionReached);
            master.SimulationOutput.SetValue(MassTestChamber.FocusBackKey, _focusCart.BackPositionReached);
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
            _central.ProcessState(_emitter.IsProvidingMass, cagedParticles, _isDischarging, laserActive, deltaTime);
            int measuredNoise = Mathf.RoundToInt(GD.Randf() * 16f - 8f);
            int measuredTemperature = _central.CentralTemperature + measuredNoise;
            master.SimulationOutput.SetValue(MassTestChamber.TemperatureSensorKey, measuredTemperature);
            bool massOK = _central.CollectedMass > MassNeeded;
            master.SimulationOutput.SetValue(MassTestChamber.MassSufficientKey, massOK);
        }
        
        /// <summary>
        /// According to the description, we need the right amount of mass, heat and trapped particles
        /// </summary>
        private void UpdateDischarge(MassTestChamber master, int deltaTime)
        {
            bool field = master.SimulationInput.PollBoolean(MassTestChamber.FieldGeneratorKey);
            int centralTemperature = _central.CentralTemperature;
            float centralMass = _central.CollectedMass;
            if (_isDischarging)
            {
                _recordedDischargedTime += deltaTime;
                if (!field || centralTemperature < TemperatureNeeded - 300 || centralMass <= 0f)
                {
                    _isDischarging = false;
                }
            }
            else
            {
                if (field && centralTemperature > TemperatureNeeded && centralMass > MassNeeded)
                {
                    _isDischarging = true;
                }
            }
            master.SimulationOutput.SetValue(MassTestChamber.DetectorKey, _isDischarging);
        }
        #endregion
    }
}
