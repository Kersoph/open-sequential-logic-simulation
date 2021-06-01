using Godot;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Topmost node for the Sfc2dEditorNode.tscn
    /// </summary>
    public class Sfc2dEditorNode : Control
    {
        #region ==================== Fields Properties ====================
        private ReferenceRect _renderViewportReferenceRect;
        private int _zoomLevel = 2;
        private static readonly float[] zoomLevels = new float[] { 0.5f, 0.7f, 1f, 1.5f, 2f, 3f };
        private bool _isDragging;
        private Vector2 _lastDragPosition;
        
        public Sfc2dEditorControl Sfc2dEditorControl { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates a controller and initializes the patch fields.
        /// </summary>
        public void InitializeEditor(ProcessingData data)
        {
            _renderViewportReferenceRect = GetNode<ReferenceRect>("RenderViewportReferenceRect");
            Sfc2dEditorControl = new Sfc2dEditorControl(_renderViewportReferenceRect, data);
        }
        
        public override void _Process(float delta)
        {
            if (_isDragging)
            {
                Vector2 currentMousePosition = GetViewport().GetMousePosition();
                Vector2 deltaPosition = currentMousePosition - _lastDragPosition;
                ApplyDiagramOffset(deltaPosition + _renderViewportReferenceRect.RectPosition);
                _lastDragPosition = currentMousePosition;
            }
        }
        
        /// <summary>
        /// Saves the SFC diagram to a file
        /// </summary>
        public void SaveDiagram(string filepath)
        {
            Sfc2dEditorControl.SaveDiagram(filepath);
        }
        
        /// <summary>
        /// Loads the file and builds the SFC diagram if the file exists
        /// Creates a default diagram if it could not be loaded
        /// </summary>
        public void TryLoadDiagram(string filepath)
        {
            Sfc2dEditorControl.LoadDiagramOrDefault(filepath);
        }
        
        /// <summary>
        /// Uses the next higher zoom level
        /// </summary>
        public void ZoomIn()
        {
            if (_zoomLevel + 1 < zoomLevels.Length)
            {
                _zoomLevel++;
                float scale = zoomLevels[_zoomLevel];
                ApplyDiagramScale(new Vector2(scale, scale));
            }
        }
        
        /// <summary>
        /// Uses the next lower zoom level
        /// </summary>
        public void ZoomOut()
        {
            if (_zoomLevel > 0)
            {
                _zoomLevel--;
                float scale = zoomLevels[_zoomLevel];
                ApplyDiagramScale(new Vector2(scale, scale));
            }
        }
        
        /// <summary>
        /// Moving with the middle mouse button always works.
        /// </summary>
        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_translate"))
            {
                _lastDragPosition = GetViewport().GetMousePosition();
                _isDragging = true;
            }
            else if (@event.IsActionReleased("ui_translate"))
            {
                _isDragging = false;
            }
        }
        
        /// <summary>
        /// Using secondary move buttons only when they are not used for another control
        /// </summary>
        public override void _GuiInput(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_translate_idle"))
            {
                _lastDragPosition = GetViewport().GetMousePosition();
                _isDragging = true;
            }
            else if (@event.IsActionReleased("ui_translate_idle"))
            {
                _isDragging = false;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void ApplyDiagramScale(Vector2 scale)
        {
            _renderViewportReferenceRect.RectScale = scale;
        }
        
        private void ApplyDiagramOffset(Vector2 position)
        {
            _renderViewportReferenceRect.SetPosition(position);
        }
        #endregion
    }
}