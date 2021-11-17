using System.Collections.Generic;


namespace Osls.Plants.CircularSaw
{
    /// <summary>
    /// Boundary to the circular saw simulation
    /// </summary>
    public class CircularSawModel : SimulationPage
    {
        #region ==================== Fields / Properties ====================
        public const string ONButtonKey = "S1";
        public const string OFFButtonKey = "S2";
        
        /// <summary>
        /// Links the saw blade and motor controller
        /// </summary>
        public CircularSawNode CircularSawNode { get; private set; }
        
        /// <summary>
        /// Sets or gets the current button state in the simulation output table
        /// </summary>
        public bool ONButtonState
        {
            get
            {
                return SimulationOutput.PollBoolean(ONButtonKey);
            }
            set
            {
                SimulationOutput.SetValue(ONButtonKey, value);
            }
        }
        
        /// <summary>
        /// Sets or gets the current button state in the simulation output table
        /// </summary>
        public bool OFFButtonState
        {
            get
            {
                return SimulationOutput.PollBoolean(OFFButtonKey);
            }
            set
            {
                SimulationOutput.SetValue(OFFButtonKey, value);
            }
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole page. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            CircularSawNode = GetNode<CircularSawNode>("CircularSawNode");
            CircularSawNode.Setup();
        }
        
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public override void SetupUi()
        {
            GetNode<UIControl>("UIControl").SetupUI();
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Returns the input definition for the simulation
        /// </summary>
        protected override StateTable DefineInputs()
        {
            return new StateTable(
                new List<StateEntry<bool>>()
                {
                    { new StateEntry<bool>(CircularSawNode.MotorKey, false, "Saw motor", "True will connect the motor to the power source") },
                },
                new List<StateEntry<int>>()
                {
                }
            );
        }
        
        /// <summary>
        /// Returns the output definition for the simulation
        /// </summary>
        protected override StateTable DefineOutput()
        {
            return new StateTable(
                new List<StateEntry<bool>>()
                {
                    { new StateEntry<bool>(ONButtonKey, false, "ON button pressed", "True if the saw should turn on") },
                    { new StateEntry<bool>(OFFButtonKey, false, "OFF button pressed", "True if the saw should turn off") },
                },
                new List<StateEntry<int>>()
                {
                }
            );
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int deltaTime)
        {
            CircularSawNode.CalculateNextStep(this, deltaTime);
        }
        #endregion
    }
}
