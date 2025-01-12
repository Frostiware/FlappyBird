/**
Todo
- Day/Night Animation bugs
- Increase Flappybird speed with time
- Add rotation to flapping birds
- Change Medal based on Score
- The score should be divided by 2
- Floor should follow consecutively
- Pipe should follow consecutively

- Swap spritesheet
	- Make use of the night pipe
	- Change the bird avatar

- Settings
- About
- Difficulty


- Refactor the game screen
	- menu should use ui element
	- control element should be better presented on game scene
- Add functionalities to all options on the menu screen
- Save bestscore locally
*/

using Godot;

public partial class Game : Node2D
{
	private Sprite2D bgNight, bgDay, _pausedIcon;
	private double _dayNightCounter;

	private Node pole;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bgNight = (Sprite2D)GetNode<Node>("Background/NightBg");
		bgDay = (Sprite2D)GetNode<Node>("Background/DayBg");
		_pausedIcon = (Sprite2D)GetNode<Node>("PausedIcon");
		_pausedIcon.Visible = false;
		_dayNightCounter = 1;
		
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
		ProcessEvent();
		bgDay.Visible = (360 % (int)_dayNightCounter) == 0;
		bgNight.Visible = !bgDay.Visible;
		_pausedIcon.Visible = FlappyBird.IsStateEqual(FlappyBird.GameState.PAUSED);

		if(!FlappyBird.IsStateEqual(FlappyBird.GameState.PLAYING))
			return;

		Pole.Spawn(delta, ref pole);

		float maxCounter = 360 * 2;
		_dayNightCounter += delta;
		if(_dayNightCounter >= maxCounter) _dayNightCounter = 1;
	}


	public static void AnimateCurrentScore(ref Label label)
	{
		label.Scale = Vector2.One;
		var tween = label.GetTree().CreateTween();
		tween.TweenProperty(label, "scale", new Vector2(1.5f, 1.5f), 0.5f)
		.SetTrans(Tween.TransitionType.Sine)
		.SetEase(Tween.EaseType.InOut);
		tween.TweenProperty(label, "scale", Vector2.One, 0.5f)
		.SetTrans(Tween.TransitionType.Sine)
		.SetEase(Tween.EaseType.InOut)
		.SetDelay(0.5f);
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
		foreach(var child in nodes) 
		{
			if(child is Sprite2D d) 
				d.Visible = FlappyBird.IsStateEqual(FlappyBird.GameState.OVER);
			if(child is Control c) 
				c.Visible = FlappyBird.IsStateEqual(FlappyBird.GameState.OVER);
		}
		
	}

}
