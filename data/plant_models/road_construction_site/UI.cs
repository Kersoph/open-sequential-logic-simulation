using Godot;


namespace Osls.Plants.RoadConstructionSite
{
    public class UI : Control
    {
        #region ==================== Public Methods ====================
        public void Setup()
        {
            GetNode<Slider>("LambdaOption").Value = SpawnTimeGenerator.GetLambda();
            GetNode<Slider>("LambdaOption").Connect("value_changed", this, nameof(OnValueChanged));
            Visible = true;
        }
        
        public void OnValueChanged(float value)
        {
            SpawnTimeGenerator.SetLambda(value);
        }
        #endregion
    }
}