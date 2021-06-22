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
    }
}
