/*
- ISSUES
	- Menu Choice
	- Rate Game
	- Share Game
*/
using Godot;

public partial class Menu : Control
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		FlappyBird.SetState(FlappyBird.GameState.MENU);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustReleased("tap"))
		{
			// Issue context should also be considered right here
			GetTree().ChangeSceneToFile("res://Scenes/Pages/Game.tscn");
		}
	}
}
