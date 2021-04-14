using System.Collections.Generic;


namespace Osls
{
    public interface IProcessingData
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// Holds the input registers of the processing unit
        /// </summary>
        StateTable InputRegisters { get; }
        
        /// <summary>
        /// Holds the output registers of the processing unit
        /// </summary>
        StateTable OutputRegisters { get; }
        
        /// <summary>
        /// Links the integer variables
        /// </summary>
        IEnumerable<string> IntVariables { get; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Returns true if there is a internal variable with this key
        /// </summary>
        bool HasIntVariable(string key);
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        int LookupIntVariable(string key);
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        bool LookupBoolVariable(string key);
        #endregion
    }
}