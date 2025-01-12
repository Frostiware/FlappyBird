using Godot;
using System;

public partial class Floor : Area2D
{
	public static Vector2 Pos{ get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pos = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!FlappyBird.IsStateEqual(FlappyBird.GameState.PLAYING))
			return;
			
		var nPos = new Vector2(Position.X - (float)(FlappyBird.Speed * delta), Pos.Y);
		Position = nPos;
		if(Position.X <= -FlappyBird.WindowSize.X)
		{
			Position = new Vector2(FlappyBird.WindowSize.X, Pos.Y);
		}

	}
}
