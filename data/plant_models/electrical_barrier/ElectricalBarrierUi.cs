using Godot;


namespace Osls.Plants.ElectricalBarrier
{
    public class ElectricalBarrierUi : Control
    {
        #region ==================== Fields / Properties ====================
        private ElectricalBarrier _electricalBarrier;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// </summary>
        public void SetupUi(ElectricalBarrier electricalBarrier)
        {
            _electricalBarrier = electricalBarrier;
            Visible = true;
            GetNode<Button>("BlockEntryButton").Connect("toggled", this, nameof(OnBlockEntryToggled));
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void OnBlockEntryToggled(bool pressed)
        {
            _electricalBarrier.Guard.AllowVehiclePass = !pressed;
        }
        #endregion
    }
}
