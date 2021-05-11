using Godot;
using Osls.Bubbles;


namespace Osls.Plants.ElectricalBarrier
{
    public class GuardAgent : Spatial
    {
        #region ==================== Fields / Properties ====================
        public const string OpenGateSwitchKey = "S_OpenGate";
        private int _lastCarNumber = -1;
        private int _reactionTimer;
        private bool _witnessedBreakdown;
        
        protected BubbleSprite Bubble { get { return GetNode<BubbleSprite>("BubbleSprite"); } }
        
        /// <summary>
        /// Set to false to prevent the gate being opened
        /// </summary>
        public bool AllowVehiclePass { get; set; } = true;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Calculates the next simulation step.
        /// </summary>
        public void Update(ElectricalBarrier master, int deltaTime)
        {
            if (master.Vehicle.IsCarAtCheckpoint || master.Vehicle.IsCarUnderBarrier)
            {
                if (AllowVehiclePass)
                {
                    ReactToValidCar(master, deltaTime);
                }
                else
                {
                    ReactToInvalidCar(master);
                }
            }
            else
            {
                Idle(master);
            }
            GeneralObservation(master);
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void ReactToValidCar(ElectricalBarrier master, int deltaTime)
        {
            if (_lastCarNumber != master.Vehicle.TimesPassedTrack)
            {
                _lastCarNumber = master.Vehicle.TimesPassedTrack;
                _reactionTimer = 0;
            }
            _reactionTimer += deltaTime;
            if (_reactionTimer > 6000)
            {
                if (!_witnessedBreakdown) Bubble.ShowAs(BubbleSprite.Bubble.Think, BubbleSprite.Expression.Confused, 2);
                _reactionTimer = 0;
            }
            if (master.Vehicle.WaitingTime >= 28000) Bubble.ShowAs(BubbleSprite.Bubble.Think, BubbleSprite.Expression.Annoyed, 2);
            master.SimulationOutput.SetValue(OpenGateSwitchKey, true);
        }
        
        private void ReactToInvalidCar(ElectricalBarrier master)
        {
            if (_lastCarNumber != master.Vehicle.TimesPassedTrack && master.Vehicle.IsCarAtCheckpoint)
            {
                _lastCarNumber = master.Vehicle.TimesPassedTrack;
                if (!_witnessedBreakdown) Bubble.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Nok, 1);
            }
            if (master.Vehicle.IsCarUnderBarrier)
            {
                Bubble.ShowAs(BubbleSprite.Bubble.Shout, BubbleSprite.Expression.Exclamation, 2, true);
            }
            master.SimulationOutput.SetValue(OpenGateSwitchKey, false);
        }
        
        private void Idle(ElectricalBarrier master)
        {
            if (_reactionTimer > 10000)
            {
                Bubble.ShowAs(BubbleSprite.Bubble.Think, BubbleSprite.Expression.Sleeping, 2);
                _reactionTimer = 0;
            }
            master.SimulationOutput.SetValue(OpenGateSwitchKey, false);
        }
        
        private void GeneralObservation(ElectricalBarrier master)
        {
            if (!_witnessedBreakdown && (master.Vehicle.Damaged || master.Barrier.IsBroken))
            {
                _witnessedBreakdown = true;
                Bubble.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Attention, 4, true);
            }
        }
        #endregion
    }
}