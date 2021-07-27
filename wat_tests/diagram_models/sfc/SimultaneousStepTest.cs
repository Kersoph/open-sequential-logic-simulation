using WAT;
using System.Collections.Generic;
using Osls.SfcEditor;


namespace Tests.SfcEditor.Interpreters
{
    public class SimultaneousStepTest : Test
    {
        public override string Title()
        {
            return "Simultaneous SFC tests";
        }
        
        /// <summary>
        /// [S]
        /// -|- true
        ///  ====================
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
                    LowerBranch = BranchType.Double,
                    TransitionText = "true"
                },
                new PatchEntity(1, 0)
                {
                    LowerBranch = BranchType.Double,
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
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(1, 1), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 1), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(1, 1), "2 Steps");
        }
        
        /// <summary>
        ///                    [S]
        ///                    -|- true
        ///  ========================================
        ///  |                                      |
        ///  |                                      |
        /// [S]                                    [S]
        /// -|- false                              -|- false
        /// </summary>
        [Test]
        public void LeftBranchActive()
        {
            List<PatchEntity> stepEntities = new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    LowerBranch = BranchType.Double,
                },
                new PatchEntity(1, 0)
                {
                    SfcStepType = StepType.StartingStep,
                    TransitionText = "true",
                    LowerBranch = BranchType.Double,
                },
                new PatchEntity(2, 0)
                {
                    LowerBranch = BranchType.Double,
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.Pass,
                },
                new PatchEntity(2, 1)
                {
                    SfcStepType = StepType.Pass,
                },
                new PatchEntity(0, 2)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
                new PatchEntity(2, 2)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(1, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 2), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(2, 2), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(1, 0), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 2), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(2, 2), "2 Steps");
            sfc.UpdateStep();
        }
        
        /// <summary>
        /// [S]               [S]
        ///  ===================
        /// -|- true
        /// [S]
        /// -|- false
        /// </summary>
        [Test]
        public void SimultaneousMerge()
        {
            List<PatchEntity> stepEntities = new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    SfcStepType = StepType.StartingStep,
                    UpperBranch = BranchType.Double,
                    TransitionText = "true"
                },
                new PatchEntity(1, 0)
                {
                    SfcStepType = StepType.StartingStep,
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "1 Step");
            Assert.IsTrue(sfc.IsStepActive(1, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(0, 0), "2 Steps");
            Assert.IsFalse(sfc.IsStepActive(1, 0), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 1), "2 Steps");
        }
        
        /// <summary>
        /// [S]               [S]
        ///  ===================
        /// -|- true
        /// [S]
        /// -|- false
        /// </summary>
        [Test]
        public void WaitingForSimultaneousMerge()
        {
            List<PatchEntity> stepEntities = new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    SfcStepType = StepType.StartingStep,
                    UpperBranch = BranchType.Double,
                    TransitionText = "true"
                },
                new PatchEntity(1, 0)
                {
                    SfcStepType = StepType.Step,
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(1, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "1 Step");
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "2 Steps");
            Assert.IsFalse(sfc.IsStepActive(1, 0), "2 Steps");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "2 Steps");
        }
        
        /// <summary>
        ///                   [S]
        ///                    |
        /// [S]                |
        ///  ===================
        /// -|- true
        /// [S]
        /// -|- false
        /// </summary>
        [Test]
        public void SimultaneousMergePass()
        {
            List<PatchEntity> stepEntities = new List<PatchEntity>()
            {
                new PatchEntity(1, 0)
                {
                    SfcStepType = StepType.StartingStep,
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.StartingStep,
                    UpperBranch = BranchType.Double,
                    TransitionText = "true"
                },
                new PatchEntity(1, 1)
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
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(1, 0), "1 Step");
            Assert.IsTrue(sfc.IsStepActive(0, 1), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 2), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(1, 0), "2 Steps");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 2), "2 Steps");
        }
        
        /// <summary>
        /// [S]               [S]
        ///  |                 |
        ///  |                 |
        ///  ===================
        /// -|- true
        ///  ===================
        /// [S]               [S]
        /// -|- false         -|- false
        /// </summary>
        [Test]
        public void SimultaneousMergeAndBranch()
        {
            List<PatchEntity> stepEntities = new List<PatchEntity>()
            {
                new PatchEntity(0, 0)
                {
                    SfcStepType = StepType.StartingStep,
                },
                new PatchEntity(1, 0)
                {
                    SfcStepType = StepType.StartingStep,
                },
                new PatchEntity(0, 1)
                {
                    SfcStepType = StepType.Pass,
                    UpperBranch = BranchType.Double,
                    TransitionText = "true",
                    LowerBranch = BranchType.Double
                },
                new PatchEntity(1, 1)
                {
                    SfcStepType = StepType.Pass,
                    UpperBranch = BranchType.Double,
                    LowerBranch = BranchType.Double
                },
                new PatchEntity(0, 2)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
                new PatchEntity(1, 2)
                {
                    SfcStepType = StepType.Step,
                    TransitionText = "false"
                },
            };
            SfcTestHelper sfc = new SfcTestHelper(stepEntities, this);
            sfc.UpdateStep();
            Assert.IsTrue(sfc.IsStepActive(0, 0), "1 Step");
            Assert.IsTrue(sfc.IsStepActive(1, 0), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(0, 2), "1 Step");
            Assert.IsFalse(sfc.IsStepActive(1, 2), "1 Step");
            sfc.UpdateStep();
            Assert.IsFalse(sfc.IsStepActive(1, 0), "2 Steps");
            Assert.IsFalse(sfc.IsStepActive(0, 1), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(0, 2), "2 Steps");
            Assert.IsTrue(sfc.IsStepActive(1, 2), "2 Steps");
        }
    }
}
