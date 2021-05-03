using Godot;
using System.Collections.Generic;


namespace Osls.Core
{
    /// <summary>
    /// Provides an info panel with the inputs and outputs of the simulated plant.
    /// </summary>
    public class IoInfo : Control
    {
        #region ==================== Fields / Properties ====================
        [Export] private PackedScene _dataEntryScene;
        private readonly List<DataEntry> _aviableInputs = new List<DataEntry>();
        private readonly List<DataEntry> _aviableOutputs = new List<DataEntry>();
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Creates the text for the plant info panel according to the given inputs and outputs
        /// </summary>
        public void UpdateText(StateTable inputs, StateTable outputs)
        {
            if (inputs == null || outputs == null)
            {
                foreach (var entry in _aviableInputs)
                {
                    entry.QueueFree();
                }
                _aviableInputs.Clear();
                foreach (var entry in _aviableOutputs)
                {
                    entry.QueueFree();
                }
                _aviableOutputs.Clear();
            }
            else
            {
                Node inputNode = GetNode<VBoxContainer>("ScrollContainer/VBox/Inputs");
                Node outputNode = GetNode<VBoxContainer>("ScrollContainer/VBox/Outputs");
                UpdateList(inputNode, inputs, _aviableInputs);
                UpdateList(outputNode, outputs, _aviableOutputs);
            }
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        private void UpdateList(Node root, StateTable inputs, List<DataEntry> aviable)
        {
            var booleans = inputs.BooleanEntries;
            var integers = inputs.IntegerEntries;
            
            int neededCount = booleans.Count + integers.Count;
            for (int i = aviable.Count; i < neededCount; i++)
            {
                var entry = _dataEntryScene.Instance<DataEntry>();
                root.AddChild(entry);
                aviable.Add(entry);
            }
            for (int i = aviable.Count; i > neededCount; i--)
            {
                aviable[i - 1].QueueFree();
                aviable.RemoveAt(i - 1);
            }
            int state = 0;
            foreach (var entry in booleans)
            {
                aviable[state].UpdateText(entry);
                state++;
            }
            foreach (var entry in integers)
            {
                aviable[state].UpdateText(entry);
                state++;
            }
        }
        #endregion
    }
}
