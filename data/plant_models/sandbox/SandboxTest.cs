using Osls.SfcEditor;
using Osls.SfcSimulation.Viewer;


namespace Osls.Plants.Sandbox
{
    /// <summary>
    /// Minimal example class for a test viewer.
    /// </summary>
    public class SandboxTest : TestPage
    {
        #region ==================== Fields / Properties ====================
        private bool _isExecutable;
        private Master _simulationMaster;
        private Sandbox _simulation;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole twat viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _simulation = GetNode<Sandbox>("Sandbox");
            string filepath = openedLesson.TemporaryDiagramFilePath;
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
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
        }
        
        public override void _Process(float delta)
        {
            if (_isExecutable)
            {
                // If timing is important (real hardware?) we might should use the
                // absolute system time instead of the relative delta time.
                _simulationMaster.UpdateSimulation(System.Convert.ToInt32(delta * 1000));
            }
        }
        #endregion
    }
}