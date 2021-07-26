using WAT;
using System.Collections.Generic;
using Osls.SfcEditor;


namespace Tests.SfcEditor.Interpreters
{
    public class StepTransitionTest : Test
    {
        public override string Title()
        {
            return "Basic SFC tests";
        }
        
        /// <summary>
        /// [S]
        /// -|- true
        /// [S]
        /// -|- false
        /// </summary>
        [Test]
        public void DirectTransition()
        {
            List<PatchEntity> stepEntities = new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    SfcStepType = StepType.StartingStep,
                    TransitionText = "true"
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            Assert.IsFalse(sfc.IsStepActive(0, 0), "Startup");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "Startup");
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 1), "2 Steps");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "3 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 1), "3 Steps");
        }
        
        /// <summary>
        /// [S]
        ///  |
        /// -|- true
        /// [S]
        /// -|- false
        /// </summary>
        [Test]
        public void PassStep()
        {
            List<PatchEntity> stepEntities= new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    SfcStepType = StepType.StartingStep,
                    TransitionText = "true"
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.Pass,
                },
                new PatchEntity(0, 2)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            Assert.IsFalse(sfc.IsStepActive(0, 0), "Startup");
            Assert.IsFalse(sfc.IsStepActive(0, 2), "Startup");
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 2), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 2), "2 Steps");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "3 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 2), "3 Steps");
        }
        
        /// <summary>
        /// [S]
        ///  ----
        ///    -|- true
        ///  ----
        /// [S]
        /// -|- false
        /// </summary>
        [Test]
        public void WeirdPath()
        {
            List<PatchEntity> stepEntities= new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    SfcStepType = StepType.StartingStep,
                    UpperBranch = BranchType.Single,
                    TransitionText = "true",
                    LowerBranch = BranchType.Single,
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            Assert.IsFalse(sfc.IsStepActive(0, 0), "Startup");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "Startup");
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 1), "2 Steps");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "3 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 1), "3 Steps");
        }
        
        /// <summary>
        /// [S]
        ///  ---- ----
        ///         -|- true
        ///         [S]
        ///         -|- false
        /// </summary>
        [Test]
        public void ExtraWeirdPath()
        {
            List<PatchEntity> stepEntities= new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    SfcStepType = StepType.StartingStep,
                    UpperBranch = BranchType.Single,
                },
                new PatchEntity(1, 0)
                {
                    UpperBranch = BranchType.Single,
                },
                new PatchEntity(2, 0)
                {
                    UpperBranch = BranchType.Single,
                    TransitionText = "true"
                },
                new PatchEntity(2, 1)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            Assert.IsFalse(sfc.IsStepActive(0, 0), "Startup");
            Assert.IsFalse(sfc.IsStepActive(2, 1), "Startup");
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(2, 1), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(2, 1), "2 Steps");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "3 Steps");
            Assert.IsTrue(sfc.IsStepActive(2, 1), "3 Steps");
        }
    }
}
