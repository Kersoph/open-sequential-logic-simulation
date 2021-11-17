using Godot;


namespace Osls.Plants.CircularSaw
{
    public class CircularSawNode : MeshInstance
    {
        #region ==================== Fields / Properties ====================
        public const string MotorKey = "M1";
        private readonly bool[] _signalHistory = new bool[10];
        private int _flickerCount;
        private MeshInstance _blade;
        
        /// <summary>
        /// True if the motor has power
        /// </summary>
        public bool MotorSwitchedOn { get; private set; }
        
        /// <summary>
        /// True if the flicker amount was so high that it broke down.
        /// </summary>
        public bool Broken { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called before the simulation is added to the main tree
        /// </summary>
        public void Setup()
        {
            _blade = GetNode<MeshInstance>("Blade");
        }
        
        public void CalculateNextStep(CircularSawModel master, int deltaTime)
        {
            MotorSwitchedOn = master.SimulationInput.PollBoolean(MotorKey);
            PushValue(MotorSwitchedOn);
            if (MotorSwitchedOn && !Broken)
            {
                _blade.RotateX(0.02f * deltaTime);
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
        
        private void BreakDown()
        {
            Broken = true;
            GetNode<Particles>("BreakdownSmoke").Visible = true;
            GetNode<Flash>("BreakdownFlash").FlashNextFrame();
        }
        #endregion
    }
}
