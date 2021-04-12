namespace Osls.Plants.RoadConstructionSite
{
    public static class SpawnTimeGenerator
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// Must be a prime number to generate all numbers from 0 to SpawnTimeModulator
        /// </summary>
        private const int SpawnSeed = 17;
        
        /// <summary>
        /// Half the value is the expected value of the generator
        /// Must be a prime number to generate all numbers from 0 to SpawnTimeModulator
        /// </summary>
        private const int SpawnModulator = 73;
        
        /// <summary>
        /// Constant offset of the spawn time in seconds.
        /// For LastSpawnNumber from 0 to SpawnModulator the minimum is 1.
        /// </summary>
        private const float Offset = -0.5f;
        
        /// <summary>
        /// Lambda of the approximated probability density function.
        /// "Rate Parameter"
        /// </summary>
        private static float Lambda = 0.05f;
        
        /// <summary>
        /// Last pseudo random number
        /// </summary>
        private static int LastSpawnNumber = 0;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next spawn time of a car in update cycles by:
        /// 1. Get a deterministic number between 0 and SpawnTimeModulator with an uniform distribution.
        /// 2. Calculate the exponential waiting time with the generated number.
        /// 3. Add a minimal waiting time TimeOffset.
        /// </summary>
        public static int GetNextSpawnTime()
        {
            LastSpawnNumber = (LastSpawnNumber + SpawnSeed) % SpawnModulator;
            float spawnTime = Godot.Mathf.Exp(LastSpawnNumber * Lambda) + Offset;
            return (int)(spawnTime * 1000);
        }
        
        /// <summary>
        /// Sets a new lambda to the Exp([0..SpawnModulator] * Lambda) + Offset
        /// </summary>
        public static void SetLambda(float lambda)
        {
            Lambda = lambda;
        }

        /// <summary>
        /// Gets the current lambda to the Exp([0..SpawnModulator] * Lambda) + Offset
        /// </summary>
        public static float GetLambda()
        {
            return Lambda;
        }
        
        /// <summary>
        /// Resets the random generator to the initial vector
        /// </summary>
        public static void ResetGenerator()
        {
            LastSpawnNumber = 0;
            Lambda = 0.05f;
        }
        #endregion
    }
}