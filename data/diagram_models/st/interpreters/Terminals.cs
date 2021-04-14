namespace Osls.St
{
    internal class Terminals
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Gets all words
        /// </summary>
        public string[] Words { get; private set; }
        
        /// <summary>
        /// Gets the current position
        /// </summary>
        public int Position { get; private set; }
        
        /// <summary>
        /// True if the end of the word chain is reached
        /// </summary>
        public bool IsEndReached { get { return Position >= Words.Length; } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public Terminals(string text)
        {
            Words = text.Split(' ');
            Position = 0;
        }
        
        public Terminals(string[] words)
        {
            Words = words;
            Position = 0;
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        public string GetNext()
        {
            string word = Words[Position];
            Position++;
            return word;
        }
        #endregion
    }
}