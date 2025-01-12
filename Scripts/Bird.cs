using Godot;
using System;

public partial class Bird : Area2D
{
	private int score = 0;
	private const float jumpForce = 200.0f;
	private Vector2 weight;
	private Vector2 velocity;

	private Sprite2D sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = 0;
		weight = new(0, 100);
		velocity = new(0, 0);
		Position = new(FlappyBird.Width * 0.30f, Position.Y);
		sprite = (Sprite2D)GetNode<Node>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustReleased("space"))
		{
			velocity.Y -= jumpForce;	
		}
		velocity += weight * (float)delta;
		Position += velocity * (float)delta;

		if(Position.Y - sprite.Texture.GetSize().Y <= 0)
		{
			Position = new(Position.X, sprite.Texture.GetSize().Y);
		}
		
	}

	public void Reset()
	{

	}
}
