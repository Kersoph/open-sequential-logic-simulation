using Godot;

namespace Osls.Plants.RoadConstructionSite
{
    /// <summary>
    /// Topmost node for the DynamicCar.tscn
    /// </summary>
    public class DynamicCar : PathFollow
    {
        #region ==================== Fields Properties ====================
        private const float DriveSpeed = 10f;
        private const float AccidentResetTime = 40f;
        private static readonly string[] CarModels = new string[] {
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/highcar_black.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/highcar_blue.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/highcar_green.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/highcar_orange.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/highcar_red.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/highcar_yellow.tscn",
        };
        private static int LastCarModel;
        
        private float _currentAccidentResetTime;
        private DynamicCar _precursoryCar;
        private DynamicCar _followingCar;
        private TrafficControlSystem _trafficLight;
        
        public DynamicCarReport Report { get; private set; }
        #endregion
        
        
        #region ==================== Public ====================
        public override void _Ready()
        {
            Report = new DynamicCarReport();
            LastCarModel++;
            if (LastCarModel >= CarModels.Length) LastCarModel = 0;
            MeshInstance mesh = (MeshInstance)((PackedScene)GD.Load(CarModels[LastCarModel])).Instance();
            AddChild(mesh);
        }
        
        /// <summary>
        /// Called every simulation step to update the position
        /// </summary>
        public void UpdateProcess(float delta)
        {
            if (Report.HadAnAccident)
            {
                AccidentState(delta);
            }
            else
            {
                Drive(delta);
            }
        }
        
        /// <summary>
        /// Called if the detector is colliding with another detector.
        /// Can be called multiple times
        /// </summary>
        public void OnCollision()
        {
            if (Report.HadAnAccident) return;
            GetNode<Particles>("ExplosionParticles").Emitting = true;
            Report.HadAnAccident = true;
        }
        
        /// <summary>
        /// Sets the car to the start position and enables the collision detection
        /// </summary>
        public void StartDriving(DynamicCar precursoryCar, TrafficControlSystem trafficLight)
        {
            _precursoryCar = precursoryCar;
            if (_precursoryCar != null) precursoryCar._followingCar = this;
            _trafficLight = trafficLight;
            Offset = 0.001f; // 0 is not possible because godot only sets the position with a delta.
        }
        
        /// <summary>
        /// Called when this instance is removed from the simulation.
        /// When we remove the instance we set our precursory car to the following car as a precursory car.
        /// </summary>
        public void RemoveCar()
        {
            _trafficLight.Controller.CheckAndRemoveChild(this);
            if (_followingCar != null)
            {
                _followingCar._precursoryCar = _precursoryCar;
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void Drive(float delta)
        {
            bool precursoryCarDistanceEnsured = _precursoryCar == null || _precursoryCar.IsQueuedForDeletion() || _precursoryCar.Offset > Offset + 6f;
            bool TrafficLightOk = !_trafficLight.CheckStopSignal(Offset, 3f);
            if (precursoryCarDistanceEnsured && TrafficLightOk)
            {
                Offset += delta * DriveSpeed;
            }
            else
            {
                Report.WaitingCycles++;
            }
            if (UnitOffset > 0.98f) Report.SimulationCompleted = true;
        }
        
        private void AccidentState(float delta)
        {
            _currentAccidentResetTime += delta;
            if (_currentAccidentResetTime >= AccidentResetTime)
            {
                Report.SimulationCompleted = true;
            }
        }
        #endregion
    }
}