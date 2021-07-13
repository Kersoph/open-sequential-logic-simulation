using Godot;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.Viewer
{
    public class BreakpointOverlayButton : Button
    {
        #region ==================== Fields / Properties ====================
        private const string Scene = "res://data/diagram_models/sfc/processing_viewer/BreakpointOverlayButton.tscn";
        private int _patchId;
        private BreakpointManager _target;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Adds the breakpoint overlay and relays events.
        /// </summary>
        public static void CreateAndSetup(int id, SfcPatchNode patch, BreakpointManager target)
        {
            BreakpointOverlayButton button = (BreakpointOverlayButton)GD.Load<PackedScene>(Scene).Instance();
            button._patchId = id;
            button._target = target;
            button.Connect("toggled", button, nameof(OnButtonToggled));
            patch.OverlayWith(button);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void OnButtonToggled(bool pressed)
        {
            if (pressed)
            {
                _target.CheckSet.Add(_patchId);
            }
            else
            {
                _target.CheckSet.Remove(_patchId);
            }
            GetNode<TextureRect>("Breakpoint").Visible = pressed;
        }
        #endregion
    }
}
