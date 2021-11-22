namespace Tests.Core
{
    public class LessonEntityMock : Osls.ILessonEntity
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// The lesson title as a BBCode.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The Description title as a BBCode.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The goal of the lesson as a BBCode.
        /// </summary>
        public string Goal { get; set; }
        
        /// <summary>
        /// The data folder path of the lesson.
        /// </summary>
        public string FolderPath { get; set; }
        
        /// <summary>
        /// The complete path where the temporary sfc file is stored for this lesson.
        /// Used for test / scene communication and immediate saves.
        /// </summary>
        public string TemporaryDiagramFilePath { get; set; }
        
        /// <summary>
        /// The complete path where the custom sfc file is stored for this lesson.
        /// </summary>
        public string CustomDiagramFilePath { get; set; }
        
        /// <summary>
        /// The data path of the simulation.
        /// </summary>
        public string SimulationPath { get; set; }
        
        /// <summary>
        /// The data path of the test ui.
        /// </summary>
        public string TestPath { get; set; }
        
        /// <summary>
        /// The number of stars displayed in the landing page
        /// </summary>
        public int Stars { get; set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Serializes the given amount of stars to the file
        /// </summary>
        public void SetAndSaveStars(int stars)
        {
            Stars = stars;
        }
        #endregion
    }
}
