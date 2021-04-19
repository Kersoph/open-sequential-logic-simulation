using System.Collections.Generic;


namespace Osls.St
{
    public class Lexer
    {
        #region ==================== Public Methods ====================
        /// <summary>
        /// Splits the text into tokens / terminal symbols
        /// </summary>
        public static string[] Tokenise(string text)
        {
            string[] splits = text.Split(' ');
            List<string> words = new List<string>(splits.Length);
            for (int i = 0; i < splits.Length; i++)
            {
                CheckSplits(splits[i], words);
            }
            return words.ToArray();
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Checks the rough splits for combined words or empty entries.
        /// Check possible use of regular expressions for future versions.
        /// </summary>
        private static void CheckSplits(string split, List<string> target)
        {
            if (string.IsNullOrWhiteSpace(split)
            || CheckAndAddCharacter(split, '!', target)
            || CheckAndAddCharacter(split, '(', target)
            || CheckAndAddCharacter(split, ')', target)) return;
            target.Add(split); // atomic word
        }
        
        private static bool CheckAndAddCharacter(string split, char character, List<string> target)
        {
            int index = split.IndexOf(character);
            if (index == 0)
            {
                target.Add(character.ToString());
                CheckSplits(split.Substring(1), target);
                return true;
            }
            if (index > 0)
            {
                CheckSplits(split.Substring(0, index), target);
                target.Add(character.ToString());
                CheckSplits(split.Substring(index + 1), target);
                return true;
            }
            return false;
        }
        #endregion
    }
}