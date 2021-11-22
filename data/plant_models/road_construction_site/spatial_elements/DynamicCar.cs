using Godot;
using Osls.Bubbles;


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
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/HighCarBlack.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/HighCarBlue.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/HighCarGreen.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/HighCarOrange.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/HighCarRed.tscn",
            "res://data/plant_models/road_construction_site/spatial_elements/meshes/HighCarYellow.tscn",
        };
        private static int LastCarModel;
        
        private float _currentAccidentResetTime;
        private DynamicCar _precursoryCar;
        private DynamicCar _followingCar;
        private TrafficControlSystem _trafficLight;
        private BubbleSprite _bubble;
        
        public DynamicCarReport Report { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public void Initialise()
        {
            Report = new DynamicCarReport();
            LastCarModel++;
            if (LastCarModel >= CarModels.Length) LastCarModel = 0;
            MeshInstance mesh = (MeshInstance)((PackedScene)GD.Load(CarModels[LastCarModel])).Instance();
            AddChild(mesh);
            _bubble = GetNode<BubbleSprite>("BubbleSprite");
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
            _bubble.ShowAs(BubbleSprite.Bubble.Shout, BubbleSprite.Expression.Surprised, 3, true);
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
                if (Report.WaitingCycles == 2000) _bubble.ShowAs(BubbleSprite.Bubble.Think, BubbleSprite.Expression.Waiting, 1);
                if (Report.WaitingCycles == 2200) _bubble.ShowAs(BubbleSprite.Bubble.Think, BubbleSprite.Expression.Sad, 1);
                if (Report.WaitingCycles == 4000) _bubble.ShowAs(BubbleSprite.Bubble.Think, BubbleSprite.Expression.Frustrated, 1);
                if (Report.WaitingCycles == 5700) _bubble.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Angry, 1);
                if (Report.WaitingCycles == 5000) _bubble.ShowAs(BubbleSprite.Bubble.Shout, BubbleSprite.Expression.Frustrated, 1);
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
