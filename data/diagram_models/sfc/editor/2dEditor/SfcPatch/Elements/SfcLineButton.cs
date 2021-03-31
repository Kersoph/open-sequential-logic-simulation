using Godot;

namespace SfcSandbox.Data.Model.SfcEditor
{
    /// <summary>
    /// Topmost node for the SFC line.
    /// Controls the visual apperance of the line.
    /// </summary>
    public class SfcLineButton : TextureButton
    {
        #region ==================== Fields ====================
        private const string SingleLineText = "res://Data/Model/SfcEditor/2dEditor/SfcPatch/Elements/SfcLine.png";
        private const string DoubleLineText = "res://Data/Model/SfcEditor/2dEditor/SfcPatch/Elements/SfcLineDouble.png";
        private static Color VisibleColor = new Color(1f, 1f, 1f, 1f);
        private static Color TiltColor = new Color(1f, 1f, 1f, 0.4f);
        private static Color HiddenColor = new Color(1f, 1f, 1f, 0.0f);
        
        /// <summary>
        /// True, if it is the top line. Otherwise it is the bottom branch/merge
        /// </summary>
        [Export] public bool TopLine;
        private SfcBranchLineType _sfcLineType = SfcBranchLineType.Unused;
        private TextureRect _sfcLineTextureNode;
        #endregion
        
        
        #region ==================== Properties ====================
        private TextureRect SfcLineTextureNode
        {
            get
            {
                if (_sfcLineTextureNode == null) _sfcLineTextureNode = this.GetNode<TextureRect>("SfcLineTexture");
                return _sfcLineTextureNode;
            }
        }
        #endregion
        
        
        #region ==================== Updates ====================
        public override void _Ready()
        {
            this.Connect("mouse_entered", this, nameof(MouseEntered));
            this.Connect("mouse_exited", this, nameof(MouseExited));
            
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
        public void UpdateBranchLine(SfcBranchLineType type)
        {
            if (_sfcLineType != type)
            {
                if (type != SfcBranchLineType.Unused)
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
        private void MouseEntered()
        {
            SfcLineTextureNode.SelfModulate = TiltColor;
        }
        
        /// <summary>
        /// Called when a mouse leaves the rect
        /// </summary>
        private void MouseExited()
        {
            UpdateLineVisibility();
        }
        
        private void UpdateLineVisibility()
        {
            if (_sfcLineType == SfcBranchLineType.Unused)
            {
                SfcLineTextureNode.SelfModulate = HiddenColor;
            }
            else
            {
                SfcLineTextureNode.SelfModulate = VisibleColor;
            }
        }
        
        private void SetLineTexture(SfcBranchLineType sfcLineType)
        {
            Texture texture;
            if (sfcLineType == SfcBranchLineType.Single)
            {
                texture = GD.Load<Texture>(SingleLineText);
            }
            else
            {
                texture = GD.Load<Texture>(DoubleLineText);
            }
            SfcLineTextureNode.Texture = texture;
        }
        #endregion
    }
}