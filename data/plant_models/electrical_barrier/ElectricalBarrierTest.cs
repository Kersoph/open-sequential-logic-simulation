using Osls.SfcEditor;


namespace Osls.Plants.ElectricalBarrier
{
    /// <summary>
    /// Minimal example class for a test viewer.
    /// </summary>
    public class ElectricalBarrierTest : TestPage
    {
        #region ==================== Fields / Properties ====================
        private enum Stages { ExecuteTests, DisplayResults };
        private Stages _stage = Stages.ExecuteTests;
        private ILessonEntity _openedLesson;
        [Godot.Export] private Godot.NodePath _resultLabelPath = "Stars/Result";
        [Godot.Export] private Godot.NodePath _starPath = "Stars";
        
        private RegularOperation _regularOperation;
        private StartOpen _startOpen;
        private StartPassing _startPassing;
        private NoiseAndBlackout _noiseAndBlackout;
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
            
            _regularOperation = GetNode<RegularOperation>("Tests/RegularOperation");
            _regularOperation.InitialiseWith(mainNode, openedLesson, sfcEntity);
            _startOpen = GetNode<StartOpen>("Tests/StartOpen");
            _startOpen.InitialiseWith(mainNode, openedLesson, sfcEntity);
            _startPassing = GetNode<StartPassing>("Tests/StartPassing");
            _startPassing.InitialiseWith(mainNode, openedLesson, sfcEntity);
            _noiseAndBlackout = GetNode<NoiseAndBlackout>("Tests/NoiseAndBlackout");
            _noiseAndBlackout.InitialiseWith(mainNode, openedLesson, sfcEntity);
        }
        
        public override void _Process(float delta)
        {
            switch (_stage)
            {
                case Stages.ExecuteTests:
                    UpdateTests();
                    if (AreTestsDone())
                    {
                        CreateResult();
                        _stage = Stages.DisplayResults;
                    }
                    break;
                case Stages.DisplayResults:
                    break;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void UpdateTests()
        {
            if (_regularOperation.Result == -1) _regularOperation.UpdateStep();
            if (_startOpen.Result == -1) _startOpen.UpdateStep();
            if (_startPassing.Result == -1) _startPassing.UpdateStep();
            if (_noiseAndBlackout.Result == -1) _noiseAndBlackout.UpdateStep();
        }
        
        private bool AreTestsDone()
        {
            return _regularOperation.Result != -1
                && _startOpen.Result != -1
                && _startPassing.Result != -1
                && _noiseAndBlackout.Result != -1;
        }
        
        private void CreateResult()
        {
            GetNode<Godot.Control>(_starPath).Visible = true;
            Godot.Label resultLabel = GetNode<Godot.Label>(_resultLabelPath);
            if (_regularOperation.Result == 0)
            {
                _openedLesson.SetAndSaveStars(0);
                resultLabel.Text = "0 Stars";
            }
            else if (_startOpen.Result == 0 || _startPassing.Result == 0)
            {
                _openedLesson.SetAndSaveStars(1);
                resultLabel.Text = "1 Star";
            }
            else if (_noiseAndBlackout.Result == 0)
            {
                _openedLesson.SetAndSaveStars(2);
                resultLabel.Text = "2 Stars!";
            }
            else
            {
                _openedLesson.SetAndSaveStars(3);
                resultLabel.Text = "3 Stars! Nice!";
            }
        }
        #endregion
    }
}