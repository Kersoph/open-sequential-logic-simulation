using Godot;
using Godot.Collections;

namespace Osls
{
    /// <summary>
    /// Deserialized lesson entity class.
    /// It loads and stores all content in the given lesson folder.
    /// Todo: Swap godot JSON parser to something else.
    /// </summary>
    public class LessonEntity
    {
        #region ==================== Fields ====================
        public const string DescriptionFilePath = "/Description.json";
        public const string UserResultDirectory = "/User";
        public const string UserResultFilePath = UserResultDirectory + "/Results.json";
        public const string UserDiagramDirectory = UserResultDirectory + "/Diagram.sfc";
        
        /// <summary>
        /// The lesson title as a BBCode.
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// The Description title as a BBCode.
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The goal of the lesson as a BBCode.
        /// </summary>
        public string Goal { get; private set; }
        
        /// <summary>
        /// The data folder path of the lesson.
        /// </summary>
        public string FolderPath { get; private set; }
        
        /// <summary>
        /// The complete path where the sfc file is stored for this lesson.
        /// </summary>
        public string DiagramFilePath { get { return FolderPath + UserDiagramDirectory; } }
        
        /// <summary>
        /// The data path of the simulation.
        /// </summary>
        public string SimulationPath { get; private set; }
        
        /// <summary>
        /// The data path of the test ui.
        /// </summary>
        public string TestPath { get; private set; }
        
        /// <summary>
        /// The number of stars displayed in the landing page
        /// </summary>
        public int Stars { get; set; }
        
        private Dictionary _loadedJsonResultDictionary;
        #endregion
        
        
        #region ==================== Constructor ====================
        public LessonEntity(string folderPath)
        {
            FolderPath = folderPath;
            if (!System.IO.Directory.Exists(FolderPath + UserResultDirectory))
            {
                System.IO.Directory.CreateDirectory(FolderPath + UserResultDirectory);
            }
            if (!System.IO.File.Exists(FolderPath + UserResultFilePath))
            {
                using (File descriptionFile = new File())
                {
                    descriptionFile.Open(FolderPath + UserResultFilePath, File.ModeFlags.Write);
                    descriptionFile.StoreString("{\n}");
                    descriptionFile.Close(); // Godot is not pushing the contents in the using pattern...
                }
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Loads the whole folder at the FolderPath.
        /// </summary>
        public void LoadFolderContent()
        {
            LoadDescriptionDictionary();
            LoadUserResultDictionary();
        }
        
        /// <summary>
        /// Serializes the given amount of stars to the file
        /// </summary>
        public void SetAndSaveStars(int stars)
        {
            Stars = stars;
            using (File descriptionFile = new File())
            {
                Error e = descriptionFile.Open(FolderPath + UserResultFilePath, File.ModeFlags.WriteRead);
                if (e != Error.Ok) return;
                _loadedJsonResultDictionary["Stars"] = stars;
                descriptionFile.StoreLine(JSON.Print(_loadedJsonResultDictionary, "\t"));
                descriptionFile.Close(); // Godot is not handling it with the using pattern atm...
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Loads the description json as a godot dictionary.
        /// It is different than the System.Runtime.Serialization json attribute deserilisation.
        /// But it is sufficient for our needs.
        /// </summary>
        private void LoadDescriptionDictionary()
        {
            Dictionary descriptionDictionary;
            using (File descriptionFile = new File())
            {
                descriptionFile.Open(FolderPath + DescriptionFilePath, File.ModeFlags.Read);
                JSONParseResult result = JSON.Parse(descriptionFile.GetAsText());
                descriptionDictionary = (Dictionary) result.Result;
                result.Dispose();
                descriptionFile.Close(); // Godot is not handling it with the using pattern atm...
            }
            Title = (string)descriptionDictionary["Title"];
            Description = (string)descriptionDictionary["Description"];
            Goal = (string)descriptionDictionary["Goal"];
            SimulationPath = (string)descriptionDictionary["Simulation"];
            TestPath = (string)descriptionDictionary["Test"];
        }
        
        /// <summary>
        /// Loads the user result json as a godot dictionary.
        /// We may want to swap the Godot JSON parser, as it is not as good as expected.
        /// </summary>
        private void LoadUserResultDictionary()
        {
            using (File descriptionFile = new File())
            {
                Error e = descriptionFile.Open(FolderPath + UserResultFilePath, File.ModeFlags.Read);
                if (e != Error.Ok) return;
                JSONParseResult result = JSON.Parse(descriptionFile.GetAsText());
                _loadedJsonResultDictionary = (Dictionary) result.Result;
                result.Dispose();
                descriptionFile.Close(); // Godot is not handling it with the using pattern atm...
            }
            if (_loadedJsonResultDictionary == null) return; // File was deleted or invalid.
            if (_loadedJsonResultDictionary.Contains("Stars"))
            {
                Stars = Mathf.RoundToInt((float)_loadedJsonResultDictionary["Stars"]);
            }
        }
        #endregion
    }
}