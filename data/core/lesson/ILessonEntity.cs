namespace Osls
{
    public interface ILessonEntity
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// The lesson title as a BBCode.
        /// </summary>
        string Title { get; }
        
        /// <summary>
        /// The Description title as a BBCode.
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// The goal of the lesson as a BBCode.
        /// </summary>
        string Goal { get; }
        
        /// <summary>
        /// The data folder path of the lesson.
        /// </summary>
        string FolderPath { get; }
        
        /// <summary>
        /// The complete path where the temporary sfc file is stored for this lesson.
        /// Used for test / scene communication and immediate saves.
        /// </summary>
        string TemporaryDiagramFilePath { get; }
        
        /// <summary>
        /// The complete path where the custom sfc file is stored for this lesson.
        /// </summary>
        string CustomDiagramFilePath { get; set; }
        
        /// <summary>
        /// The data path of the simulation.
        /// </summary>
        string SimulationPath { get; }
        
        /// <summary>
        /// The data path of the test ui.
        /// </summary>
        string TestPath { get; }
        
        /// <summary>
        /// The number of stars displayed in the landing page
        /// </summary>
        int Stars { get; set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Serializes the given amount of stars to the file
        /// </summary>
        void SetAndSaveStars(int stars);
        #endregion
    }
}
