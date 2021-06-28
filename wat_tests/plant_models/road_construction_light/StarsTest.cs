using WAT;


namespace Tests.PlantModels.RoadConstructionSite
{
    public class StarsTest : Test
    {
        public override string Title()
        {
            return "Testing the solutions of the road construction site";
        }
        
        [Test]
        public void ThreeStars()
        {
            string scenePath = "res://data/plant_models/road_construction_site/RoadConstructionSiteTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/road_construction_light/Diagram_Full.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 3, 1000);
        }
        
        [Test]
        public void TwoStars()
        {
            string scenePath = "res://data/plant_models/road_construction_site/RoadConstructionSiteTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/road_construction_light/Diagram_Sensor.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 2, 1000);
        }
        
        [Test]
        public void OneStar()
        {
            string scenePath = "res://data/plant_models/road_construction_site/RoadConstructionSiteTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/road_construction_light/Diagram_Timed.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 1, 1000);
        }
        
        [Test]
        public void ZeroStars()
        {
            string scenePath = "res://data/plant_models/road_construction_site/RoadConstructionSiteTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/Empty.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 0, 1000);
        }
    }
}
