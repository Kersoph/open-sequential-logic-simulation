using System.Collections.Generic;


namespace Osls.Plants.Sandbox
{
    /// <summary>
    /// Sandbox lesson to test own ideas
    /// </summary>
    public class Sandbox : SimulationPage
    {
        #region ==================== Fields / Properties ====================
        private DigitalInputs _digitalInputs;
        private DigitalOutputs _digitalOutputs;
        private AnalogueIO _analogueIO;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole page. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _digitalInputs = GetNode<DigitalInputs>("UI/DigitalInputs");
            _digitalOutputs = GetNode<DigitalOutputs>("UI/DigitalOutputs");
            _analogueIO = GetNode<AnalogueIO>("UI/AnalogueIO");
            _digitalInputs.SetupUi();
            _digitalOutputs.SetupUi();
            _analogueIO.SetupUi();
        }
        
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public override void SetupUi()
        {
            
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
                    { new StateEntry<bool>("DO1", false, "Digital Output 1", "This plant input is an output of the controller") },
                    { new StateEntry<bool>("DO2", false, "Digital Output 2", "This plant input is an output of the controller") },
                    { new StateEntry<bool>("DO3", false, "Digital Output 3", "This plant input is an output of the controller") },
                    { new StateEntry<bool>("DO4", false, "Digital Output 4", "This plant input is an output of the controller") },
                    { new StateEntry<bool>("DO5", false, "Digital Output 5", "This plant input is an output of the controller") },
                    { new StateEntry<bool>("DO6", false, "Digital Output 6", "This plant input is an output of the controller") },
                    { new StateEntry<bool>("DO7", false, "Digital Output 7", "This plant input is an output of the controller") },
                    { new StateEntry<bool>("DO8", false, "Digital Output 8", "This plant input is an output of the controller") },
                    { new StateEntry<bool>("DO9", false, "Digital Output 9", "This plant input is an output of the controller") },
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>("AO1", 0, "Analog Output 1", "This plant input is an output of the controller") },
                    { new StateEntry<int>("AO2", 0, "Analog Output 2", "This plant input is an output of the controller") },
                    { new StateEntry<int>("AO3", 0, "Analog Output 3", "This plant input is an output of the controller") },
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
                    { new StateEntry<bool>("DI1", false, "Digital Input 1", "This plant output is an input of the controller") },
                    { new StateEntry<bool>("DI2", false, "Digital Input 2", "This plant output is an input of the controller") },
                    { new StateEntry<bool>("DI3", false, "Digital Input 3", "This plant output is an input of the controller") },
                    { new StateEntry<bool>("DI4", false, "Digital Input 4", "This plant output is an input of the controller") },
                    { new StateEntry<bool>("DI5", false, "Digital Input 5", "This plant output is an input of the controller") },
                    { new StateEntry<bool>("DI6", false, "Digital Input 6", "This plant output is an input of the controller") },
                    { new StateEntry<bool>("DI7", false, "Digital Input 7", "This plant output is an input of the controller") },
                    { new StateEntry<bool>("DI8", false, "Digital Input 8", "This plant output is an input of the controller") },
                    { new StateEntry<bool>("DI9", false, "Digital Input 9", "This plant output is an input of the controller") },
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>("AI1", 0, "Analog Input 1", "This plant output is an input of the controller") },
                    { new StateEntry<int>("AI2", 0, "Analog Input 2", "This plant output is an input of the controller") },
                    { new StateEntry<int>("AI3", 0, "Analog Input 3", "This plant output is an input of the controller") },
                }
            );
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int deltaTime)
        {
            _digitalInputs.UpdateModel(this);
            _digitalOutputs.UpdateModel(this);
            _analogueIO.UpdateModel(this);
        }
        #endregion
    }
}
