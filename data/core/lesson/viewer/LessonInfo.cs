using Godot;
using System.Text;


namespace Osls.Core
{
    /// <summary>
    /// Controls the LessonInfoLabel to show the desired text.
    /// </summary>
    public class LessonInfo : ColorRect
    {
        #region ==================== Fields ====================
        private const string LessonInfoLabelPath = "LessonInfoLabel";
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Updates the BBCode description of the lesson entity.
        /// </summary>
        public void UpdateLessonEntity(LessonEntity lessonEntity)
        {
            StringBuilder builder = new StringBuilder(100);
            builder.AppendLine("[center][b][u]Lesson Description[/u][/b][/center]");
            builder.AppendLine(lessonEntity.Description);
            builder.AppendLine("[center][b][u]Goal[/u][/b][/center]");
            builder.AppendLine(lessonEntity.Goal);
            GetNode<RichTextLabel>(LessonInfoLabelPath).BbcodeText = builder.ToString();
        }
        #endregion
    }
}