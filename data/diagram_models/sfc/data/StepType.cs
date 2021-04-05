namespace Osls.SfcEditor
{
    public enum StepType
    {
        /// <summary>
        /// This step cell is not used, except for bypassing branches to neighbouring cells.
        /// </summary>
        Unused,
        
        /// <summary>
        /// This step cell is skipped and bridging down the logic.
        /// </summary>
        Pass,
        
        /// <summary>
        /// This step cell is a standard step.
        /// </summary>
        Step,
        
        /// <summary>
        /// This step cell is a starting step, indicated with a double lined frame.
        /// </summary>
        StartingStep,
        
        /// <summary>
        /// This step cell is a jump, connectiong it to another cell.
        /// </summary>
        Jump
    }
}