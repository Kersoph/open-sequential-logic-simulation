using Godot;
using Osls.Bubbles;


namespace Osls.Plants.MassTestChamber
{
    public class DecorationNode : Spatial
    {
        #region ==================== Fields / Properties ====================
        private Particles _crashParticles;
        private Light _flashLight;
        private BubbleSprite _flashSprite;
        private Light _errorLight;
        private BubbleSprite _errorSprite1;
        private BubbleSprite _errorSprite2;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the decoration node
        /// </summary>
        public void Initialise()
        {
            _crashParticles = GetNode<Particles>("CrashSparkles");
            _flashLight = GetNode<Light>("FlashLight");
            _flashSprite = GetNode<BubbleSprite>("FlashSprite");
            _errorLight = GetNode<Light>("ErrorLight");
            _errorSprite1 = GetNode<BubbleSprite>("ErrorSprite1");
            _errorSprite2 = GetNode<BubbleSprite>("ErrorSprite2");
        }
        
        /// <summary>
        /// Called by the emergency system with the amount of time until it breaks down.
        /// Negative values will let it run out.
        /// </summary>
        public void UpdateToFailure(int timeLeft)
        {
            if (timeLeft <= 0)
            {
                Reset();
                _flashSprite.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Nok, 1.0f);
                _errorSprite2.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Nok, 1.0f);
                _errorSprite1.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Nok, 1.0f);
            }
            else if (timeLeft < 200)
            {
                _flashLight.Visible = true;
                _crashParticles.Emitting = true;
                _errorLight.Visible = true;
            }
            else if (timeLeft < 400)
            {
                _flashSprite.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Exclamation, 1.0f);
                _errorLight.Visible = false;
            }
            else if (timeLeft < 600)
            {
                _errorLight.Visible = true;
            }
            else if (timeLeft < 800)
            {
                _errorSprite2.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Attention, 1.4f);
                _errorLight.Visible = false;
            }
            else if (timeLeft < 1000)
            {
                _errorSprite1.ShowAs(BubbleSprite.Bubble.Say, BubbleSprite.Expression.Nok, 1.6f);
                _errorLight.Visible = true;
            }
        }
        
        /// <summary>
        /// Resets the simulation back to the start
        /// </summary>
        public void Reset()
        {
            _errorLight.Visible = false;
            _flashLight.Visible = false;
            _crashParticles.Emitting = false;
        }
        #endregion
    }
}
