using Godot;
using System.IO;

namespace SfcSandbox.Data.Model.SfcEditor
{
    /// <summary>
    /// Topmost node for the Sfc2dEditorNode.tscn
    /// </summary>
    public class Sfc2dEditorNode : Node
    {
        #region ==================== Fields Properties ====================
        public Sfc2dEditorControl Sfc2dEditorControl { get; private set; }
        
        private ReferenceRect _renderViewportReferenceRect;
        private int _zoomLevel = 3;
        private static readonly float[] zoomLevels = new float[] { 0.34f, 0.5f, 0.7f, 1f, 1.5f, 2f, 3f };
        private bool _isDragging;
        private Vector2 _lastDragPosition;
        #endregion


        #region ==================== Public ====================
        /// <summary>
        /// Creates a controller and initializes the patch fields.
        /// </summary>
        public void InitializeEditor()
        {
            _renderViewportReferenceRect = GetNode<ReferenceRect>("RenderViewportReferenceRect");
            Sfc2dEditorControl = new Sfc2dEditorControl(_renderViewportReferenceRect);
            // Todo: Later we will load the entity
            SfcPatchEntity entity = new SfcPatchEntity(1, 0);
            entity.SfcStepType = SfcStepType.StartingStep;
            Sfc2dEditorControl.CreatePatchAt(entity);
            Sfc2dEditorControl.UpdateGrid();
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
            using (FileStream stream = System.IO.File.Open(filepath, FileMode.OpenOrCreate))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    Sfc2dEditorControl.WriteTo(writer);
                }
            }
        }
        
        /// <summary>
        /// Loads the file and builds the SFC diagram if the file exists
        /// </summary>
        public bool TryLoadDiagram(string filepath)
        {
            if(!System.IO.File.Exists(filepath)) return false;
            using (FileStream stream = System.IO.File.Open(filepath, FileMode.OpenOrCreate))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    Sfc2dEditorControl.ReadFrom(reader);
                }
            }
            return true;
        }
        
        public void ZoomIn()
        {
            if (_zoomLevel + 1 < zoomLevels.Length)
            {
                _zoomLevel++;
                float scale = zoomLevels[_zoomLevel];
                ApplyDiagramScale(new Vector2(scale, scale));
            }
        }
        
        public void ZoomOut()
        {
            if (_zoomLevel > 0)
            {
                _zoomLevel--;
                float scale = zoomLevels[_zoomLevel];
                ApplyDiagramScale(new Vector2(scale, scale));
            }
        }
        
        public void ApplyDiagramScale(Vector2 scale)
        {
            _renderViewportReferenceRect.SetScale(scale);
        }
        
        public void ApplyDiagramOffset(Vector2 position)
        {
            _renderViewportReferenceRect.SetPosition(position);
        }
        #endregion
        
        
        #region ==================== Input Events ====================
        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_MiddleMouseButton"))
            {
                _lastDragPosition = GetViewport().GetMousePosition();
                _isDragging = true;
            }
            else if (@event.IsActionReleased("ui_MiddleMouseButton"))
            {
                _isDragging = false;
            }
        }
        #endregion
    }
}