using WAT;
using Osls.St;


namespace Tests.SfcEditor.Interpreters
{
    public class LexerTest : Test
    {
        public override string Title()
        {
            return "ST like Lexer test";
        }
        
        [Test]
        [RunWith("hello world", new string[] {"hello", "world"})]
        [RunWith("A and B", new string[] {"A", "and", "B"})]
        [RunWith("AandB", new string[] {"AandB"})]
        [RunWith("!B", new string[] {"!", "B"})]
        [RunWith("!(B and A)!C", new string[] {"!", "(", "B", "and", "A", ")", "!", "C"})]
        [RunWith("     A    !! B", new string[] {"A", "!", "!", "B"})]
        public void SplitTests(string text, string[] words)
        {
            string[] splitWords = Lexer.Tokenise(text);
            Assert.IsEqual(splitWords.Length, words.Length);
            for (int i = 0; i < splitWords.Length; i++)
            {
                Assert.IsEqual(splitWords[i], words[i]);
            }
        }
    }
}