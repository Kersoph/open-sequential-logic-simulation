using Godot;


namespace Osls.Core
{
    public class DataEntry : MarginContainer
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates the text for the plant info panel according to the given entry
        /// </summary>
        public void UpdateText<T>(StateEntry<T> data) where T : struct
        {
            GetNode<Label>("Data/Key").Text = data.Key;
            GetNode<Label>("Data/Value").Text = data.Value.ToString();
            GetNode<Label>("Data/Description").Text = data.Description;
            HintTooltip = data.Hint;
        }
        #endregion
    }
}
