using System.Collections.Generic;


namespace Osls.Plants.RoadConstructionSite
{
    public class ResultCollector
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// Number of vehicles with an accident
        /// </summary>
        public int HadAnAccident { get; private set; } =  0;
        
        /// <summary>
        /// With how many cycles the best car got through
        /// </summary>
        public int MinimumWaitingCycles { get; private set; } = 999999;
        
        /// <summary>
        /// Total waiting cycles of all cars combined
        /// </summary>
        public ulong TotalWaitingCycles { get; private set; } = 0;
        
        /// <summary>
        /// How long the slowest car had to wait
        /// </summary>
        public int MaximumWaitingCycles { get; private set; } = 0;
        
        /// <summary>
        /// How many cars passed
        /// </summary>
        public int CompletedSimulations { get; private set; } = 0;
        
        /// <summary>
        /// Average time in ms the cars had to wait
        /// </summary>
        /// <value></value>
        public int AverageWaitingMs { get; private set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public ResultCollector(List<DynamicCarReport> reports, int simulatedCycleTime)
        {
            CalculateResults(reports, simulatedCycleTime);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Calculates the metrices from the collected reports
        /// </summary>
        private void CalculateResults(List<DynamicCarReport> reports, int simulatedCycleTime)
        {
            for (int i = 0; i < reports.Count; i++)
            {
                if (reports[i].HadAnAccident)
                {
                    HadAnAccident++;
                }
                else
                {
                    int cycles = reports[i].WaitingCycles;
                    if (MaximumWaitingCycles < cycles) MaximumWaitingCycles = cycles;
                    if (reports[i].SimulationCompleted && MinimumWaitingCycles > cycles) MinimumWaitingCycles = cycles;
                    TotalWaitingCycles = checked(TotalWaitingCycles + (ulong)cycles);
                    CompletedSimulations++;
                }
            }
            AverageWaitingMs = (int)(TotalWaitingCycles / (ulong)CompletedSimulations) * simulatedCycleTime;
        }
        #endregion
    }
}
