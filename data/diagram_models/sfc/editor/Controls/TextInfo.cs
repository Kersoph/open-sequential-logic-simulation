using Godot;


namespace Osls.SfcEditor
{
    public class TextInfo : Label
    {
        #region ==================== Fields / Properties ====================
        private enum Stages { Idle, Build, Show, Destroy };
        private Stages _stages;
        [Export] private float _duration = 3.0f;
        private float _timeLeft;
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Process(float delta)
        {
            switch (_stages)
            {
                case Stages.Idle:
                    break;
                case Stages.Build:
                    _timeLeft -= delta;
                    PercentVisible = 1.0f - _timeLeft;
                    if (_timeLeft <= 0f)
                    {
                        PercentVisible = 1.0f;
                        _stages = Stages.Show;
                        _timeLeft = _duration;
                    }
                    break;
                case Stages.Show:
                    _timeLeft -= delta;
                    if (_timeLeft <= 0f)
                    {
                        _stages = Stages.Destroy;
                        _timeLeft = 1.0f;
                    }
                    break;
                case Stages.Destroy:
                    _timeLeft -= delta;
                    PercentVisible = _timeLeft;
                    if (_timeLeft <= 0f)
                    {
                        PercentVisible = 0.0f;
                        Visible = false;
                        _stages = Stages.Idle;
                    }
                    break;
            }
        }
        
        public void ShowMessage(string message)
        {
            Visible = true;
            _stages = Stages.Build;
            Text = message;
            _timeLeft = 1.0f;
        }
        #endregion
    }
}