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
        }
        
        private bool AreTestsDone()
        {
            return _regularOperation.Result != -1;
        }
        
        private void CreateResult()
        {
            if (_regularOperation.Result == 0)
            {
                _openedLesson.SetAndSaveStars(0);
            }
            else
            {
                _openedLesson.SetAndSaveStars(1);
            }
        }
        #endregion
    }
}