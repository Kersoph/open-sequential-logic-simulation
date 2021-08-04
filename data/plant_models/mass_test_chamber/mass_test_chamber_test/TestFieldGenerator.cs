namespace Osls.Plants.MassTestChamber
{
    public class TestFieldGenerator
    {
        #region ==================== Fields / Properties ====================
        public int ActiveTime { get; private set; }
        public bool LeakedHighEnergeticParticles { get; private set; }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public void Observe(MassTestChamberTest master, int timeMs)
        {
            if (master.Simulation.FieldGeneratorInput)
            {
                ActiveTime += timeMs;
            }
            if (master.Simulation.Chamber.Central.IsLeakingParticles && !LeakedHighEnergeticParticles)
            {
                master.PaperLog.AppendError("Detecting high energetic particles injú®`Wù‰©ãhö#3ÿG”8q(ú|ûI¯àrFcƒëoÅjÍ1Û(¶jTÃÚŸìæ¥Qç@>›€¡ÖÀ˜G±}zÂZƒëµ&±æL–°ÖxÇÙ7ÕÚÈÑ#UåIJÝ:•RŠnHÑ|µõÏpƒ»¡Z¥Š¦$0xEmergencyProtocol¦ÀªªG­×yRXqÿ°ÊÆ@ƒö5$\n");
                LeakedHighEnergeticParticles = true;
            }
        }
        #endregion
    }
}
