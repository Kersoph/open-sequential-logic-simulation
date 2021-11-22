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
        public override void _Ready()
        {
            Connect("resized", this, nameof(OnResized));
        }
        
        /// <summary>
        /// Updates the BBCode description of the lesson entity.
        /// </summary>
        public void UpdateLessonEntity(ILessonEntity lessonEntity)
        {
            StringBuilder builder = new StringBuilder(100);
            builder.AppendLine("[center][b][u]Lesson Description[/u][/b][/center]");
            builder.AppendLine(lessonEntity.Description);
            builder.AppendLine("[center][b][u]Goal[/u][/b][/center]");
            builder.AppendLine(lessonEntity.Goal);
            GetNode<RichTextLabel>(LessonInfoLabelPath).BbcodeText = builder.ToString();
        }
        
        /// <summary>
        /// Called if the control was resized to update the block text.
        /// This is fixed in the next Godot version:
        /// https://github.com/godotengine/godot/issues/45488
        /// https://github.com/godotengine/godot/pull/54031
        /// </summary>
        public void OnResized()
        {
            GetNode<RichTextLabel>(LessonInfoLabelPath).BbcodeText = GetNode<RichTextLabel>(LessonInfoLabelPath).BbcodeText;
        }
        #endregion
    }
}
