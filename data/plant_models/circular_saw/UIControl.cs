using Godot;
using Godot.Collections;


namespace Osls.Plants.CircularSaw
{
    public class UIControl : Control
    {
        #region ==================== Fields / Properties ====================
        private CircularSawModel CircularSawModel { get { return GetNode<CircularSawModel>(".."); } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called before the simulation is added to the main tree
        /// </summary>
        public void SetupUI()
        {
            Visible = true;
            GetNode<Button>("ONButton").Connect("button_down", this, nameof(SetONState), new Array { true });
            GetNode<Button>("ONButton").Connect("button_up", this, nameof(SetONState), new Array { false });
            GetNode<Button>("OFFButton").Connect("button_down", this, nameof(SetOFFState), new Array { true });
            GetNode<Button>("OFFButton").Connect("button_up", this, nameof(SetOFFState), new Array { false });
            
            GetNode<Button>("Combined").Connect("button_down", this, nameof(SetONState), new Array { true });
            GetNode<Button>("Combined").Connect("button_up", this, nameof(SetONState), new Array { false });
            GetNode<Button>("Combined").Connect("button_down", this, nameof(SetOFFState), new Array { true });
            GetNode<Button>("Combined").Connect("button_up", this, nameof(SetOFFState), new Array { false });
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void SetONState(bool state)
        {
            CircularSawModel.ONButtonState = state;
        }
        
        private void SetOFFState(bool state)
        {
            CircularSawModel.OFFButtonState = state;
        }
        #endregion
    }
}
