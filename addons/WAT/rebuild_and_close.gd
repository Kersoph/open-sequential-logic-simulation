tool
extends Label

var a = 0.0

func _ready() -> void:
	print("Starting reimport process...")

func _process(delta):
	a = a + delta
	text = String(a)
	if (a > 10):
		print("Exiting reimport process...")
		OS.set_exit_code(1)
		get_tree().quit()
	pass
