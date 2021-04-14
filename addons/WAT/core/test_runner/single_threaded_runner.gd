extends Node

const Log: Script = preload("res://addons/WAT/log.gd")
const TestController: Script = preload("test_controller.gd")
const MonoTestController = preload("TestController.cs")
signal run_completed

# Ignore threads here, this is just for abvoe
func run(tests: Array, threads: int = 0) -> void:
	Log.method("run", self)
	var results: Array = []
	var _test_controller: TestController = TestController.new()
	var _mono_controller: MonoTestController = MonoTestController.new()
	add_child(_test_controller)
	add_child(_mono_controller)
	for test in tests:
		if test["script"] is GDScript:
			_test_controller.run(test)
			yield(_test_controller, "finished")
			results.append(_test_controller.get_results())
		elif test["script"] is CSharpScript:
			_mono_controller.Run(test)
			yield(_mono_controller, "finished")
			results.append(_mono_controller.GetResults())
	_test_controller.queue_free()
	_mono_controller.queue_free()
	Log.method("run_completed", self)
	emit_signal("run_completed", results)
