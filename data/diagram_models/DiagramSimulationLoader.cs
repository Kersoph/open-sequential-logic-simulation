using Osls.SfcEditor;
using System.Text;
using Osls.SfcSimulation.Viewer;
using Osls.Plants;


namespace Osls
{
    public static class DiagramSimulationLoader
    {
        #region ==================== Fields / Properties ====================
        public const string SFC = ".sfc";
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Loads the TemporaryDiagramFilePath and creates a simulation master from it.
        /// </summary>
        /// <param name="openedLesson">The opened lesson we want to create a simulation master from.</param>
        /// <param name="simulationPage">the target simulation page we want to control.</param>
        /// <returns>the simulation master if the data could be loaded. Null otherwise.</returns>
        public static IDiagramSimulationMaster LoadTemp(ILessonEntity openedLesson, SimulationPage simulationPage)
        {
            string tempFile = openedLesson.TemporaryDiagramFilePath;
            if (string.IsNullOrEmpty(tempFile)) return null;
            if (tempFile.EndsWith(SFC)) return LoadSfc(openedLesson, simulationPage);
            return null;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private static IDiagramSimulationMaster LoadSfc(ILessonEntity openedLesson, SimulationPage simulationPage)
        {
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(openedLesson.TemporaryDiagramFilePath);
            if (sfcEntity == null) return null;
            return new Master(sfcEntity, simulationPage);
        }
        #endregion
    }
}
