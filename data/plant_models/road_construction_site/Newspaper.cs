using Godot;


namespace Osls.Plants.RoadConstructionSite
{
    /// <summary>
    /// Topmost node for the newspaper scene
    /// </summary>
    public class Newspaper : Control
    {
        /// <summary>
        /// Sets the newspaper texts and makes them visible
        /// </summary>
        public void SetTexts(string headline, string infoBox, string news1, string news2)
        {
            GetNode<Label>("Headline").Text = headline;
            GetNode<Label>("InfoBox").Text = infoBox;
            GetNode<Label>("News1").Text = news1;
            GetNode<Label>("News2").Text = news2;
            Visible = true;
        }
    }
}
