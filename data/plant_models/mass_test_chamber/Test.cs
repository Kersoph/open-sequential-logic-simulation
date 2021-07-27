using Godot;
using Osls.SfcEditor;
using Osls.SfcSimulation.Viewer;


namespace Osls.Plants.MassTestChamber
{
    /// <summary>
    /// Contains the tests for this plant
    /// </summary>
    public class Test : TestPage
    {
        #region ==================== Fields / Properties ====================
        private enum Stages { ExecuteTests, DisplayResults, Done };
        private Stages _stage = Stages.ExecuteTests;
        private ILessonEntity _openedLesson;
        [Export] private NodePath _testChamberPath = "ViewportContainer/Viewport/MassTestChamber";
        
        public MassTestChamber Simulation { get; private set; }
        public Master SimulationMaster { get; private set; }
        public bool IsExecutable { get; private set; }
        public TestController TestController { get; private set; }
        public PaperLog PaperLog { get; private set; }
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
            
            Simulation = GetNode<MassTestChamber>(_testChamberPath);
            if (sfcEntity != null)
            {
                Simulation.InitialiseWith(mainNode, openedLesson);
                SimulationMaster = new Master(sfcEntity, Simulation);
                IsExecutable = SimulationMaster.IsProgramSimulationValid();
            }
            else
            {
                IsExecutable = false;
            }
            PaperLog = GetNode<PaperLog>("PaperLog");
            PaperLog.Setup();
            TestController = new TestController(this);
            TestController.Setup();
        }
        
        public override void _Process(float delta)
        {
            switch (_stage)
            {
                case Stages.ExecuteTests:
                    TestController.ExecuteProtocoll();
                    if (TestController.Stage == TestController.Stages.Done) _stage = Stages.DisplayResults;
                    break;
                case Stages.DisplayResults:
                    int stars = TestController.CreateResult();
                    SetResult(stars);
                    GetNode<Viewport>("ViewportContainer/Viewport").RenderTargetUpdateMode = Viewport.UpdateMode.Once;
                    _stage = Stages.Done;
                    break;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void SetResult(int stars)
        {
             _openedLesson.SetAndSaveStars(stars);
        }
        #endregion
    }
}