using Godot;
using System.Text;
using System.Collections.Generic;
using Osls.Plants;


namespace Osls.SfcEditor
{
    /// <summary>
    /// Provides an info panel with the inputs and outputs of the simulated plant.
    /// </summary>
    public class PlantInfoPanel : ColorRect
    {
        #region ==================== Fields Properties ====================
        private SimulationPage _simulationControlNode;
        #endregion;
        
        
        #region ==================== Constructor ====================
        /// <summary>
        /// Creates the text for the plant info panel according to the simulation interface
        /// </summary>
        public void SetSimulationInfo(SimulationPage simulationControlNode)
        {
            _simulationControlNode = simulationControlNode;
            UpdateText(false);
        }
        #endregion
        
        
        #region ==================== Public ====================
        public void UpdateText(bool withStates)
        {
            if (_simulationControlNode == null)
            {
                this.GetNode<RichTextLabel>("PlantInfoText").BbcodeText = string.Empty;
            }
            else
            {
                StringBuilder builder = new StringBuilder(100);
                
                builder.Append("[b][u]Plant Inputs[/u][/b]\n");
                List<string> booleanInputKeys = _simulationControlNode.SimulationInput.BooleanKeys;
                foreach (string key in booleanInputKeys)
                {
                    builder.Append(key);
                    builder.Append("    [i]bool[/i]   ");
                    if (withStates) builder.Append(_simulationControlNode.SimulationInput.PollBooleanInput(key));
                    builder.Append("\n");
                }
                List<string> integerInputKeys = _simulationControlNode.SimulationInput.IntegerKeys;
                foreach (string key in integerInputKeys)
                {
                    builder.Append(key);
                    builder.Append("    [i]int[/i]    ");
                    if (withStates) builder.Append(_simulationControlNode.SimulationInput.PollIntegerInput(key));
                    builder.Append("\n");
                }
                
                builder.Append("\n[b][u]Plant Outputs[/u][/b]\n");
                List<string> booleanOutputKeys = _simulationControlNode.SimulationOutput.BooleanKeys;
                foreach (string key in booleanOutputKeys)
                {
                    builder.Append(key);
                    builder.Append("    [i]bool[/i]   ");
                    if (withStates) builder.Append(_simulationControlNode.SimulationOutput.PollBooleanOutput(key));
                    builder.Append("\n");
                }
                List<string> integerOutputKeys = _simulationControlNode.SimulationOutput.IntegerKeys;
                foreach (string key in integerOutputKeys)
                {
                    builder.Append(key);
                    builder.Append("    [i]int[/i]    ");
                    if (withStates) builder.Append(_simulationControlNode.SimulationOutput.PollIntegerOutput(key));
                    builder.Append("\n");
                }
                
                this.GetNode<RichTextLabel>("PlantInfoText").BbcodeText = builder.ToString();
            }
        }
        #endregion
    }
}