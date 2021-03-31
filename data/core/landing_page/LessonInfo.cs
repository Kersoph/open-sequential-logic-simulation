using Godot;


namespace Osls.LandingPage
{
    /// <summary>
    /// Controls the LessonInfoLabel to show the desired text.
    /// </summary>
    public class LessonInfo : ColorRect
    {
        #region ==================== Fields ====================
        private const string LessonInfoLabelPath = "LessonInfoLabel";
        private RichTextLabel _richTextLabel;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            _richTextLabel = GetNode<RichTextLabel>(LessonInfoLabelPath);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Updates the BBCode description of the lesson entity.
        /// </summary>
        public void UpdateLessonEntity(LessonEntity lessonEntity)
        {
            _richTextLabel.BbcodeText = lessonEntity.Description;
        }
        #endregion
    }
}