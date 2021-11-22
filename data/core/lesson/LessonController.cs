using Godot;


namespace Osls
{
    /// <summary>
    /// Control class to the active lesson entity.
    /// </summary>
    public class LessonController
    {
        #region ==================== Fields ====================
        private const string LandingPagePath = "res://data/core/landing_page/LandingPage.tscn";
        private const string EditorPath = "res://data/diagram_models/sfc/editor/SfcEditor.tscn";
        private const string SimulationPath = "res://data/diagram_models/sfc/processing_viewer/SfcSimulationViewer.tscn";
        private readonly MainNode _mainNode;
        private ILessonEntity _openedLesson;
        
        private PageModule _currentPage;
        #endregion
        
        
        #region ==================== Constructor ====================
        public LessonController(MainNode mainNode)
        {
            _mainNode = mainNode;
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Changes the main lesson to the given lesson.
        /// </summary>
        public void ApplyNewLesson(ILessonEntity lesson)
        {
            _openedLesson = lesson;
        }
        
        /// <summary>
        /// Requests a change of the current page to the new page.
        /// Used to provide the possibility for the user to save or cancel the action.
        /// </summary>
        public void UserRequestsChangeTo(PageCategory page)
        {
            if (_currentPage == null) return;
            _currentPage.OnUserRequestsChange(_mainNode, page);
        }
        
        /// <summary>
        /// Loads the page which can be applied as a child to the main view.
        /// </summary>
        public void ApplyPage(PageCategory page)
        {
            StoreOldScene();
            LoadNewScene(page);
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        private void StoreOldScene()
        {
            if (_currentPage != null)
            {
                _currentPage.QueueFree();
                _currentPage = null;
            }
        }
        
        private void LoadNewScene(PageCategory view)
        {
            PageModule scene = null;
            switch (view)
            {
                case PageCategory.LandingPage:
                    scene = ((PackedScene)GD.Load(LandingPagePath)).Instance() as PageModule;
                    break;
                case PageCategory.LogicEditor:
                    scene = ((PackedScene)GD.Load(EditorPath)).Instance() as PageModule;
                    break;
                case PageCategory.Simulation:
                    if (!string.IsNullOrEmpty(_openedLesson.SimulationPath))
                    {
                        scene = ((PackedScene)GD.Load(SimulationPath)).Instance() as PageModule;
                    }
                    break;
                case PageCategory.Examination:
                    if (!string.IsNullOrEmpty(_openedLesson.TestPath))
                    {
                        scene = ((PackedScene)GD.Load(_openedLesson.TestPath)).Instance() as PageModule;
                    }
                    break;
            }
            _currentPage = scene;
            if (_currentPage != null)
            {
                _currentPage.InitialiseWith(_mainNode, _openedLesson);
                _mainNode.AddChild(_currentPage);
                _mainNode.MoveChild(_currentPage, 0);
            }
        }
        #endregion
    }
}
