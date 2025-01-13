using Godot;

public partial class Game : Node2D
{
	private Sprite2D _bgNight, _bgDay, _pausedIcon;
	private Node _pole;

	private SceneTreeTimer _backgroundChangeTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_bgNight = GetNode<Sprite2D>("Background/NightBg");
		_bgDay = GetNode<Sprite2D>("Background/DayBg");
		_pausedIcon = (Sprite2D)GetNode<Node>("PausedIcon");
		_pausedIcon.Visible = false;
		
		_pole = GetNode<Node>("Poles");

		FlappyBird.WindowSize = GetViewport().GetVisibleRect().Size;
		
		
		FlappyBird.SetState(FlappyBird.GameState.PLAYING);
		var gameOverNode = GetNode<Node>("GameOver");
		ToggleGameOver(ref gameOverNode);
		Pole.Init(ref _pole);
		Bird.Reset();

		var color = _bgNight.Modulate;
		color.A = 0;
		_bgNight.Modulate = color;
		OnBackgroundChange(120.0f);	// every 2mins, the day/night should change
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ProcessEvent();
		_pausedIcon.Visible = FlappyBird.IsStateEqual(FlappyBird.GameState.PAUSED);

		if(!FlappyBird.IsStateEqual(FlappyBird.GameState.PLAYING))
			return;

		Pole.Spawn(delta, ref _pole);
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



	private void OnBackgroundChange(float interval, int counter = 1)
	{
		if(!FlappyBird.IsStateEqual(FlappyBird.GameState.PLAYING)) 
			return;

		var timer = GetTree().CreateTimer(interval);
		timer.Timeout += () => {
			var bgHideName = counter % 2 != 0 ? "NightBg": "DayBg";
			var bgShowName = counter % 2 != 0 ? "DayBg" : "NightBg";
			Sprite2D bgHide = GetNode<Sprite2D>($"Background/{bgHideName}");
			Sprite2D bgShow = GetNode<Sprite2D>($"Background/{bgShowName}");

			var tween = GetTree().CreateTween();
			tween.TweenProperty(bgHide, "modulate:a", 0.3f, 1.0f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

			tween.TweenProperty(bgShow, "modulate:a", 1.0f, 0.5f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
			
			OnBackgroundChange(interval, ++counter);
		};
	}


	private void ProcessEvent()
	{
		if(Input.IsActionJustReleased("tap"))
		{
			switch (FlappyBird.GetState())
			{
				case FlappyBird.GameState.OVER:
					GetTree().ChangeSceneToFile("res://Scenes/Pages/Menu.tscn");
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
