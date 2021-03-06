using WAT;


namespace Tests.PlantModels.MassTestChamber
{
    public class EmergencySystemStarTest : Test
    {
        public override string Title()
        {
            return "Testing the solutions of the emergency system";
        }
        
        [Test]
        public void FullThreeStars()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/EmergencySystemTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/EmergencySystemThreeStars.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 3, 4000);
        }
        
        [Test]
        public void OneStar()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/EmergencySystemTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/EmergencySystemOneStar.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 1, 4000);
        }
        
        [Test]
        public void TimedZeroStars()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/EmergencySystemTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/mass_test_chamber/EmergencySystemZeroStars.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 0, 4000);
        }
        
        [Test]
        public void EmptyZeroStars()
        {
            string scenePath = "res://data/plant_models/mass_test_chamber/EmergencySystemTest.tscn";
            string sfcPath = "res://wat_tests/plant_models/Empty.sfc";
            PlantModelRunner.Test(this, scenePath, sfcPath, 0, 4000);
        }
    }
}
