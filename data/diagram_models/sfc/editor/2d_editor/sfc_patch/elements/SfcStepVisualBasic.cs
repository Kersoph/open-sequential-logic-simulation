using Godot;

namespace Osls.SfcEditor
{
    /// <summary>
    /// Abstract class for sfc step buttons as we can not use an interface
    /// because we need to inherit godots button to instantiate this type of buttons.
    /// </summary>
    public abstract class SfcStepVisualBasic : Control
    {
        /// <summary>
        /// Displays the text as a multi line string in the editor if possible.
        /// </summary>
        public virtual void SetEditorText(string text, Sfc2dEditorControl context)
        {
        }
        
        /// <summary>
        /// Marks or unmarks the step if possible
        /// </summary>
        public virtual void MarkStep(bool setMark)
        {
        }
        
        /// <summary>
        /// Applies all user edits to the data model.
        /// </summary>
        public virtual void ApplyEdits()
        {
        }
    }
}