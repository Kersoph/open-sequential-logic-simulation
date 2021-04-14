namespace Osls.St.Assignment
{
    /// <summary>
    /// Base class for all assignment expressions
    /// </summary>
    public abstract class AssignmentExpression
    {
        #region ==================== Public ====================
        /// <summary>
        /// Executes the assignment according to the model.
        /// </summary>
        public abstract void Execute(IProcessingUnit pu);
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public abstract bool IsValid();
        #endregion
    }
}