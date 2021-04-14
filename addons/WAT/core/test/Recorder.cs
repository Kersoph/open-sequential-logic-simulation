using Godot;
using System;
using Godot.Collections;

namespace WAT 
{
	
    public class Recorder : Node
    {
        Godot.Object What;
        Godot.Collections.Dictionary<string, object> Properties = new Godot.Collections.Dictionary<string, object>();
        bool isRecording = false;
	
        public void Start() { isRecording = true; }
        public void Stop() { isRecording = false; }
		
        public void Record(Godot.Object what, Godot.Collections.Array properties)
        {
            What = what;
            foreach(string property in properties){
                Properties[property] = new Godot.Collections.Array();
            }
        }
		
        public override void _Process(float delta)
        {
            if(isRecording) { Capture(); }
        }
		
        private void Capture()
        {
            if(IsInstanceValid(What)){
                foreach(var Property in Properties.Keys){
                    Godot.Collections.Array values = (Godot.Collections.Array)Properties[Property];
                    values.Add(What.Get(Property));
                }
            }
        }
		
        public Godot.Collections.Array GetPropertyTimeline(string Property){
            return (Godot.Collections.Array)Properties[Property];
        }
		
        public Godot.Collections.Dictionary GetPropertyMap()
        {
            return (Godot.Collections.Dictionary)Properties;
        }
    }
}