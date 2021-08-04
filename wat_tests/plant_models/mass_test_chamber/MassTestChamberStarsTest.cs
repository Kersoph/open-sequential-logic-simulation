using WAT;


namespace Tests.PlantModels.MassTestChamber
{
    public class MassTestChamberStarsTest : Test
    {
        public override string Title()
        {
            return "Testing the solutions of the mass test chamber";
        }
        
        [Test]
        public void ThreeStars()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/MassTestChamberTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/MassTestChamberThreeStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 3, 3000);
        }
        
        [Test]
        public void TwoStars()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/MassTestChamberTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/MassTestChamberTwoStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 2, 3000);
        }
        
        [Test]
        public void OneStar()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/MassTestChamberTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/MassTestChamberOneStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 1, 3000);
        }
        
        [Test]
        public void ZeroStars()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/MassTestChamberTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/MassTestChamberRandom.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 0, 2000);
        }
    }
}
