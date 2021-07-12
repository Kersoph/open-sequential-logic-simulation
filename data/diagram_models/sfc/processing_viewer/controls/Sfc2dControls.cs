using Godot;
using Osls.SfcEditor;


namespace Osls.SfcSimulation.Viewer
{
    public class Sfc2dControls : Control
    {
        #region ==================== Fields / Properties ====================
        [Export] private NodePath _sfc2dEditorNodePath = "../Sfc2dEditor";
        [Export] private NodePath _sfcSimulationViewerPath = "../../..";
        
        /// <summary>
        /// Gets the sfc editor node
        /// </summary>
        public Sfc2dEditorNode Sfc2dEditorNode { get { return GetNode<Sfc2dEditorNode>(_sfc2dEditorNodePath); } }
        
        /// <summary>
        /// Gets the simulation viewer
        /// </summary>
        public SfcSimulationViewer SfcSimulationViewer { get { return GetNode<SfcSimulationViewer>(_sfcSimulationViewerPath); } }
        #endregion
    }
}