namespace Osls.SfcEditor
{
    public enum BranchType
    {
        /// <summary>
        /// This branch line is not used.
        /// </summary>
        Unused,
        
        /// <summary>
        /// This branch line is used as an OR branch/merge
        /// </summary>
        Single,
        
        /// <summary>
        /// This branch line is used as a parallel branch/merge
        /// </summary>
        Double,
    }
}
