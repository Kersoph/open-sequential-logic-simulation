using Godot;

namespace SfcSandbox.Data.Model.SfcEditor
{
    /// <summary>
    /// Represents the MultilineLabel in Godot used fot the SfcStepButton.
    /// It is a workaround to have multiple lines without loosing the formatting options.
    /// </summary>
    public class MultilineLabel : Label
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Sets the string as a two line text if it is too long
        /// for one line without loosing the formatter.
        /// </summary>
        public void SetMultilineText(string text)
        {
            if (text.Length > 9)
            {
                this.Text = text.Insert(8, "\n");
            }
            else
            {
                this.Text = text;
            }
        }
        #endregion
    }
}