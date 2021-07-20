using Godot;


namespace Osls.Plants.MassTestChamber
{
    public class CentralParticles : Particles
    {
        #region ==================== Fields / Properties ====================
        private const float SpecificHeatCapacity = 10f;
        private const float MassCollectionPerSecond = 0.1f;
        private const float LaserHeatingPower = 0.1f;
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
            if (caged)
            {
                ShowAsOrbiting();
                if (Emitting == discharging) Emitting = !discharging;
            }
            else if (buildingUpMass)
            {
                if (!Emitting) Emitting = true;
                ShowAsBuildingUp();
            }
            else
            {
                if (Emitting) Emitting = false;
                ShowAsBreakFree();
            }
        }
        
        /// <summary>
        /// Updates the mass and temperature simulation
        /// </summary>
        public void ProcessState(bool buildingUpMass, bool caged, bool discharging, bool heating, int deltaTime)
        {
            if (buildingUpMass)
            {
                float newMass = MassCollectionPerSecond * deltaTime * 0.001f;
                CollectedMass += newMass;
            }
            if (!caged)
            {
                float escapedMass = CollectedMass * deltaTime * 0.001f;
                CollectedMass -= escapedMass;
            }
            if (heating)
            {
                float newEnergy = LaserHeatingPower * deltaTime * 0.001f;
                CollectedEnergy += newEnergy;
            }
            float emittedEnergy = CollectedEnergy * deltaTime * 0.001f * 0.1f;
            CollectedEnergy -= emittedEnergy;
            if (discharging)
            {
                CollectedMass *= 0.98f;
                CollectedEnergy *= 0.97f;
            }
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
