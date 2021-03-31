using Godot;
using SfcSandbox.Data.Model;

namespace SfcSandbox.Data.Main
{
    /// <summary>
    /// Top node connected to the main tree. It will be rendered to the main window.
    /// Handels the visibility and layers of the modules.
    /// </summary>
    public class MainNode : Node
    {
        #region ==================== Fields ====================
        private LessonController _lessonController;
        private NavigationSteps _navigationSteps;
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            _lessonController = new LessonController(this);
            _navigationSteps = GetNode<NavigationSteps>("NavigationSteps");
            ChangeEditorTo(EditorView.LandingPage);
        }
        
        public override void _Notification(int id)
        {
            if (id == MainLoop.NotificationWmQuitRequest)
            {
                QuitSfcSandbox();
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Opens up a new lesson and changes the view to the SfcEditor step.
        /// </summary>
        public void OpenNewLesson(LessonEntity lessonData)
        {
            _lessonController.ApplyNewLesson(lessonData);
            ChangeEditorTo(EditorView.SfcStep);
        }
        
        /// <summary>
        /// Requests a change of the current editor to the new editor.
        /// Used to privide the possibility for the user to save or cancel the action.
        /// </summary>
        public void UserRequestsChangeTo(EditorView editorView)
        {
            switch (editorView)
            {
                case EditorView.Exit:
                    QuitSfcSandbox();
                    break;
                default:
                    ChangeEditorTo(editorView);
                    break;
            }
        }
        
        /// <summary>
        /// Changes the main editor to the given view. Forced.
        /// </summary>
        public void ChangeEditorTo(EditorView editorView)
        {
            _lessonController.ChangeEditorTo(editorView);
            _navigationSteps.VisibleViewIs(editorView);
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        private void QuitSfcSandbox()
        {
            this.GetTree().Quit();
        }
        #endregion
    }
}