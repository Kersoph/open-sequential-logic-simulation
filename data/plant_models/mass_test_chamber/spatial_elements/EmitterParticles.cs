using Godot;


namespace Osls.Plants.MassTestChamber
{
    public class EmitterParticles : Particles
    {
        #region ==================== Fields / Properties ====================
        private ParticlesMaterial _processMaterial;
        
        /// <summary>
        /// true if the process is valid and mass is added
        /// </summary>
        public bool IsProvidingMass { get; private set; }
        
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
        /// The particles will run out with TIME
        /// </summary>
        public void ShowAsOff(Cart cart)
        {
            if (Emitting) Emitting = false;
            IsProvidingMass = false;
            ShowAsToFarAway(1f - cart.RailPosition);
            IsLeakingParticles = false;
        }
        
        /// <summary>
        /// Will emit particles according to the current state.
        /// The cart must be at the front position, the force field off and the focus on.
        /// </summary>
        public void ShowAsActiveWith(Cart cart, bool field, bool focus)
        {
            if (!Emitting) Emitting = true;
            if (field)
            {
                ShowAsInvalidField();
                IsProvidingMass = false;
                IsLeakingParticles = true;
            }
            else
            {
                if (focus)
                {
                    if (cart.FrontPositionReached)
                    {
                        ShowAsCorrect();
                        IsProvidingMass = true;
                        IsLeakingParticles = false;
                    }
                    else
                    {
                        ShowAsToFarAway(1f - cart.RailPosition);
                        IsProvidingMass = false;
                        IsLeakingParticles = false;
                    }
                }
                else
                {
                    ShowAsNoFocus();
                    IsProvidingMass = false;
                    IsLeakingParticles = true;
                }
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void ShowAsCorrect()
        {
            _processMaterial.Spread = 20f;
            _processMaterial.Gravity = new Vector3(-5f, 0f, 0f);
            _processMaterial.RadialAccel = -10f;
            _processMaterial.TangentialAccel = 0f;
            _processMaterial.Damping = 5f;
        }
        
        /// <summary>
        /// Used if the cart is too far away from the intended position.
        /// </summary>
        /// <param name="amount">from 0 (correct frontal position) to 1 (back position)</param>
        private void ShowAsToFarAway(float amount)
        {
            _processMaterial.Spread = 20f;
            _processMaterial.Gravity = new Vector3(-5f, 0f, 0f);
            _processMaterial.RadialAccel = -10f;
            _processMaterial.TangentialAccel = 6f * amount;
            _processMaterial.Damping = 5f;
        }
        
        /// <summary>
        /// Used if no focus is active for the particles
        /// </summary>
        private void ShowAsNoFocus()
        {
            _processMaterial.Spread = 7f;
            _processMaterial.Gravity = new Vector3(5.5f, 0f, 0f);
            _processMaterial.RadialAccel = 0f;
            _processMaterial.TangentialAccel = 0f;
            _processMaterial.Damping = 4f;
        }
        
        /// <summary>
        /// Used if the force field is already active which will blow non caged particles away
        /// </summary>
        private void ShowAsInvalidField()
        {
            _processMaterial.Spread = 10f;
            _processMaterial.Gravity = new Vector3(5f, 0f, 0f);
            _processMaterial.RadialAccel = 0f;
            _processMaterial.TangentialAccel = 0f;
            _processMaterial.Damping = 5f;
        }
        #endregion
    }
}
