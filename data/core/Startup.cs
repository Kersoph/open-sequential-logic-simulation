using Godot;
using System;

namespace Osls
{
    public class Startup : Node
    {
        #region ==================== Fields / Properties ====================
        [Export] private PackedScene _mainScene;
        private int _initialisationStep;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Add initialisation steps to the loading scene util GetTree().ChangeSceneTo() is called.
        /// If it takes some time create a loading bar.
        /// </summary>
        public override void _Process(float delta)
        {
            switch (_initialisationStep)
            {
                case 0:
                    OS.WindowMaximized = true;
                    _initialisationStep++;
                    break;
                case 1:
                    GetTree().ChangeSceneTo(_mainScene);
                    break;
            }
        }
        #endregion
    }
}