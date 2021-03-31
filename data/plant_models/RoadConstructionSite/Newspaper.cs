using Godot;

namespace Osls.SfcSimulation.PlantModels.RoadConstructionSite
{
    /// <summary>
    /// Topmost node for the newspaper scene
    /// </summary>
    public class Newspaper : Control
    {
        /// <summary>
        /// Setrs the newspaper texts and makes them visible
        /// </summary>
        public void SetTexts(string headline, string infoBox, string news1, string news2)
        {
            this.GetNode<Label>("Headline").Text = headline;
            this.GetNode<Label>("InfoBox").Text = infoBox;
            this.GetNode<Label>("News1").Text = news1;
            this.GetNode<Label>("News2").Text = news2;
            this.Visible = true;
        }
    }
}