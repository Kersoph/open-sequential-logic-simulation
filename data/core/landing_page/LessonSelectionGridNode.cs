using Godot;


namespace Osls.LandingPage
{
    /// <summary>
    /// Represents the LessonSelectionGridNode in the godot UI.
    /// It controls the lesson selection process.
    /// </summary>
    public class LessonSelectionGridNode : GridContainer
    {
        #region ==================== Fields ====================
        [Export] private NodePath LessonsNodePath = "../../../..";
        [Export] private PackedScene _lessonControllerScene;
        private LessonControllerNode[] _lessonNodes;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            try
            {
                ILessonEntity[] lessons = LessonCollector.LoadLessons();
                CreateButtons(lessons);
            }
            catch (System.Exception e)
            {
                // After around 48 loads the Godot JSON parser can fail
                // Its not clear atm why, but generally a user needs 2-3 loads so it is
                // regarded as OK for the Prototype.
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
            LessonsNode lessonsNode = GetNode<LessonsNode>(LessonsNodePath);
            lessonsNode.SelectionChangedTo(node.LessonEntity);
        }
        
        /// <summary>
        /// Informs the grid, that the selection has changed to this button and the UI needs to be updated.
        /// </summary>
        public void StartSelectedLesson(LessonControllerNode node)
        {
            LessonsNode lessonsNode = GetNode<LessonsNode>(LessonsNodePath);
            lessonsNode.StartSelectedLesson(node.LessonEntity);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void CreateButtons(ILessonEntity[] lessons)
        {
            _lessonNodes = new LessonControllerNode[lessons.Length];
            for (int i = 0; i < lessons.Length; i++)
            {
                LessonControllerNode controllerNode = (LessonControllerNode)_lessonControllerScene.Instance();
                controllerNode.SetLessonInfo(lessons[i], this);
                AddChild(controllerNode);
                _lessonNodes[i] = controllerNode;
            }
        }
        #endregion
    }
}