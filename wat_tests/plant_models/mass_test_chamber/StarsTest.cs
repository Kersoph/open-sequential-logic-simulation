using WAT;


namespace Tests.PlantModels.MassTestChamber
{
    public class StarsTest : Test
    {
        public override string Title()
        {
            return "Testing the solutions of the mass test chamber";
        }
        
        [Test]
        public void ThreeStars()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/Test.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/ThreeStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 3, 3000);
        }
        
        [Test]
        public void TwoStars()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/Test.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/TwoStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 2, 3000);
        }
        
        [Test]
        public void OneStar()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/Test.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/OneStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 1, 3000);
        }
        
        [Test]
        public void ZeroStars()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/Test.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/Random.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 0, 2000);
        }
    }
}
