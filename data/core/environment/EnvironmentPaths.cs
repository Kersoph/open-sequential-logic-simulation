namespace SfcSandbox.Data.Environment
{
    public static class EnvironmentPaths
    {
        /// <summary>
        /// Gets the globalized lesson folder path
        /// </summary>
        public static string LessonsFolderPath
        {
            get
            {
                return Godot.ProjectSettings.GlobalizePath("res://Lessons/");
            }
        }
    }
}