using Godot;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Topmost node for the Sfc2dEditorNode.tscn
    /// </summary>
    public class Sfc2dEditorNode : Control
    {
        #region ==================== Fields / Properties ====================
        private ReferenceRect _renderViewportReferenceRect;
        private int _zoomLevel = 1;
        private static readonly float[] zoomLevels = new float[] { 0.5f, 1f, 1.5f, 2f, 3f };
        private bool _isDragging;
        private Vector2 _lastDragPosition;
        private bool _markedToRestore;
        private Vector2 _currentScale;
        private Vector2 _currentPosition;
        
        public Sfc2dEditorControl Sfc2dEditorControl { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates a controller and initializes the patch fields.
        /// </summary>
        public void InitializeEditor(ProcessingData data, bool isEditable)
        {
            _renderViewportReferenceRect = GetNode<ReferenceRect>("RenderViewportReferenceRect");
            Sfc2dEditorControl = new Sfc2dEditorControl(_renderViewportReferenceRect, data, isEditable);
            Connect("resized", this, nameof(MarkForResizeOffsetRestore));
            _currentScale = new Vector2(1f, 1f);
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
            if (_markedToRestore) OnResizeOffsetRestore();
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
                StartDrag();
            }
            else if (@event.IsActionReleased("ui_translate"))
            {
                StopDrag();
            }
        }
        
        /// <summary>
        /// Using secondary move buttons only when they are not used for another control
        /// Generally used here for mouse interactions.
        /// </summary>
        public override void _GuiInput(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_translate_idle"))
            {
                StartDrag();
            }
            else if (@event.IsActionReleased("ui_translate_idle"))
            {
                StopDrag();
            }
            else if (@event.IsActionPressed("ui_left", true))
            {
                _renderViewportReferenceRect.RectPosition += new Vector2(50f, 0f);
            }
            else if (@event.IsActionPressed("ui_right", true))
            {
                _renderViewportReferenceRect.RectPosition += new Vector2(-50f, 0f);
            }
            else if (@event.IsActionPressed("ui_up", true))
            {
                _renderViewportReferenceRect.RectPosition += new Vector2(0f, 50f);
            }
            else if (@event.IsActionPressed("ui_down", true))
            {
                _renderViewportReferenceRect.RectPosition += new Vector2(0f, -50f);
            }
        }
        
        /// <summary>
        /// Called when an Godot.InputEvent hasn't been consumed by Godot.Node._Input(Godot.InputEvent)
        /// or any GUI. The input event propagates up through the node tree until a node consumes it.
        /// Generally used here for key interactions.
        /// </summary>
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_left", true))
            {
                _renderViewportReferenceRect.RectPosition += new Vector2(50f, 0f);
                GetTree().SetInputAsHandled();
            }
            else if (@event.IsActionPressed("ui_right", true))
            {
                _renderViewportReferenceRect.RectPosition += new Vector2(-50f, 0f);
                GetTree().SetInputAsHandled();
            }
            else if (@event.IsActionPressed("ui_up", true))
            {
                _renderViewportReferenceRect.RectPosition += new Vector2(0f, 50f);
                GetTree().SetInputAsHandled();
            }
            else if (@event.IsActionPressed("ui_down", true))
            {
                _renderViewportReferenceRect.RectPosition += new Vector2(0f, -50f);
                GetTree().SetInputAsHandled();
            }
            else if (@event.IsActionPressed("ui_home"))
            {
                _renderViewportReferenceRect.RectPosition = new Vector2(0f, 0f);
                GetTree().SetInputAsHandled();
            }
        }
        
        /// <summary>
        /// Called if the user wants to drag the editor
        /// </summary>
        public void StartDrag()
        {
            _lastDragPosition = GetViewport().GetMousePosition();
            _isDragging = true;
        }
        
        /// <summary>
        /// Called if the user stops to drag the editor
        /// </summary>
        public void StopDrag()
        {
            _isDragging = false;
        }
        
        /// <summary>
        /// Called when the RECT size changed and we have to restore the position and scale in the next process update.
        /// This is a workaround for this issue where pos/scale gets reset if the scroll container changes.
        /// </summary>
        public void MarkForResizeOffsetRestore()
        {
            _markedToRestore = true;
        }
        
        /// <summary>
        /// Called when the RECT size changed and we have to restore the position and scale.
        /// </summary>
        public void OnResizeOffsetRestore()
        {
            _markedToRestore = false;
            Vector2 currentScale = _currentScale;
            Vector2 currentPosition = _currentPosition;
            CallDeferred(nameof(ApplyDiagramScale), currentScale);
            CallDeferred(nameof(ApplyDiagramOffset), currentPosition);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void ApplyDiagramScale(Vector2 scale)
        {
            Vector2 oldPosition = _renderViewportReferenceRect.RectPosition;
            Vector2 oldScale = _renderViewportReferenceRect.RectScale;
            _renderViewportReferenceRect.RectScale = scale;
            _currentScale = scale;
            ApplyDiagramOffset(new Vector2((oldPosition.x * scale.x) / oldScale.x, (oldPosition.y * scale.y) / oldScale.y));
        }
        
        private void ApplyDiagramOffset(Vector2 position)
        {
            _renderViewportReferenceRect.RectPosition = position;
            _currentPosition = position;
        }
        #endregion
    }
}
