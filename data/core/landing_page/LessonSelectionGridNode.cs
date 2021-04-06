using Godot;
using Osls.Environment;


namespace Osls.LandingPage
{
    /// <summary>
    /// Represents the LessonSelectionGridNode in the godot UI.
    /// It controls the lesson selection process.
    /// </summary>
    public class LessonSelectionGridNode : GridContainer
    {
        #region ==================== Fields ====================
        private const string LessonControllerScene = "res://data/core/landing_page/LessonSourceController/LessonController.tscn";
        private LessonControllerNode[] _lessonNodes;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            try
            {
                LessonEntity[] lessons = LoadLessons();
                CreateButtons(lessons);
            }
            catch (System.Exception e)
            {
                // After around 48 loads the Godot JSON parser can fail
                // Its not clear atm why, but generally a user needs 2-3 loads so it is
                // regarded as OK for the Protorype.
                GD.PrintErr(e);
                GD.PushError(e.Message);
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Informs the grid, that the selection has changed to this button and the UI needs to be updated.
        /// </summary>
        public void SelectionChangedTo(LessonControllerNode node)
        {
            for (int i = 0; i < _lessonNodes.Length; i++)
            {
                if (_lessonNodes[i] != node)
                {
                    _lessonNodes[i].ResetButtonStatus();
                }
            }
            LessonsNode lessonsNode = GetNode<LessonsNode>("../../..");
            lessonsNode.SelectionChangedTo(node);
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        private LessonEntity[] LoadLessons()
        {
            string[] LessonPaths = System.IO.Directory.GetDirectories(EnvironmentPaths.LessonsFolderPath);
            LessonEntity[] lessonEntities = new LessonEntity[LessonPaths.Length];
            for (int i = 0; i < LessonPaths.Length; i++)
            {
                LessonEntity lessonData = new LessonEntity(LessonPaths[i]);
                lessonData.LoadFolderContent();
                lessonEntities[i] = lessonData;
            }
            return lessonEntities;
        }
        
        private void CreateButtons(LessonEntity[] lessons)
        {
            PackedScene packedControllerNode = (PackedScene)GD.Load(LessonControllerScene);
            _lessonNodes = new LessonControllerNode[lessons.Length];
            for (int i = 0; i < lessons.Length; i++)
            {
                LessonControllerNode controllerNode = (LessonControllerNode)packedControllerNode.Instance();
                controllerNode.SetLessonInfo(lessons[i], this);
                this.AddChild(controllerNode);
                _lessonNodes[i] = controllerNode;
            }
        }
        #endregion
    }
}