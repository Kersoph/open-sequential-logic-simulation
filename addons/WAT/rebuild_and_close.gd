tool
extends Label

var a = 0.0

func _ready() -> void:
	print("Starting reimport process...")
	push_warning("Starting reimport process...")

func _process(delta) -> void:
	a = a + delta
	text = String(a)
	if (a > 10):
		print("Exiting reimport process...")
		push_warning("Exiting reimport process...")
		OS.set_exit_code(1)
		get_tree().quit()
	pass

static func initialize() -> void:
	print("rebuild_and_close is ready")
	push_warning("rebuild_and_close is ready.")
