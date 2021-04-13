namespace Osls.Plants.RoadConstructionSite
{
    public class DynamicCarReport
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// True if this car has finished its simulation purpose
        /// </summary>
        public bool SimulationCompleted { get; set; }
        
        /// <summary>
        /// True if this car had an accident
        /// </summary>
        public bool HadAnAccident { get; set; }
        
        /// <summary>
        /// Number of waiting cycles
        /// </summary>
        public int WaitingCycles { get; set; }
        #endregion
    }
}