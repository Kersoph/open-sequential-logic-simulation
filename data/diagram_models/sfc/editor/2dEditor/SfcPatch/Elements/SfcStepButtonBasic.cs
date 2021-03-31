using Godot;

namespace SfcSandbox.Data.Model.SfcEditor
{
    /// <summary>
    /// Abstract class for sfc step buttons as we can not use an interface
    /// because we need to inherit godots button to instanciate this type of buttons.
    /// </summary>
    public abstract class SfcStepButtonBasic : Button
    {
        /// <summary>
        /// Displays the text as a multi line string in the editor if possible.
        /// </summary>
        public virtual void SetEditorText(string text)
        {
        }
        
        /// <summary>
        /// Called when the button is pressed.
        /// </summary>
        public override void _Pressed()
        {
            this.GetNode<SfcStepNode>("..").NotifyButtonPressed();
        }
        
        /// <summary>
        /// Marks or unmarks the step if possible
        /// </summary>
        public virtual void MarkStep(bool setMark)
        {
        }
    }
}