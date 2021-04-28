using Godot;
using System.Text;
using System.Collections.Generic;


namespace Osls.Core
{
    /// <summary>
    /// Provides an info panel with the inputs and outputs of the simulated plant.
    /// </summary>
    public class IoInfo : ColorRect
    {
        #region ==================== Fields / Properties ====================
        private const string IoTablePath = "IoTable";
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates the text for the plant info panel according to the given inputs and outputs
        /// </summary>
        public void UpdateText(StateTable inputs, StateTable outputs, bool withStates)
        {
            if (inputs == null || outputs == null)
            {
                GetNode<RichTextLabel>(IoTablePath).BbcodeText = string.Empty;
            }
            else
            {
                StringBuilder builder = new StringBuilder(100);
                
                builder.Append("[b][u]Inputs[/u][/b]\n");
                List<string> booleanInputKeys = inputs.BooleanKeys;
                foreach (string key in booleanInputKeys)
                {
                    builder.Append(key);
                    builder.Append("    [i]bool[/i]   ");
                    if (withStates) builder.Append(inputs.PollBoolean(key));
                    builder.Append("\n");
                }
                List<string> integerInputKeys = inputs.IntegerKeys;
                foreach (string key in integerInputKeys)
                {
                    builder.Append(key);
                    builder.Append("    [i]int[/i]    ");
                    if (withStates) builder.Append(inputs.PollInteger(key));
                    builder.Append("\n");
                }
                
                builder.Append("\n[b][u]Outputs[/u][/b]\n");
                List<string> booleanOutputKeys = outputs.BooleanKeys;
                foreach (string key in booleanOutputKeys)
                {
                    builder.Append(key);
                    builder.Append("    [i]bool[/i]   ");
                    if (withStates) builder.Append(outputs.PollBoolean(key));
                    builder.Append("\n");
                }
                List<string> integerOutputKeys = outputs.IntegerKeys;
                foreach (string key in integerOutputKeys)
                {
                    builder.Append(key);
                    builder.Append("    [i]int[/i]    ");
                    if (withStates) builder.Append(outputs.PollInteger(key));
                    builder.Append("\n");
                }
                
                GetNode<RichTextLabel>(IoTablePath).BbcodeText = builder.ToString();
            }
        }
        #endregion
    }
}
