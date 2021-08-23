namespace Osls.Plants
{
    /// <summary>
    /// Top node for the whole test view.
    /// We expect to be a child of the MainNode
    /// </summary>
    public abstract class TestPage : PageModule
    {
        #region ==================== Fields / Properties ====================
        private int _previousSimulationFrames = 1;
        
        /// <summary>
        /// Gets the scene page type
        /// </summary>
        public override PageCategory ScenePage { get { return PageCategory.Examination; } }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Determines the number of calculated simulation steps per frame to help
        /// users with slow computers to speed it up in a reasonable amount.
        /// 60+ ~ 1
        /// 30 ~ 2
        /// 20 ~ 3
        /// 13- ~ 4
        /// </summary>
        protected int LookupTargetSimulationCycles()
        {
            float fps = Godot.Engine.GetFramesPerSecond();
            switch (_previousSimulationFrames)
            {
                case 1:
                    if (fps < 40)
                    {
                        _previousSimulationFrames = 2;
                        return 2;
                    }
                    return 1;
                case 2:
                    if (fps > 50)
                    {
                        _previousSimulationFrames = 1;
                        return 1;
                    }
                    if (fps < 24)
                    {
                        _previousSimulationFrames = 3;
                        return 3;
                    }
                    return 2;
                case 3:
                    if (fps > 30)
                    {
                        _previousSimulationFrames = 2;
                        return 2;
                    }
                    if (fps < 12)
                    {
                        _previousSimulationFrames = 4;
                        return 4;
                    }
                    return 3;
                case 4:
                    if (fps > 17)
                    {
                        _previousSimulationFrames = 3;
                        return 3;
                    }
                    return 4;
            }
            _previousSimulationFrames = 1;
            return 1;
        }
        #endregion
    }
}
