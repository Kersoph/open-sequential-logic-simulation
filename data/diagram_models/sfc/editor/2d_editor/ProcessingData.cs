using System.IO;
using System.Collections.Generic;
using Osls.SfcEditor.Interpreters;


namespace Osls.SfcEditor
{
    public class ProcessingData : IProcessingData
    {
        #region ==================== Fields Properties ====================
        /// <summary>
        /// Holds the input registers of the processing unit
        /// </summary>
        public StateTable InputRegisters { get; set; }
        
        /// <summary>
        /// Holds the output registers of the processing unit
        /// </summary>
        public StateTable OutputRegisters { get; set; }
        
        /// <summary>
        /// The sfc entity holding the diagram data
        /// </summary>
        public SfcEntity SfcEntity { get; private set; }
        
        /// <summary>
        /// Step Master holding the step names and Ids
        /// </summary>
        public StepMaster StepMaster { get; private set; }
        
        /// <summary>
        /// Links the integer variables
        /// </summary>
        public IEnumerable<string> IntVariables { get { return StepMaster.PatchStepTimeMap.Keys; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public ProcessingData()
        {
            SfcEntity = new SfcEntity();
            StepMaster = new StepMaster();
        }
        
        /// <summary>
        /// Returns true if there is a internal variable with this key
        /// </summary>
        public bool HasIntVariable(string key)
        {
            return StepMaster.ContainsInternalNumeric(key);
        }
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        public int LookupIntVariable(string key)
        {
            return 0;
        }
        
        /// <summary>
        /// Returns true if there is a internal variable with this key
        /// </summary>
        public bool HasBoolVariable(string key)
        {
            return StepMaster.ContainsInternalBoolean(key);
        }
        
        /// <summary>
        /// Gets the value of the internal variable
        /// </summary>
        public bool LookupBoolVariable(string key)
        {
            return false;
        }
        
        /// <summary>
        /// Loads the sfc file and repolaces the current data in the SfcEntity
        /// </summary>
        public void LoadData(string filepath)
        {
            using (FileStream stream = File.Open(filepath, FileMode.OpenOrCreate))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    SfcEntity.ReadFrom(reader);
                }
            }
        }
        
        /// <summary>
        /// Saves the SfcEntity to a file
        /// </summary>
        public void SaveData(string filepath)
        {
            using (FileStream stream = File.Open(filepath, FileMode.OpenOrCreate))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    SfcEntity.WriteTo(writer);
                }
            }
        }
        #endregion
    }
}