using Godot;
using System;

namespace SfcSandbox.Data.Model.LandingPage
{
    /// <summary>
    /// Controls the simulations shown in the LessonPreviewViewport
    /// </summary>
    public class LessonPreview : Node
    {
        #region ==================== Fields ====================
        private Node _simulationNode;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Loads the simulation of the lesson and sets it as a child of the viewport.
        /// </summary>
        public void UpdateLessonEntity(LessonEntity lessonEntity)
        {
            if (_simulationNode != null)
            {
                GetNode("LessonPreviewViewport").RemoveChild(_simulationNode);
                _simulationNode.QueueFree();
                _simulationNode = null;
            }
            if (!String.IsNullOrEmpty(lessonEntity.SimulationPath))
            {
                _simulationNode = ((PackedScene)GD.Load(lessonEntity.SimulationPath)).Instance();
                GetNode("LessonPreviewViewport").AddChild(_simulationNode);
            }
        }
        #endregion
    }
}