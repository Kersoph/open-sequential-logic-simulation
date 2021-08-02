using Godot;
using Osls.SfcEditor;
using Osls.SfcSimulation.Viewer;


namespace Osls.Plants.MassTestChamber
{
    /// <summary>
    /// Contains the tests for this plant
    /// </summary>
    public class EmergencySystemTest : TestPage
    {
        #region ==================== Fields / Properties ====================
        private enum Stages { Startup, Test, CollectResults, Done, Idle };
        private Stages _stage = Stages.Startup;
        private ILessonEntity _openedLesson;
        [Export] private NodePath _emergencySystemPath = "ViewportContainer/Viewport/EmergencySystem";
        
        public EmergencySystem Simulation { get; private set; }
        public Master SimulationMaster { get; private set; }
        public bool IsExecutable { get; private set; }
        public PaperLog PaperLog { get; private set; }
        public EmergencySystemTestStages TestController { get; private set; }
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
            
            Simulation = GetNode<EmergencySystem>(_emergencySystemPath);
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
            TestController = new EmergencySystemTestStages();
            TestController.InitialiseWith(this);
        }
        
        public override void _Process(float delta)
        {
            switch (_stage)
            {
                case Stages.Startup:
                    if (!IsExecutable)
                    {
                        PaperLog.AppendError("SFC is invalid!\n");
                        _stage = Stages.CollectResults;
                    }
                    else
                    {
                        _stage = Stages.Test;
                    }
                    break;
                case Stages.Test:
                    TestController.TestStage();
                    if (TestController.Stage == EmergencySystemTestStages.Stages.Done) _stage = Stages.CollectResults;
                    break;
                case Stages.CollectResults:
                    CollectResults();
                    _stage = Stages.Done;
                    break;
                case Stages.Done:
                    GetNode<Viewport>("ViewportContainer/Viewport").RenderTargetUpdateMode = Viewport.UpdateMode.Once;
                    _stage = Stages.Idle;
                    break;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void CollectResults()
        {
            bool hadErrors = !IsExecutable
                || TestController.AlarmHornTest.ReportedEarlyActivation
                || TestController.AlarmHornTest.ReportedEarlyDeactivation
                || TestController.AlarmLightTest.ReportedEarlyActivation
                || TestController.AlarmLightTest.ReportedEarlyDeactivation;
            if (hadErrors)
            {
                PaperLog.Append("[b]Result: 0 Stars[/b]\n");
                _openedLesson.SetAndSaveStars(0);
            }
            else
            {
                PaperLog.Append("No problems found.\n");
                PaperLog.Append("[b]Result: 3 Stars![/b]\n");
                _openedLesson.SetAndSaveStars(3);
            }
        }
        #endregion
    }
}