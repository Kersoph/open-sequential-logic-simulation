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
        /// Gets the current element
        /// </summary>
        public string Current { get { return Words[Position]; } }
        
        /// <summary>
        /// True if the end of the word chain is reached
        /// </summary>
        public bool IsEndReached { get { return Position >= Words.Length; } }
        #endregion
        
        
        #region ==================== Constructor ====================
        public Terminals(string text)
        {
            Words = Lexer.Tokenise(text);
            Position = 0;
        }
        
        public Terminals(string[] words)
        {
            Words = words;
            Position = 0;
        }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Advances the position to the next element.
        /// </summary>
        public void MoveNext()
        {
            Position++;
        }
        #endregion
    }
}