using Godot;


namespace Osls.Plants.MassTestChamber
{
    public class PaperLog : NinePatchRect
    {
        #region ==================== Fields / Properties ====================
        private RichTextLabel _label;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initialises the paper log
        /// </summary>
        public void Setup()
        {
            _label = GetNode<RichTextLabel>("Text");
            _label.Clear();
        }
        
        /// <summary>
        /// Appends the bbcode to the log
        /// </summary>
        public void Append(string text)
        {
            _label.AppendBbcode(text);
        }
        
        /// <summary>
        /// Appends the bbcode to the log
        /// </summary>
        public void AppendWarning(string text)
        {
            _label.AppendBbcode("W: [b]" + text + "[/b]");
        }
        
        /// <summary>
        /// Appends the bbcode to the log
        /// </summary>
        public void AppendError(string text)
        {
            _label.AppendBbcode("E: [b][color=#640000]" + text + "[/color][/b]");
        }
        
        public override void _Process(float delta)
        {
            int total = _label.GetTotalCharacterCount();
            int visible = _label.VisibleCharacters;
            if (total > visible) _label.VisibleCharacters++;
        }
        #endregion
    }
}
