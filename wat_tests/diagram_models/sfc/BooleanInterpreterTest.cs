using WAT;
using Osls.St.Boolean;
using Osls;
using System.Collections.Generic;


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
        [RunWith("True", true)]
        [RunWith("False", false)]
        [RunWith("TRUE", true)]
        [RunWith("FALSE", false)]
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
        [RunWith("False OR true", true)]
        [RunWith("FALSE And TRUE", false)]
        [RunWith("true and True And TRUE AND true", true)]
        [RunWith("true and True And TRUE AND false", false)]
        [RunWith("false or False Or FALSE OR false", false)]
        [RunWith("false or False Or TRUE OR false", true)]
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
        [RunWith("1 >= 1", true)]
        [RunWith("1 <= 1", true)]
        [RunWith("1 == 1", true)]
        [RunWith("1 >= 2", false)]
        [RunWith("2 <= 1", false)]
        [RunWith("2 == 1", false)]
        [RunWith("1 < 1 or 1 > 1", false)]
        [RunWith("2 < 1 or 2 > 1", true)]
        [RunWith("1 < 2 and 1 == 1 and true and not 1 == 2", true)]
        public void RelationalOperation(string transition, bool result)
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            BooleanExpression expression = Interpreter.AsBooleanExpression(transition, pu);
            Assert.IsTrue(expression.IsValid());
            Assert.IsEqual(expression.Result(pu), result);
        }
        
        [Test]
        public void BooleanIO()
        {
            var boolKeys = new Dictionary<string, bool>
            {
                { "testBool", true }
            };
            var inputRegisters = new StateTable(boolKeys, new Dictionary<string, int>());
            var outputRegisters = new StateTable(new Dictionary<string, bool>(), new Dictionary<string, int>());
            ProcessingUnitMock pu = new ProcessingUnitMock(inputRegisters, outputRegisters);
            BooleanExpression expressionA = Interpreter.AsBooleanExpression("testBool", pu);
            Assert.IsTrue(expressionA.IsValid());
            Assert.IsEqual(expressionA.Result(pu), true);
            BooleanExpression expressionB = Interpreter.AsBooleanExpression("invalidBool", pu);
            Assert.IsTrue(expressionB == null || !expressionB.IsValid());
        }
    }
}