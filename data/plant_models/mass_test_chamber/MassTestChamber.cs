using System.Collections.Generic;


namespace Osls.Plants.MassTestChamber
{
    /// <summary>
    /// Boundary class between the simulation and the SFC Controllers
    /// </summary>
    public class MassTestChamber : SimulationPage
    {
        #region ==================== Fields / Properties ====================
        public const string LaserKey = "E301";
        public const string TemperatureSensorKey = "B306";
        public const string FieldGeneratorKey = "G001";
        public const string MassSufficientKey = "B003";
        public const string DetectorKey = "B004";
        
        public const string EmitterMotorKey = "M101";
        public const string EmitterKey = "H101";
        public const string EmitterBackKey = "B100";
        public const string EmitterFrontKey = "B101";
        
        public const string FocusMotorKey = "M201";
        public const string FocusKey = "U201";
        public const string FocusBackKey = "B200";
        public const string FocusFrontKey = "B201";
        
        public bool FieldGeneratorInput { get; private set; }
        public bool LaserInput { get; private set; }
        public bool EmitterInput { get; private set; }
        public bool FocusInput { get; private set; }
        public int EmitterMotorInput { get; private set; }
        public int FocusMotorInput { get; private set; }
        
        /// <summary>
        /// Links the test chamber
        /// </summary>
        public Chamber Chamber { get { return GetNode<Chamber>("Chamber"); } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole page. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            Chamber.Setup();
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
                    { new StateEntry<bool>(FieldGeneratorKey, false, "Field generator", "True will activate the field generator\nto trap particles from entering or leaving.\nFalse will turn it off.") },
                    { new StateEntry<bool>(LaserKey, false, "Heating laser", "True will turn on the laser heater.\nFalse will turn it off.\nFast pulses are possible.") },
                    { new StateEntry<bool>(EmitterKey, false, "Emitter", "True will turn on the particle emitter on the cart.\nFalse will turn it off.") },
                    { new StateEntry<bool>(FocusKey, false, "Focus", "True will keep particles in the intended position\nused to build up mass.\nFalse will turn it off.") },
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>(EmitterMotorKey, 0, "Emitter cart motor", "A signal of 1 will move the chart \ntowards the center and -1 away from it.\nA signal of 0 will keep it in position.") },
                    { new StateEntry<int>(FocusMotorKey, 0, "Focus cart motor", "A signal of 1 will move the chart \ntowards the center and -1 away from it.\nA signal of 0 will keep it in position.") },
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
                    { new StateEntry<bool>(EmitterBackKey, false, "Emitter cart position: Back", "True if the chart is at the distant position.\nFalse otherwise.") },
                    { new StateEntry<bool>(EmitterFrontKey, false, "Emitter cart position: Front", "True if the chart is at the front position.\nFalse otherwise.") },
                    { new StateEntry<bool>(FocusBackKey, false, "Focus cart position: Back", "True if the chart is at the distant position.\nFalse otherwise.") },
                    { new StateEntry<bool>(FocusFrontKey, false, "Focus cart position: Front", "True if the chart is at the front position.\nFalse otherwise.") },
                    { new StateEntry<bool>(MassSufficientKey, false, "Mass OK", "True if the trapped mass is sufficient.\nFalse otherwise.") },
                    { new StateEntry<bool>(DetectorKey, false, "Discharge detector", "True if the detector receives a signal.\nFalse otherwise.") },
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>(TemperatureSensorKey, 293, "Central temperature K", "Measured temperature of the central mass.") },
                }
            );
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int deltaTime)
        {
            UpdateInputs();
            Chamber.Update(this, deltaTime);
        }
        
        private void UpdateInputs()
        {
            FieldGeneratorInput = SimulationInput.PollBoolean(FieldGeneratorKey);
            LaserInput = SimulationInput.PollBoolean(LaserKey);
            EmitterInput = SimulationInput.PollBoolean(EmitterKey);
            FocusInput = SimulationInput.PollBoolean(FocusKey);
            EmitterMotorInput = SimulationInput.PollInteger(EmitterMotorKey);
            FocusMotorInput = SimulationInput.PollInteger(FocusMotorKey);
        }
        #endregion
    }
}
