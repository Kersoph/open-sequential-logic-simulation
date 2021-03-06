namespace Osls.Plants.MassTestChamber
{
    public class TestCart
    {
        #region ==================== Fields / Properties ====================
        private readonly Cart _cart;
        private readonly string _name;
        public bool ReportedDamage { get; set; }
        public bool ReportedBreakdown { get; set; }
        #endregion
        
        
        #region ==================== Constructor ====================
        public TestCart(Cart cart, string name)
        {
            _cart = cart;
            _name = name;
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Observes the cart and reports damage
        /// </summary>
        public void Observe(MassTestChamberTest master)
        {
            if (_cart.Damaged && !ReportedDamage)
            {
                master.PaperLog.AppendWarning("Detected collision at " + _name + "\n");
                ReportedDamage = true;
            }
            if (_cart.IsBroken && !ReportedBreakdown)
            {
                master.PaperLog.AppendError(_name + " returns error: Motors overheated\n");
                ReportedBreakdown = true;
            }
        }
        #endregion
    }
}
