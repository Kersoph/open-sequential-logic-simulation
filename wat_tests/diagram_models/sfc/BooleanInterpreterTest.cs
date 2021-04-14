using WAT;
using Osls.SfcEditor.Interpreters.Boolean;


namespace Tests.SfcEditor.Interpreters
{
    public class BooleanInterpreterTest : Test
    {
        public override string Title()
        {
            return "ST like interpreter test";
        }
        
        [Test]
        [RunWith("true", true)]
        [RunWith("false", false)]
        public void BooleanConstant(string transition, bool result)
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            BooleanExpression expression = Interpreter.AsBooleanExpression(transition, pu);
            Assert.IsTrue(expression.IsValid());
            Assert.IsEqual(expression.Result(pu), result);
        }
        
        [Test]
        [RunWith("true and true", true)]
        [RunWith("true and false", false)]
        [RunWith("false and false", false)]
        [RunWith("false and true", false)]
        [RunWith("true or true", true)]
        [RunWith("true or false", true)]
        [RunWith("false or false", false)]
        [RunWith("false or true", true)]
        public void LogicalCombination(string transition, bool result)
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            BooleanExpression expression = Interpreter.AsBooleanExpression(transition, pu);
            Assert.IsTrue(expression.IsValid());
            Assert.IsEqual(expression.Result(pu), result);
        }
        
        [Test]
        [RunWith("not true", false)]
        [RunWith("not false", true)]
        [RunWith("not not false", false)]
        [RunWith("not not not true", false)]
        public void LogicalInverter(string transition, bool result)
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            BooleanExpression expression = Interpreter.AsBooleanExpression(transition, pu);
            Assert.IsTrue(expression.IsValid());
            Assert.IsEqual(expression.Result(pu), result);
        }
        
        [Test]
        [RunWith("1 > 2", false)]
        [RunWith("1 < 2", true)]
        [RunWith("1 > 1", false)]
        [RunWith("1 < 1", false)]
        public void RelationalOperation(string transition, bool result)
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            BooleanExpression expression = Interpreter.AsBooleanExpression(transition, pu);
            Assert.IsTrue(expression.IsValid());
            Assert.IsEqual(expression.Result(pu), result);
        }
        
    }
}