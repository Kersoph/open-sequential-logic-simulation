using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Timer = Godot.Timer;

namespace WAT
{

	public class Test : Node
	{
		[AttributeUsage(AttributeTargets.Method)]
		protected class TestAttribute : Attribute
		{
		}

		[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
		protected class RunWith : Attribute
		{
			public object[] arguments;

			public RunWith(params object[] args)
			{
				arguments = args;
			}
		}

		protected const string YIELD = "finished";
		public const bool TEST = true;
		public Assertions Assert;
		public Timer Yielder;
		public Reference Watcher;
		private bool lastAssertionSuccess;

		[Signal]
		public delegate void Described(string MethodDescription);

		public virtual string Title()
		{
			return GetType().Name;
		}

		public void OnLastAssertion(Dictionary assertion)
		{
			lastAssertionSuccess = (bool) assertion["success"];
		}

		public bool PreviousAssertionFailed()
		{
			return !lastAssertionSuccess;
		}

		protected void Describe(string message)
		{
			EmitSignal(nameof(Described), message);
		}

		public virtual void Start()
		{
		}

		public virtual void Pre()
		{
		}

		public virtual void Post()
		{
		}

		public virtual void End()
		{
		}

		protected Timer UntilTimeout(double time)
		{
			return (Timer) Yielder.Call("until_timeout", time);
		}

		protected Timer UntilSignal(Godot.Object emitter, string signal, double time)
		{
			Watcher.Call("watch", emitter, signal);
			return (Timer) Yielder.Call("until_signal", time, emitter, signal);
		}

		protected void Watch(Godot.Object emitter, string signal)
		{
			Watcher.Call("watch", emitter, signal);
		}

		protected void UnWatch(Godot.Object emitter, string signal)
		{
			Watcher.Call("unwatch", emitter, signal);
		}

		protected Recorder Record(Godot.Object who, Array properties)
		{
			Recorder recorder = new Recorder();
			recorder.Record(who, properties);
			AddChild(recorder);
			return recorder;

		}

		public void Simulate(Node obj, int times, float delta)
		{
			for (int i = 0; i < times; i++)
			{
				if (obj.HasMethod("_Process"))
				{
					obj._Process(delta);
				}

				if (obj.HasMethod("_PhysicsProcess"))
				{
					obj._PhysicsProcess(delta);
				}

				foreach (Node kid in obj.GetChildren())
				{
					Simulate(kid, 1, delta);
				}
			}
		}

		public static string get_instance_base_type()
		{
			return "WAT.Test";
		}

		

		public Array GetScriptMethodList()
		{
			Array methods = new Array();
			List<MethodInfo> methodInfos = new List<MethodInfo>(GetType().GetMethods().Where(m => m.IsDefined(typeof(TestAttribute))).ToList());
			foreach (var methodInfo in methodInfos)
			{
				methods.Add(new Dictionary {{"name", methodInfo.Name}});
			}
			return methods;
		}

		public Array GetScriptMethodListWithArgs()
		{
			Array methods = new Array();
			List<MethodInfo> methodInfos = new List<MethodInfo>(GetType().GetMethods().Where(m => m.IsDefined(typeof(TestAttribute))).ToList());
			foreach (var methodInfo in methodInfos)
			{
				if (methodInfo.IsDefined(typeof(RunWith)))
				{
					Attribute[] attrs = System.Attribute.GetCustomAttributes(methodInfo);
					foreach (Attribute attr in attrs)
					{
						if (attr is RunWith runWith)
						{
							var args = new Array();
							foreach (var arg in runWith.arguments)
							{
								args.Add(arg);
							}
							var info = new Dictionary {{"name", methodInfo.Name}, {"args", args}};
							methods.Add(info);
						}
					}
				}
				else
				{
					var info = new Dictionary {{"name", methodInfo.Name}, {"args", new Array()}};
					methods.Add(info);
				}
			}
			return methods;
		}

		public static Test CreateInstance(CSharpScript script)
		{
			return (Test) script.New();
		}

	}
}
