using Godot;
using Osls.Plants;


namespace Osls.Core
{
    /// <summary>
    /// Controls the palant view to the simulation through the viewport.
    /// </summary>
    public class PlantView : ViewportContainer
    {
        #region ==================== Fields / Properties ====================
        private const string ViewportPath = "PlantViewport";
        
        /// <summary>
        /// The entity loaded by the PlantViewNode to provide access to the simulation interface.
        /// Can be null or Godot Invalid if no simulation is assigned to this lesson.
        /// </summary>
        public SimulationPage LoadedSimulationNode { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Exchanges the current simulation view to the given simulation.
        /// </summary>
        public void UpdateLessonEntity(ILessonEntity lessonEntity)
        {
            if (LoadedSimulationNode != null && IsInstanceValid(LoadedSimulationNode))
            {
                LoadedSimulationNode.QueueFree();
                LoadedSimulationNode = null;
            }
            if (!string.IsNullOrEmpty(lessonEntity.SimulationPath))
            {
                LoadedSimulationNode = (SimulationPage)((PackedScene)GD.Load(lessonEntity.SimulationPath)).Instance();
                GetNode(ViewportPath).AddChild(LoadedSimulationNode);
            }
        }
        #endregion
    }
}
