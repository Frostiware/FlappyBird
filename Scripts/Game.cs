using Godot;
using System;
using System.Linq;

public partial class Game : Node2D
{
	private Sprite2D bgNight, bgDay, _pausedIcon;

	private Node pole;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bgNight = (Sprite2D)GetNode<Node>("Background/NightBg");
		bgDay = (Sprite2D)GetNode<Node>("Background/DayBg");
		_pausedIcon = (Sprite2D)GetNode<Node>("PausedIcon");
		pole = GetNode<Node>("Poles");

		FlappyBird.WindowSize = GetViewport().GetVisibleRect().Size;
		
		
		FlappyBird.SetState(FlappyBird.GameState.PLAYING);
		var gameOverNode = GetNode<Node>("GameOver");
		ToggleGameOver(ref gameOverNode);
		Pole.Init(ref pole);
		Bird.Reset();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		bgDay.Visible = FlappyBird.IsDay;
		bgNight.Visible = !bgDay.Visible;
		_pausedIcon.Visible = FlappyBird.IsStateEqual(FlappyBird.GameState.PAUSED);

		ProcessEvent();

		Pole.Spawn(delta, ref pole);
	}

	private void ProcessEvent()
	{
		if(Input.IsActionJustReleased("tap"))
		{
			switch (FlappyBird.GetState())
			{
				case FlappyBird.GameState.OVER:
					GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
					break;
				case FlappyBird.GameState.PAUSED:
					FlappyBird.SetState(FlappyBird.GameState.PLAYING);
					break;
			}
		}

		if(Input.IsActionJustReleased("pause"))
			FlappyBird.SetState(FlappyBird.GameState.PAUSED);
	}


	// This function toggles the visibility of all children that 
	// belongs to the GameOver node
	public static void ToggleGameOver(ref Node node)
	{
		var nodes = node.GetChildren();
		foreach(Sprite2D sprite in nodes.Cast<Sprite2D>())
			sprite.Visible = FlappyBird.IsStateEqual(FlappyBird.GameState.OVER);
	}

}
