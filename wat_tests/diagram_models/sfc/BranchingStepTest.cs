using WAT;
using System.Collections.Generic;
using Osls.SfcEditor;


namespace Tests.SfcEditor.Interpreters
{
    public class BranchingStepTest : Test
    {
        public override string Title()
        {
            return "Branching SFC tests";
        }
        
        /// <summary>
        /// [S]
        ///  --------------------
        /// -|- false          -|- true
        /// [S]                [S]
        /// -|- false          -|- false
        /// </summary>
        [Test]
        public void RightBranchActive()
        {
            List<PatchEntity> stepEntities = new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    SfcStepType = StepType.StartingStep,
                    UpperBranch = BranchType.Single,
                    TransitionText = "false"
                },
                new PatchEntity(1, 0)
                {
                    TransitionText = "true"
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
                new PatchEntity(1, 1)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            Assert.IsFalse(sfc.IsStepActive(0, 0), "Startup");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "Startup");
            Assert.IsFalse(sfc.IsStepActive(1, 1), "Startup");
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(1, 1), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "2 Steps");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(1, 1), "2 Steps");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "3 Steps");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "3 Steps");
            Assert.IsTrue(sfc.IsStepActive(1, 1), "3 Steps");
        }
        
        /// <summary>
        ///                    [S]
        ///  --------------------
        /// -|- true           -|- false
        /// [S]                [S]
        /// -|- false          -|- false
        /// </summary>
        [Test]
        public void LeftBranchActive()
        {
            List<PatchEntity> stepEntities = new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    UpperBranch = BranchType.Single,
                    TransitionText = "true"
                },
                new PatchEntity(1, 0)
                {
                    SfcStepType = StepType.StartingStep,
                    TransitionText = "false"
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
                new PatchEntity(1, 1)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            Assert.IsFalse(sfc.IsStepActive(1, 0), "Startup");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "Startup");
            Assert.IsFalse(sfc.IsStepActive(1, 1), "Startup");
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(1, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(1, 1), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(1, 0), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 1), "2 Steps");
            Assert.IsFalse(sfc.IsStepActive(1, 1), "2 Steps");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(1, 0), "3 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 1), "3 Steps");
            Assert.IsFalse(sfc.IsStepActive(1, 1), "3 Steps");
        }
    }
}
