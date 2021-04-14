using WAT;
using Osls.SfcEditor.Interpreters.Numerical;


namespace Tests.SfcEditor.Interpreters
{
    public class NumericalInterpreterTest : Test
    {
        public override string Title()
        {
            return "ST like interpreter test";
        }
        
        [Test]
        [RunWith("0", 0)]
        [RunWith("1", 1)]
        [RunWith("17", 17)]
        [RunWith("-1", -1)]
        public void NumericalConstant(string text, int result)
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            NumericalExpression expression = Interpreter.AsNumericalExpression(text, pu);
            Assert.IsTrue(expression.IsValid());
            Assert.IsEqual(expression.Result(pu), result);
        }
    }
}