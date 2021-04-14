using Godot;


namespace Osls.Plants.RoadConstructionSite
{
    public class UI : Control
    {
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            GetNode<Slider>("LambdaOption").Value = SpawnTimeGenerator.GetLambda();
            GetNode<Slider>("LambdaOption").Connect("value_changed", this, nameof(OnValueChanged));
        }
        
        public void OnValueChanged(float value)
        {
            SpawnTimeGenerator.SetLambda(value);
        }
        #endregion
    }
}