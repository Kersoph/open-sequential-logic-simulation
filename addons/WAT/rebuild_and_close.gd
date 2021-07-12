tool
extends Label

var delay = 0.0

func _ready() -> void:
	push_warning("Starting reimport process...")

func _process(delta) -> void:
	delay = delay + delta
	text = String(delay)
	if (delay > 10):
		push_warning("Exiting reimport process...")
		OS.exit_code = 0
		get_tree().quit()
	pass
