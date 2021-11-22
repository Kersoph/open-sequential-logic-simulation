using WAT;


namespace Tests.PlantModels.CircularSaw
{
    public class StarsTest : Test
    {
        public override string Title()
        {
            return "Testing the solutions of the circular saw";
        }
        
        [Test]
        public void ThreeStars()
        {
            string scenePath = "res://data/plant_models/circular_saw/CircularSawTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/circular_saw/ThreeStars.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 3, 200);
        }
        
        [Test]
        public void TwoStars()
        {
            string scenePath = "res://data/plant_models/circular_saw/CircularSawTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/circular_saw/TwoStars.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 2, 200);
        }
        
        [Test]
        public void ZeroStars()
        {
            string scenePath = "res://data/plant_models/circular_saw/CircularSawTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/circular_saw/ZeroStars.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 0, 200);
        }
        
        [Test]
        public void EmptyTest()
        {
            string scenePath = "res://data/plant_models/circular_saw/CircularSawTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/Empty.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 0, 200);
        }
    }
}
