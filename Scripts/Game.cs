using Godot;
using System;


public partial class Game : Node2D
{
	private Sprite2D bgNight, bgDay;
	private Node pole;

	private double lastTime = 10.0;
	private double spawnCounter = 0.0;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		bgNight = (Sprite2D)GetNode<Node>("Background/NightBg");
		bgDay = (Sprite2D)GetNode<Node>("Background/DayBg");
		pole = GetNode<Node>("Poles");
		Pole.Init(ref pole);
		FlappyBird.State = FlappyBird.GameState.PLAYING;
		toggleGameOver();

		GD.Print("Enter the Game Scene");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		spawnCounter += delta;
		if(spawnCounter >= lastTime)
		{
			spawnCounter = 0.0;
			lastTime = FlappyBird.GetRandRange(5, 10);
			Pole.Spawn(ref pole);
		}

		bgDay.Visible = FlappyBird.IsDay;
		bgNight.Visible = !bgDay.Visible;

		ProcessEvent();
	}

	private void toggleGameOver()
	{
		var node = GetNode<Node>("GameOver").GetChildren();
		foreach(Sprite2D sprite in node)
		{
			sprite.Visible = FlappyBird.State == FlappyBird.GameState.OVER;
		}
	}

	private void ProcessEvent()
	{
		if(Input.IsActionJustReleased("enter"))
		{
			switch (FlappyBird.State)
			{
				case FlappyBird.GameState.OVER:
					GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
					break;
				case FlappyBird.GameState.PAUSED:
					GD.Print("Game Paused");
					break;
				case FlappyBird.GameState.PLAYING:
					GD.Print("Game Playing");
					break;
			}
		}
	}
}
