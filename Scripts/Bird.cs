using Godot;
using System;

public partial class Bird : Area2D
{
	public static float Speed { get; private set; }
	public static float Distance{ get; private set; }
	public static int BestScore{ get; set; }
	public static int Score{ get; set; }
	
	private Vector2 _weight, _velocity, _size;

	private AnimatedSprite2D sprite;
	private Game parentNode;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_weight = new(0, 400);
		_velocity = new(0, 0);
		Position = FlappyBird.WindowSize * new Vector2(0.3f, 0.5f);
		sprite = (AnimatedSprite2D)GetNode<Node>("AnimatedSprite2D");

		Vector2 spriteSize = new(20, 18);
		float width = FlappyBird.WindowSize.X * 0.15f;
		float height = width * (spriteSize.X / spriteSize.Y);
		_size = new(width, height);
		sprite.Scale = _size / spriteSize;

		Connect(Area2D.SignalName.AreaEntered, new Callable(this, nameof(OnAreaEntered)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!FlappyBird.IsStateEqual(FlappyBird.GameState.PLAYING))
		{
			sprite.Animation = "idle";
			return;
		}
		
		sprite.Animation = "default";

		if(Input.IsActionJustReleased("tap"))
		{
			float impulse = -10000;
			// v = It / mass; Assuming mass = 1
			_velocity = new Vector2(0, impulse * (float)delta);
		}

		_velocity += _weight * (float)delta;
		Position += _velocity * (float)delta;
		Distance += Speed * (float)delta;

		if(Position.Y - _size.Y * 0.5f <= 0)
			Position = new(Position.X, _size.Y * 0.5f);
	}


	private void OnAreaEntered(Area2D area)
	{
		var name = area.Name.ToString();
		string[] collideables = new[]{ "Floor", "Pole" };
		foreach(var collideable in collideables)
		{
			if(name.StartsWith(collideable)) {
				if(collideable == "Floor") 
					Position = new(Position.X, Floor.Pos.Y - _size.Y * 0.5f);

				FlappyBird.SetState(FlappyBird.GameState.OVER);
				var gameOverNode = GetParent().GetNode<Node>("GameOver");
				Game.ToggleGameOver(ref gameOverNode);

				BestScore = Math.Max(BestScore, Score);

				var score = gameOverNode.GetNode<Label>("Control/Score");
				var bestScore = gameOverNode.GetNode<Label>("Control/BestScore");
				score.Text = Bird.Score.ToString();
				bestScore.Text = Bird.BestScore.ToString();
				
				return;
			}
		}
	}


	public static void Reset()
	{
		Speed = 100.0f;
		Distance = 0.0f;
		Score = 0;
		BestScore = Math.Max(BestScore, Score);
	}
}
