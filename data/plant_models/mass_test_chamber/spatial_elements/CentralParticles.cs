using Godot;


namespace Osls.Plants.MassTestChamber
{
    public class CentralParticles : Particles
    {
        #region ==================== Fields / Properties ====================
        public const int MaxTemperature = 2300;
        private const float SpecificHeatCapacity = 4f;
        private const float MassCollectionPerSecond = 0.05f;
        private const float LaserHeatingPower = 300.0f;
        private ParticlesMaterial _processMaterial;
        
        /// <summary>
        /// The collected mass of the central particles
        /// </summary>
        public float CollectedMass { get; set; }
        
        /// <summary>
        /// The integrated energy of the central particles
        /// </summary>
        public float CollectedEnergy { get; set; }
        
        /// <summary>
        /// The temperature in Kelvin according to the energy and mass
        /// </summary>
        public int CentralTemperature
        {
            get
            {
                if (CollectedMass <= 0.001) return 293;
                return Mathf.RoundToInt(CollectedEnergy / (CollectedMass * SpecificHeatCapacity)) + 300;
            }
        }
        
        /// <summary>
        /// true if the particles are not caged and start roaming in the area
        /// </summary>
        public bool IsLeakingParticles { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called before the simulation is added to the main tree
        /// </summary>
        public void Setup()
        {
            _processMaterial = (ParticlesMaterial)ProcessMaterial;
        }
        
        /// <summary>
        /// Will emit particles according to the current state.
        /// </summary>
        public void ShowAs(bool buildingUpMass, bool caged, bool discharging)
        {
            int temperature = CentralTemperature;
            if (caged && temperature < MaxTemperature)
            {
                ShowAsOrbiting();
                Emitting = CollectedMass > 0.2f && !discharging;
                IsLeakingParticles = false;
            }
            else if (buildingUpMass && temperature < MaxTemperature)
            {
                if (!Emitting) Emitting = true;
                ShowAsBuildingUp();
                IsLeakingParticles = false;
            }
            else
            {
                if (Emitting) Emitting = false;
                ShowAsBreakFree();
                if (CollectedMass > 0.001) IsLeakingParticles = true;
            }
        }
        
        /// <summary>
        /// Updates the mass and temperature simulation
        /// </summary>
        /// <param name="buildingUpMass">If new mass is added</param>
        /// <param name="caged">If the particles are hold by the field of focus</param>
        /// <param name="discharging">If we are discharging</param>
        /// <param name="heating">If the laser is on</param>
        /// <param name="deltaTime">delta frame time in ms</param>
        public void ProcessState(bool buildingUpMass, bool caged, bool discharging, bool heating, int deltaTime)
        {
            if (buildingUpMass)
            {
                float newMass = MassCollectionPerSecond * deltaTime * 0.001f;
                CollectedMass += newMass;
            }
            if (!caged)
            {
                CollectedMass *= 0.9f;
                CollectedEnergy *= 0.9f;
            }
            if (heating)
            {
                float newEnergy = LaserHeatingPower * deltaTime * 0.001f;
                CollectedEnergy += newEnergy;
            }
            float emittedEnergy = CollectedEnergy * deltaTime * 0.001f * 0.07f;
            CollectedEnergy -= emittedEnergy;
            if (discharging)
            {
                CollectedMass -= CollectedMass * deltaTime * 0.001f * 0.4f;
                CollectedEnergy -= CollectedEnergy * deltaTime * 0.001f * 0.4f;
            }
        }
        
        /// <summary>
        /// Resets the central particles data and visuals
        /// </summary>
        public void Reset()
        {
            Restart();
            CollectedMass = 0f;
            CollectedEnergy = 0f;
            IsLeakingParticles = false;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Used if the cage is not activated and the focus on
        /// </summary>
        private void ShowAsBuildingUp()
        {
            _processMaterial.RadialAccel = -10f;
            _processMaterial.TangentialAccel = 0f;
            _processMaterial.Damping = 3f;
        }
        
        /// <summary>
        /// Used if the cage is activated
        /// </summary>
        private void ShowAsOrbiting()
        {
            _processMaterial.RadialAccel = -10f;
            _processMaterial.TangentialAccel = 3f;
            _processMaterial.Damping = 3f;
        }
        
        /// <summary>
        /// Used if there is no cage or focus
        /// </summary>
        private void ShowAsBreakFree()
        {
            _processMaterial.RadialAccel = 0.2f;
            _processMaterial.TangentialAccel = 0f;
            _processMaterial.Damping = 0f;
        }
        #endregion
    }
}
