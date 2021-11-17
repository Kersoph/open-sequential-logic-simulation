using Godot;


namespace Osls.Plants.CircularSaw
{
    public class TestCase
    {
        #region ==================== Fields / Properties ====================
        private CircularSawTest _master;
        private int _testStepCounter;
        private bool _onButtonState;
        private bool _offButtonState;
        private bool _expectedMotorState;
        private string _labelNodePath;
        private string _okText;
        private string _errorText;
        private CircularSawTest.TestStep _successStep;
        private CircularSawTest.TestStep _failureStep;
        
        /// <summary>
        /// True if something went wrong in this step
        /// </summary>
        public bool HadError { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public TestCase(CircularSawTest master, bool onButtonState, bool offButtonState, bool expectedMotorState, string labelNodePath, string okText, string errorText, CircularSawTest.TestStep successStep, CircularSawTest.TestStep failureStep)
        {
            _master = master;
            _onButtonState = onButtonState;
            _offButtonState = offButtonState;
            _expectedMotorState = expectedMotorState;
            _labelNodePath = labelNodePath;
            _okText = okText;
            _errorText = errorText;
            _successStep = successStep;
            _failureStep = failureStep;
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Checks the step with the given parameters from the constructor
        /// </summary>
        /// <returns>The next step to do</returns>
        public CircularSawTest.TestStep Check(CircularSawTest.TestStep currentStep)
        {
            if (_testStepCounter < 30)
            {
                _master.Simulation.ONButtonState = _onButtonState;
                _master.Simulation.OFFButtonState = _offButtonState;
                _testStepCounter++;
            }
            else if (_testStepCounter < 60)
            {
                _master.Simulation.ONButtonState = false;
                _master.Simulation.OFFButtonState = false;
                if (_master.Simulation.CircularSawNode.MotorSwitchedOn != _expectedMotorState)
                {
                    _master.GetNode<Label>(_labelNodePath).Text += _errorText;
                    HadError = true;
                    return _failureStep;
                }
                _testStepCounter++;
            }
            else
            {
                _testStepCounter = 0;
                _master.GetNode<Label>(_labelNodePath).Text += _okText;
                return _successStep;
            }
            return currentStep;
        }
        #endregion
    }
}
