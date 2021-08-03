using Godot;


namespace Osls.Plants.ElectricalBarrier
{
    public class TunnelLights : Spatial
    {
        #region ==================== Fields / Properties ====================
        private readonly bool[] _signalHistory = new bool[] { true, true, true, true, true, true };
        private int _flickerCount;
        
        /// <summary>
        /// True if all lights are on
        /// </summary>
        public bool AreLightsOn
        {
            get
            {
                for (int i = 0; i < _signalHistory.Length; i++)
                {
                    if (!_signalHistory[i]) return false;
                }
                return true;
            }
        }
        
        /// <summary>
        /// True if the flicker amount was so high that they broke down.
        /// </summary>
        public bool Broken { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void UpdateLightSignal(bool on)
        {
            if (!Broken)
            {
                PushValue(on);
                UpdateLights();
                CheckFlicker();
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void PushValue(bool on)
        {
            System.Array.Copy(_signalHistory, 0, _signalHistory, 1, _signalHistory.Length - 1);
            _signalHistory[0] = on;
        }
        
        private void CheckFlicker()
        {
            int positiveSwitchCount = 0;
            for (int i = 1; i < _signalHistory.Length; i++)
            {
                if (_signalHistory[i - 1] == false && _signalHistory[i] == true)
                {
                    positiveSwitchCount++;
                }
            }
            if (positiveSwitchCount > 1)
            {
                _flickerCount++;
            }
            if (_flickerCount > 30)
            {
                BreakDown();
            }
        }
        
        private void UpdateLights()
        {
            GetNode<Light>("Light1").Visible = _signalHistory[0];
            GetNode<Light>("Light2").Visible = _signalHistory[2];
            GetNode<Light>("Light3").Visible = _signalHistory[5];
        }
        
        private void BreakDown()
        {
            Broken = true;
            GetNode<Particles>("BrokenParticles").Visible = true;
            GetNode<Light>("Light1").Visible = false;
            GetNode<Light>("Light2").Visible = false;
            GetNode<Light>("Light3").Visible = false;
        }
        #endregion
    }
}