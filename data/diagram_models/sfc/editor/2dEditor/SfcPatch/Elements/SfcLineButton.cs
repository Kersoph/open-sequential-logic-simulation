using Godot;

namespace Osls.SfcEditor
{
    /// <summary>
    /// Topmost node for the SFC line.
    /// Controls the visual apperance of the line.
    /// </summary>
    public class SfcLineButton : TextureButton
    {
        #region ==================== Fields ====================
        [Export] private Texture _singleLineTexture;
        [Export] private Texture _doubleLineTexture;
        private static Color VisibleColor = new Color(1f, 1f, 1f, 1f);
        private static Color TiltColor = new Color(1f, 1f, 1f, 0.4f);
        private static Color HiddenColor = new Color(1f, 1f, 1f, 0.0f);
        
        /// <summary>
        /// True, if it is the top line. Otherwise it is the bottom branch/merge
        /// </summary>
        [Export] public bool TopLine;
        private BranchType _sfcLineType = BranchType.Unused;
        private TextureRect _sfcLineTextureNode;
        #endregion
        
        
        #region ==================== Properties ====================
        private TextureRect SfcLineTextureNode
        {
            get
            {
                if (_sfcLineTextureNode == null) _sfcLineTextureNode = GetNode<TextureRect>("SfcLineTexture");
                return _sfcLineTextureNode;
            }
        }
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            Connect("mouse_entered", this, nameof(OnMouseEntered));
            Connect("mouse_exited", this, nameof(OnMouseExited));
            
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called when the button is pressed.
        /// </summary>
        public override void _Pressed()
        {
            if (TopLine)
            {
                GetNode<SfcPatchNode>("..").SfcPatchControl.TopLineToggled();
            }
            else
            {
                GetNode<SfcPatchNode>("..").SfcPatchControl.BotLineToggled();
            }
        }
        
        /// <summary>
        /// Changes the sfc line type to the given type.
        /// </summary>
        public void UpdateBranchLine(BranchType type)
        {
            if (_sfcLineType != type)
            {
                if (type != BranchType.Unused)
                {
                    SetLineTexture(type);
                }
                _sfcLineType = type;
                UpdateLineVisibility();
            }
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        /// <summary>
        /// Called when a mouse enters the rect
        /// </summary>
        private void OnMouseEntered()
        {
            SfcLineTextureNode.SelfModulate = TiltColor;
        }
        
        /// <summary>
        /// Called when a mouse leaves the rect
        /// </summary>
        private void OnMouseExited()
        {
            UpdateLineVisibility();
        }
        
        private void UpdateLineVisibility()
        {
            if (_sfcLineType == BranchType.Unused)
            {
                SfcLineTextureNode.SelfModulate = HiddenColor;
            }
            else
            {
                SfcLineTextureNode.SelfModulate = VisibleColor;
            }
        }
        
        private void SetLineTexture(BranchType sfcLineType)
        {
            if (sfcLineType == BranchType.Single)
            {
                SfcLineTextureNode.Texture = _singleLineTexture;
            }
            else if (sfcLineType == BranchType.Double)
            {
                SfcLineTextureNode.Texture = _doubleLineTexture;
            }
            else
            {
                SfcLineTextureNode.Texture = null;
            }
        }
        #endregion
    }
}