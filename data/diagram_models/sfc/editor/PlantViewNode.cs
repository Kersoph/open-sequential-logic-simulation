using Godot;
using System;
using SfcSandbox.Data.Model.SfcSimulation;

namespace SfcSandbox.Data.Model.SfcEditor
{
    /// <summary>
    /// Controls the palant view to the simulation through the viewport.
    /// </summary>
    public class PlantViewNode : ColorRect
    {
        #region ==================== Fields ====================
        /// <summary>
        /// The entity loaded by the PlantViewNode to provide access to the simulation interface.
        /// Can be null or Godot Invalid if no simulation is assigned to this lesson.
        /// </summary>
        public static SimulationControlNode LoadedSimulationNode { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Excahnges the current simulation view to the given simulation.
        /// </summary>
        public void UpdateLessonEntity(LessonEntity lessonEntity)
        {
            if (LoadedSimulationNode != null && Godot.Object.IsInstanceValid(LoadedSimulationNode))
            {
                LoadedSimulationNode.GetParent().RemoveChild(LoadedSimulationNode);
                LoadedSimulationNode.QueueFree();
                LoadedSimulationNode = null;
            }
            if (!String.IsNullOrEmpty(lessonEntity.SimulationPath))
            {
                LoadedSimulationNode = (SimulationControlNode)((PackedScene)GD.Load(lessonEntity.SimulationPath)).Instance();
                GetNode("PlantViewportContainer/PlantViewport").AddChild(LoadedSimulationNode);
            }
        }
        #endregion
    }
}