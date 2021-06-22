using Osls.SfcEditor;


namespace Osls.Plants.MinimalExample
{
    /// <summary>
    /// Minimal example class for a test viewer.
    /// </summary>
    public class MinimalTestExample : TestPage
    {
        #region ==================== Fields / Properties ====================
        private bool _isExecutable;
        private Master _simulationMaster;
        private MinimalSimulationExample _simulation;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole twat viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(MainNode mainNode, ILessonEntity openedLesson)
        {
            _simulation = GetNode<MinimalSimulationExample>("MinimalSimulationExample");
            string filepath = openedLesson.TemporaryDiagramFilePath;
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
            if (sfcEntity != null)
            {
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
                _simulationMaster.UpdateSimulation(16);
            }
        }
        #endregion
    }
}