using Godot;


namespace Osls.Plants.MassTestChamber
{
    public class EmergencyLight : SpotLight
    {
        #region ==================== Fields / Properties ====================
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void Update(bool active, int deltaTime)
        {
            if (active)
            {
                Visible = true;
                RotateZ(Mathf.Tau * 0.0015f * deltaTime);
            }
            else
            {
                Visible = false;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        #endregion
    }
}
