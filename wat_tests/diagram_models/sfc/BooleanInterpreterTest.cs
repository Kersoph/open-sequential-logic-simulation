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
        [RunWith("NOT true", false)]
        [RunWith("!false", true)]
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
        [RunWith("2 = 2", true)]
        [RunWith("2 = 1", false)]
        public void RelationalOperation(string transition, bool result)
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            BooleanExpression expression = Interpreter.AsBooleanExpression(transition, pu);
            Assert.IsTrue(expression.IsValid());
            Assert.IsEqual(expression.Result(pu), result);
        }
        
        [Test]
        [RunWith("testBool", "testBool", true, "outputBoolTest", "outputBoolTest = true", true)]
        [RunWith("testBool", "invalidBool", false, "outputBoolTest", "outputBoolTest = 1", false)]
        [RunWith("testBool", "testBool and true", true, "outputBoolTest", "outputBoolTest", true)]
        public void BooleanIO(string iBKey, string bTest, bool iBValid, string iAKey, string assignment, bool iAValid)
        {
            var inputBoolKeys = new List<StateEntry<bool>>()
            {
                { new StateEntry<bool>(iBKey, true, "", "") }
            };
            var outputBoolKeys = new List<StateEntry<bool>>()
            {
                { new StateEntry<bool>(iAKey, true, "", "") }
            };
            var inputRegisters = new StateTable(inputBoolKeys, new List<StateEntry<int>>());
            var outputRegisters = new StateTable(outputBoolKeys, new List<StateEntry<int>>());
            ProcessingUnitMock pu = new ProcessingUnitMock(inputRegisters, outputRegisters);
            BooleanExpression expressionA = Interpreter.AsBooleanExpression(bTest, pu);
            if (iBValid)
            {
                Assert.IsTrue(expressionA.IsValid());
                Assert.IsEqual(expressionA.Result(pu), true);
            }
            else
            {
                Assert.IsTrue(expressionA == null || !expressionA.IsValid());
            }
            var ae = Osls.St.Assignment.Interpreter.AsAssignmentExpression(assignment, pu);
            if (iAValid)
            {
                Assert.IsTrue(ae.IsValid());
            }
            else
            {
                Assert.IsTrue(ae == null || !ae.IsValid());
            }
        }
        
        [Test]
        [RunWith("(true)", true)]
        [RunWith("not (false or true)", false)]
        [RunWith("(not false) or true", true)]
        [RunWith("true or (false and false)", true)]
        [RunWith("(true or false) and false", false)]
        [RunWith("((true or false) and (false)) or ((false))", false)]
        [RunWith("((1 > 0))", true)]
        [RunWith("((1 > 0) and (1 > 0))", true)]
        public void LogicalGrouping(string transition, bool result)
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            BooleanExpression expression = Interpreter.AsBooleanExpression(transition, pu);
            Assert.IsTrue(expression.IsValid());
            Assert.IsEqual(expression.Result(pu), result);
        }
        
        [Test]
        [RunWith("(true")]
        [RunWith("((true and false)")]
        [RunWith("true) false")]
        [RunWith("true) (false)")]
        [RunWith(")")]
        public void InvalidLogicalGrouping(string transition)
        {
            ProcessingUnitMock pu = new ProcessingUnitMock();
            BooleanExpression expression = Interpreter.AsBooleanExpression(transition, pu);
            Assert.IsFalse(expression.IsValid());
        }
    }
}