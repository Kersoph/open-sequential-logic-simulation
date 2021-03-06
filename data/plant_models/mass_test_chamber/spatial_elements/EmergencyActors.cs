using Godot;
using Osls.Bubbles;


namespace Osls.Plants.MassTestChamber
{
    public class EmergencyActors : Spatial
    {
        #region ==================== Fields / Properties ====================
        public EmergencyLight EmergencyLight { get; private set; }
        public BubbleSprite Pressure { get; private set; }
        public BubbleSprite Battery { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initialises the emergency actors
        /// </summary>
        public void Setup()
        {
            EmergencyLight = GetNode<EmergencyLight>("EmergencyLight");
            EmergencyLight.Update(false, 0);
            Pressure = GetNode<BubbleSprite>("Pressure");
            Battery = GetNode<BubbleSprite>("Capacitor");
        }
        #endregion
    }
}
