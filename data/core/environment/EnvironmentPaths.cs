namespace Osls.Environment
{
    public static class EnvironmentPaths
    {
        /// <summary>
        /// Gets the forced globalized lesson folder path.
        /// ProjectSettings.GlobalizePath will not return the system path for res:// in export
        /// Currently there seems to be no way to get the execution folder directly,
        /// but we can use the exe path but only in export mode.
        /// </summary>
        public static string LessonsFolderPath
        {
            get
            {
                string path = Godot.ProjectSettings.GlobalizePath(InternalLessonsFolderPath);
                if (path.Contains(":"))
                {
                    return path;
                }
                path = Godot.OS.GetExecutablePath();
                int lastIndex = path.LastIndexOf("/");
                path = path.Substring(0, lastIndex + 1);
                return path + "lessons/";
            }
        }
        
        /// <summary>
        /// Gets the globalized alternative lesson folder path if the regular does not work.
        /// Especially used for MacOS as the res path is not writeable.
        /// </summary>
        public static string LessonsFolderPathAlternative
        {
            get
            {
                return Godot.ProjectSettings.GlobalizePath("user://lessons/");
            }
        }
        
        /// <summary>
        /// Gets the internal packed lesson folder path. Read only.
        /// </summary>
        public static string InternalLessonsFolderPath
        {
            get
            {
                return "res://lessons/";
            }
        }
    }
}