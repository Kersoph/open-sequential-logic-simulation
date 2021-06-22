using WAT;


namespace Tests.PlantModels.ElectricalBarrier
{
    public class StarsTest : Test
    {
        public override string Title()
        {
            return "Testing the solutions of the electrical barrier";
        }
        
        [Test]
        public void ThreeStars()
        {
            string scenePath = "res://data/plant_models/electrical_barrier/ElectricalBarrierTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/electrical_barrier/ThreeStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 3, 6000);
        }
        
        [Test]
        public void TwoStars()
        {
            string scenePath = "res://data/plant_models/electrical_barrier/ElectricalBarrierTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/electrical_barrier/TwoStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 2, 6000);
        }
        
        [Test]
        public void ZeroStars()
        {
            string scenePath = "res://data/plant_models/electrical_barrier/ElectricalBarrierTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/electrical_barrier/Empty.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 0, 6000);
        }
        
        [Test]
        public void LightsThreeStars()
        {
            string scenePath = "res://data/plant_models/electrical_barrier/lights/LightsTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/electrical_barrier/LightsThreeStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 3, 2900);
        }
        
        [Test]
        public void LightsOneStar()
        {
            string scenePath = "res://data/plant_models/electrical_barrier/lights/LightsTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/electrical_barrier/LightsOneStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 1, 2900);
        }
        
        [Test]
        public void LightsZeroStars()
        {
            string scenePath = "res://data/plant_models/electrical_barrier/lights/LightsTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/electrical_barrier/Empty.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 0, 6000);
        }
    }
}
