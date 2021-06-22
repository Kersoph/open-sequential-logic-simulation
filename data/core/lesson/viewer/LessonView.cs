using Godot;


namespace Osls.Core
{
    public class LessonView : Control
    {
        #region ==================== Fields / Properties ====================
        private const string PlantViewPath = "PlantView";
        private const string LessonInfoPath = "LessonInfo";
        private const string IoInfoPath = "IoInfo";
        
        /// <summary>
        /// Controls the palant view to the simulation through the viewport.
        /// </summary>
        public PlantView PlantView { get { return GetNode<PlantView>(PlantViewPath); } }
        
        /// <summary>
        /// Controls the LessonInfoLabel to show the desired text.
        /// </summary>
        public LessonInfo LessonInfo { get { return GetNode<LessonInfo>(LessonInfoPath); } }
        
        /// <summary>
        /// Provides an info panel with the inputs and outputs of the simulated plant.
        /// </summary>
        public IoInfo IoInfo { get { return GetNode<IoInfo>(IoInfoPath); } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Informs the grid, that the selection has changed to this button and the UI needs to be updated.
        /// </summary>
        public void LoadAndShowInfo(ILessonEntity selectedLesson)
        {
            PlantView.UpdateLessonEntity(selectedLesson);
            LessonInfo.UpdateLessonEntity(selectedLesson);
            IoInfo.UpdateText(PlantView.LoadedSimulationNode?.SimulationOutput, PlantView.LoadedSimulationNode?.SimulationInput);
        }
        #endregion
    }
}