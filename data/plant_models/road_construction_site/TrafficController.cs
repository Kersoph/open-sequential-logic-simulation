using Godot;
using System.Collections.Generic;


namespace Osls.Plants.RoadConstructionSite
{
    /// <summary>
    /// Controls the cars in the simulation
    /// </summary>
    public class TrafficController
    {
        #region ==================== Fields / Properties ====================
        private const string CarReference = "res://data/plant_models/road_construction_site/spatial_elements/dynamic_car.tscn";

        private readonly PathController _topPath;
        private readonly PathController _botPath;
        private readonly List<DynamicCar> _activeCars;
        private readonly List<DynamicCar> _carsToCollect;
        private readonly List<DynamicCarReport> _reports;
        private int _nextTopCarSpawn;
        private int _nextBotCarSpawn;
        #endregion
        
        
        #region ==================== Constructor ====================
        public TrafficController(PathController topPath, PathController botPath)
        {
            _topPath = topPath;
            _botPath = botPath;
            _activeCars = new List<DynamicCar>();
            _carsToCollect = new List<DynamicCar>();
            _reports = new List<DynamicCarReport>();
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Called every simulation step to update the traffic
        /// </summary>
        public void CalculateNextStep(int deltaTimeMs)
        {
            _nextTopCarSpawn = CheckNextSpawnTime(_topPath, _nextTopCarSpawn - deltaTimeMs);
            _nextBotCarSpawn = CheckNextSpawnTime(_botPath, _nextBotCarSpawn - deltaTimeMs);
            for (int i = 0; i < _activeCars.Count; i++)
            {
                _activeCars[i].UpdateProcess(deltaTimeMs * 0.001f);
                if (_activeCars[i].Report.SimulationCompleted) _carsToCollect.Add(_activeCars[i]);
            }
            CollectCompletedEntities();
            CalculateCollisions();
        }
        
        /// <summary>
        /// Returns the collected Reports for the simulated cars
        /// </summary>
        public List<DynamicCarReport> CollectReports()
        {
            List<DynamicCarReport> allReports = new List<DynamicCarReport>(_reports);
            for (int i = 0; i < _activeCars.Count; i++)
            {
                allReports.Add(_activeCars[i].Report);
            }
            return allReports;
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private int CheckNextSpawnTime(PathController referencePath, int time)
        {
            if (time <= 0)
            {
                time = SpawnTimeGenerator.GetNextSpawnTime();
                if (referencePath.SpawnPossible())
                {
                    DynamicCar car = (DynamicCar)((PackedScene)GD.Load(CarReference)).Instance();
                    referencePath.AddChildAndStart(car);
                    _activeCars.Add(car);
                }
            }
            return time;
        }
        
        private void CollectCompletedEntities()
        {
            for (int i = 0; i < _carsToCollect.Count; i++)
            {
                DynamicCar car = _carsToCollect[i];
                _reports.Add(car.Report);
                _activeCars.Remove(car);
                car.RemoveCar();
                car.QueueFree();
            }
            _carsToCollect.Clear();
        }
        
        /// <summary>
        /// Calculates the collision matrix and checks the distances
        /// </summary>
        private void CalculateCollisions()
        {
            int carCount = _activeCars.Count;
            for (int i = 0; i < carCount; i++)
            {
                for (int j = i + 1; j < carCount; j++)
                {
                    float distance = _activeCars[i].Translation.DistanceSquaredTo(_activeCars[j].Translation);
                    if (distance < 9) // 3m
                    {
                        _activeCars[i].OnCollision();
                        _activeCars[j].OnCollision();
                    }
                }
            }
        }
        #endregion
    }
}