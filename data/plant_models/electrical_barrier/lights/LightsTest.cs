using Osls.SfcEditor;
using System.Text;


namespace Osls.Plants.ElectricalBarrier
{
    /// <summary>
    /// Minimal example class for a test viewer.
    /// </summary>
    public class LightsTest : TestPage
    {
        #region ==================== Fields / Properties ====================
        private enum Stages { Regular, TurnOffTime, CreateResult, DisplayResults };
        private Stages _stage = Stages.Regular;
        private ILessonEntity _openedLesson;
        
        private bool _isExecutable;
        private Master _simulationMaster;
        private Lights _simulation;
        private int _turnOffTime;
        private int _turnOffTimout;
        
        private bool _lightsOk = true;
        private bool _turnOffOk = false;
        private bool _turnOffClose = false;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole test viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _openedLesson = openedLesson;
            string filepath = _openedLesson.TemporaryDiagramFilePath;
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
            
            _simulation = GetNode<Lights>("Lights");
            if (sfcEntity != null)
            {
                _simulation.InitialiseWith(mainNode, openedLesson);
                _simulationMaster = new Master(sfcEntity, _simulation);
                _isExecutable = _simulationMaster.IsProgramSimulationValid();
            }
            else
            {
                _isExecutable = false;
            }
            if (!_isExecutable)
            {
                _stage = Stages.CreateResult;
            }
        }
        
        public override void _Process(float delta)
        {
            switch (_stage)
            {
                case Stages.Regular:
                    TestRegular();
                    break;
                case Stages.TurnOffTime:
                    TestTurnOff();
                    break;
                case Stages.CreateResult:
                    CreateResult();
                    break;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void TestRegular()
        {
            for (int i = 0; i < 10; i++)
            {
                _simulationMaster.UpdateSimulation(25);
                CheckLights();
            }
            if (_simulation.Subscene.Vehicle.TimesPassedTrack >= 3)
            {
                _simulation.ForgetsToTurnLightsOff = true;
                _stage = Stages.TurnOffTime;
            }
        }
        
        private void TestTurnOff()
        {
            for (int i = 0; i < 10; i++)
            {
                _simulationMaster.UpdateSimulation(25);
                if (_simulation.SimulationInput.PollBoolean(Lights.LightsKey)) _turnOffTime += 25;
                _turnOffTimout += 25;
                if (_simulation.Subscene.Vehicle.TimesPassedTrack >= 4)
                {
                    _simulation.Subscene.Vehicle.CarSpeed = 0;
                }
            }
            if (_turnOffTime > 29500 && _turnOffTime < 30500)
            {
                if (!_simulation.SimulationInput.PollBoolean(Lights.LightsKey))
                {
                    _turnOffOk = true;
                    CreateResult();
                    _stage = Stages.CreateResult;
                }
            }
            else if ((_turnOffTime > 20000 && _turnOffTime < 29000) || _turnOffTime > 31000 || _turnOffTimout > 40000)
            {
                if (!_simulation.SimulationInput.PollBoolean(Lights.LightsKey) || _turnOffTimout > 40000)
                {
                    if (_turnOffTime > 28000 && _turnOffTime < 32000) _turnOffClose = true;
                    CreateResult();
                    _stage = Stages.CreateResult;
                }
            }
        }
        
        private void CheckLights()
        {
            float carUnitOffset = _simulation.Subscene.Vehicle.CarUnitOffset;
            if (carUnitOffset > VehicleAgentController.EntersTunnelSoon + 0.01
            && carUnitOffset < VehicleAgentController.ExitsTunnel - 0.01)
            {
                if (!_simulation.SimulationInput.PollBoolean(Lights.LightsKey))
                {
                    _lightsOk = false;
                }
            }
        }
        
        private void CreateResult()
        {
            CreateText();
            SetStars();
            _stage = Stages.DisplayResults;
        }
        
        private void CreateText()
        {
            StringBuilder result = new StringBuilder();
            bool lightsBroken = _simulation.Subscene.TunnelLights.Broken;
            
            result.Append("Dear Mister or Madam.\n");
            if (_lightsOk && (_turnOffOk || _turnOffClose) && !lightsBroken)
            {
                result.Append("\nOn behalf of the whole Secret Bases GmbH, we would like to thank you for your excellent work at EX02.\n");
                if (_turnOffClose) result.Append("Just our guard reports a slightly incorrect turning off time of the lamps.\n");
                result.Append("We are looking forward to work together in our next project.\n");
            }
            else if (!_isExecutable)
            {
                result.Append("I was told we can test the installation at EX02 today [b]but nothing seems to work[/b]. The red ERROR-LED on the controller is on.\n\nI hope you can have a look soon and solve the problem.\n");
            }
            else
            {
                result.Append("I was testing your installation today at EX02.\n\n");
                if (!_lightsOk)
                {
                    result.Append("The lights are [b]not turning on and off[/b] as intended - which is the core of our collaboration. ");
                }
                if (!_turnOffOk)
                {
                    result.Append("The lights are [b]not turning off automatically[/b] after our defined time. The guard told me this is not working as intended. ");
                }
                if (lightsBroken)
                {
                    result.Append("The biggest issue is, that the lights seems to [b]flicker and break down[/b] extremely fast. ");
                }
                result.Append("I hope you can have a look soon and solve the problem.\n");
            }
            result.Append("\nYours faithfully\nMarco Burri");
            GetNode<Godot.RichTextLabel>("Panel/Text").BbcodeText = result.ToString();
            GetNode<Godot.Panel>("Panel").Visible = true;
        }
        
        private void SetStars()
        {
            bool lightsBroken = _simulation.Subscene.TunnelLights.Broken;
            if (_lightsOk && _turnOffOk && !lightsBroken)
            {
                _openedLesson.SetAndSaveStars(3);
            }
            else if (_lightsOk && (_turnOffOk || _turnOffClose))
            {
                _openedLesson.SetAndSaveStars(2);
            }
            else if (_lightsOk)
            {
                _openedLesson.SetAndSaveStars(1);
            }
            else
            {
                _openedLesson.SetAndSaveStars(0);
            }
        }
        #endregion
    }
}