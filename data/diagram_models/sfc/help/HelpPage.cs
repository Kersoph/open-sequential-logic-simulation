using Godot;


namespace Osls.SfcEditor
{
    public class HelpPage : Control
    {
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_cancel"))
            {
                Visible = false;
            }
            if (@event.IsActionPressed("ui_help"))
            {
                Visible = !Visible;
            }
        }
    }
}
