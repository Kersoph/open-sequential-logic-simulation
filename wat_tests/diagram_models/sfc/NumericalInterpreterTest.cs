using WAT;
using Osls;
using Osls.St.Numerical;
using System.Collections.Generic;


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
        
        [Test]
        public void NumericalIO()
        {
            var intKeys = new List<StateEntry<int>>
            {
                { new StateEntry<int>("testInt", 1, "", "") }
            };
            var inputRegisters = new StateTable(new List<StateEntry<bool>>(), intKeys);
            var outputRegisters = new StateTable(new List<StateEntry<bool>>(), new List<StateEntry<int>>());
            ProcessingUnitMock pu = new ProcessingUnitMock(inputRegisters, outputRegisters);
            NumericalExpression expressionA = Interpreter.AsNumericalExpression("testInt", pu);
            Assert.IsTrue(expressionA.IsValid());
            Assert.IsEqual(expressionA.Result(pu), 1);
            NumericalExpression expressionB = Interpreter.AsNumericalExpression("invalidInt", pu);
            Assert.IsFalse(expressionB.IsValid());
        }
        
        [Test]
        public void NumericalVariables()
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            pu.IntLookup.Add("intVar", 5);
            NumericalExpression expressionA = Interpreter.AsNumericalExpression("intVar", pu);
            Assert.IsTrue(expressionA.IsValid());
            Assert.IsEqual(expressionA.Result(pu), 5);
            NumericalExpression expressionB = Interpreter.AsNumericalExpression("invalidVar", pu);
            Assert.IsFalse(expressionB.IsValid());
        }
    }
}
