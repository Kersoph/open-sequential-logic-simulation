using Godot;
using Osls.Environment;


namespace Osls
{
    public static class LessonCollector
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Loads all aviable lessons at the locations:
        /// res://lessons/ and as an alternative user://lessons
        /// If it does not exist we try to create a default directory at
        /// res://lessons/ and as an alternative user://lessons
        /// </summary>
        public static ILessonEntity[] LoadLessons()
        {
            string[] LessonPaths = GetLessonPaths();
            System.Array.Sort<string>(LessonPaths);
            ILessonEntity[] lessonEntities = new ILessonEntity[LessonPaths.Length];
            for (int i = 0; i < LessonPaths.Length; i++)
            {
                LessonEntity lessonData = new LessonEntity(LessonPaths[i]);
                lessonData.LoadFolderContent();
                lessonEntities[i] = lessonData;
            }
            return lessonEntities;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Gets the base directories for the lesson folders.
        /// It first looks under the res folder and then in the user path.
        /// If none could be found we create a new dir and copy the default data in it.
        /// </summary>
        private static string[] GetLessonPaths()
        {
            string[] LessonPaths = TryLoad(EnvironmentPaths.LessonsFolderPath);
            if (LessonPaths != null && LessonPaths.Length > 0) return LessonPaths;
            LessonPaths = TryLoad(EnvironmentPaths.LessonsFolderPathAlternative);
            if (LessonPaths != null && LessonPaths.Length > 0) return LessonPaths;
            if (CreateDefault(EnvironmentPaths.LessonsFolderPath))
            {
                return TryLoad(EnvironmentPaths.LessonsFolderPath);
            }
            if (CreateDefault(EnvironmentPaths.LessonsFolderPathAlternative))
            {
                return TryLoad(EnvironmentPaths.LessonsFolderPathAlternative);
            }
            GD.PushWarning("Could not find any lessons at: " + EnvironmentPaths.LessonsFolderPath + " and " + EnvironmentPaths.LessonsFolderPathAlternative);
            return new string[0];
        }
        
        private static string[] TryLoad(string path)
        {
            if (!System.IO.Directory.Exists(path)) return new string[0];
            return System.IO.Directory.GetDirectories(path);
        }
        
        /// <summary>
        /// Returns true if the default files could be copied to the base path
        /// </summary>
        private static bool CreateDefault(string basePath)
        {
            GD.Print("Try to copy " + EnvironmentPaths.InternalLessonsFolderPath + " to " + basePath);
            Directory targetDir = new Directory();
            Directory internalDir = new Directory();
            Error result = targetDir.MakeDirRecursive(basePath);
            Error internalresult = internalDir.Open(EnvironmentPaths.InternalLessonsFolderPath);
            if (result != Error.Ok || internalresult != Error.Ok) return false;
            ProcessDirectory(EnvironmentPaths.InternalLessonsFolderPath, basePath);
            return true;
        }
        
        /// <summary>
        /// Copies all files from the given internal directory to the given path.
        /// As the internal data is packed we need to use godots directory system.
        /// </summary>
        private static void ProcessDirectory(string internalPath, string externalPath)
        {
            Directory internalDir = new Directory();
            Error result = internalDir.Open(internalPath);
            if (result != Error.Ok) return;
            internalDir.ListDirBegin(true, true);
            while (true)
            {
                string item = internalDir.GetNext();
                if (string.IsNullOrEmpty(item)) return;
                if (internalDir.CurrentIsDir())
                {
                    Directory targetDir = new Directory();
                    GD.Print("create " + externalPath + item + "/");
                    targetDir.MakeDir(externalPath + item + "/");
                    ProcessDirectory(internalPath + item + "/", externalPath + item + "/");
                }
                else
                {
                    GD.Print("copy " + internalPath + item + " to " + externalPath + item);
                    internalDir.Copy(internalPath + item, externalPath + item);
                }
            }
        }
        #endregion
    }
}
