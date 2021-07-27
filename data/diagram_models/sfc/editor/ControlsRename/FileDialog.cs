using Godot;


namespace Osls.SfcEditor
{
    public class FileDialog : Godot.FileDialog
    {
        #region ==================== Fields / Properties ====================
        [Export] private NodePath ControllerPath = "..";
        protected EditorControls Controller { get { return GetNode<EditorControls>(ControllerPath); } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public override void _Ready()
        {
            Connect("file_selected", this, nameof(OnFileSelected));
        }
        
        /// <summary>
        /// Creates a dialog to load a sfc file
        /// </summary>
        public void LoadFile()
        {
            Mode = ModeEnum.OpenFile;
            CurrentDir = Controller.SfcEditorNode.OpenedLesson.FolderPath + LessonEntity.UserResultDirectory;
            PopupCentered();
        }
        
        /// <summary>
        /// Creates a dialog to save a sfc file
        /// </summary>
        public void SaveFile()
        {
            Mode = ModeEnum.SaveFile;
            CurrentDir = Controller.SfcEditorNode.OpenedLesson.FolderPath + LessonEntity.UserResultDirectory;
            PopupCentered();
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void OnFileSelected(string path)
        {
            ILessonEntity entity = Controller.SfcEditorNode.OpenedLesson;
            entity.CustomDiagramFilePath = ProjectSettings.GlobalizePath(path);
            if (Mode == ModeEnum.SaveFile)
            {
                Controller.SaveDiagram();
            }
            else
            {
                Controller.LoadDiagram();
            }
            Hide();
        }
        #endregion
    }
}
