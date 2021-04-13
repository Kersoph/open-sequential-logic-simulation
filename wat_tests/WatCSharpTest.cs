using Godot;
using WAT;


namespace Tests.Example
{
    public class WatCSharpTest : Test
    {
        public override string Title()
        {
            return "Basic WAT cs tests as an example";
        }
        
        [Test]
        public void CSharpTestsWorking()
        {
            Assert.IsTrue(true);
        }
        
        [Test]
        public void WhenCallingIsEqual()
        {
            Assert.IsEqual(1, 1, "Then it passes");
        }
        
        [Test]
        public void IsTransform()
        {
            Assert.IsType<Transform>(new Transform());
        }
        
        [Test]
        public void WhenCallingHasMethodTitle()
        {
            Assert.ObjectHasMethod(this, "Title", "Then it passes");
        }
        
        [Test]
        public void WhenCallingDoesNotHaveUserSignal()
        {
            Assert.ObjectDoesNotHaveUserSignal(this, "FalseSignal", "Then it passes");
        }
        
        [Test]
        public void WhenCallingObjectIsQueuedForDeletionAfterCallingQueueFree()
        {
            Node N = new Node();
            N.QueueFree();
            Assert.ObjectIsQueuedForDeletion(N, "Then it passes");
        }
        
        [Test]
        [RunWith(2, 2, 4)]
        [RunWith(5, 3, 8)]
        [RunWith(7, 6, 13)]
        public void ParameterizedTestAdd(int Addend, int Augend, int Result)
        {
            Describe(string.Format("When we add {0} to {1} we get {2}", Addend, Augend, Result));
            Assert.IsEqual(Result, Addend + Augend);
        }
        
        [Test]
        public void WhenCallingIsInRange()
        {
            int val = 0;
            int low = 0;
            int high = 10;
            Assert.IsInRange(val, low, high, "Then it passes");
        }
    }
}
