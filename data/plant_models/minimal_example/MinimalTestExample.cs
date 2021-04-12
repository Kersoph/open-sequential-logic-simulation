using System.IO;
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
        /// Initializes the whole sfc editor
        /// </summary>
        public override void InitialiseWith(MainNode mainNode, LessonEntity openedLesson)
        {
            _simulation = GetNode<MinimalSimulationExample>("MinimalSimulationExample");
            SfcEntity sfcEntity = new SfcEntity();
            string filepath = openedLesson.FolderPath + "/User/Diagram.sfc";
            if (TryLoadDiagram(filepath, sfcEntity))
            {
                _simulationMaster = new Master(sfcEntity, _simulation);
                _isExecutable = _simulationMaster.IsProgramSimulationValid();
            }
            else
            {
                _isExecutable = false;
            }
        }
        
        /// <summary>
        /// Loads the file into the sfc entity
        /// </summary>
        public bool TryLoadDiagram(string filepath, SfcEntity sfcEntity)
        {
            if (!File.Exists(filepath))
            {
                return false;
            }
            using (FileStream stream = File.Open(filepath, FileMode.OpenOrCreate))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    sfcEntity.ReadFrom(reader);
                }
            }
            return true;
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