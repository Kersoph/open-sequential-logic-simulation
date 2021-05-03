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
        private LessonEntity _openedLesson;
        
        private RegularOperation _regularOperation;
        private StartOpen _startOpen;
        private StartPassing _startPassing;
        private NoiseAndBlackout _noiseAndBlackout;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole test viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(MainNode mainNode, LessonEntity openedLesson)
        {
            _openedLesson = openedLesson;
            string filepath = openedLesson.FolderPath + "/User/Diagram.sfc";
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
            
            _regularOperation = GetNode<RegularOperation>("Tests/RegularOperation");
            _regularOperation.InitialiseWith(sfcEntity);
            _startOpen = GetNode<StartOpen>("Tests/StartOpen");
            _startOpen.InitialiseWith(sfcEntity);
            _startPassing = GetNode<StartPassing>("Tests/StartPassing");
            _startPassing.InitialiseWith(sfcEntity);
            _noiseAndBlackout = GetNode<NoiseAndBlackout>("Tests/NoiseAndBlackout");
            _noiseAndBlackout.InitialiseWith(sfcEntity);
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
            if (_regularOperation.Result == 0)
            {
                _openedLesson.SetAndSaveStars(0);
            }
            else if (_startOpen.Result == 0 || _startPassing.Result == 0)
            {
                _openedLesson.SetAndSaveStars(1);
            }
            else if (_noiseAndBlackout.Result == 0)
            {
                _openedLesson.SetAndSaveStars(2);
            }
            else
            {
                _openedLesson.SetAndSaveStars(3);
            }
        }
        #endregion
    }
}