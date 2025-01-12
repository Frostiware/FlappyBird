using Godot;


public partial class Pole : Sprite2D
{

	private static float width;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!Visible) return;
		Position -= new Vector2((float)(FlappyBird.Speed * delta), 0.0f);
		if(Position.X <= -width && Visible)
			Visible = false;
	}

	public void SetSize(Vector2 desiredSize, bool shouldRotate = false)
	{
		var oldSize = new Vector2(23, 159);	// size of the rect region
		Scale = desiredSize / oldSize;
		Rotation = 0;
		Position = new Vector2(FlappyBird.Width + width, desiredSize.Y * 0.5f);
		if(shouldRotate)
		{
			Rotation = 3.14159f;
			Position = new Vector2(FlappyBird.Width + width, FlappyBird.Height - desiredSize.Y * 0.5f);
		}
	}

	public static void Spawn(ref Node pole)
	{
		
		double maxHeight = FlappyBird.Height * 0.85;
		float h1 = (float)FlappyBird.GetRandRange(maxHeight * 0.1, maxHeight * 0.9);
		float h2 = (float)maxHeight - h1;

		var poles = pole.GetChildren();
		int c = 0;
		for(int i = 0; i < poles.Count; i++)
		{
			if(c > 1) break;
			if(((Pole)poles[i]).Visible) continue;

			((Pole)poles[i]).Visible = true;
			Vector2 desiredSize = new(width, 1);
			if(c == 0) 
			{
				desiredSize.Y = h1;
				((Pole)poles[i]).SetSize(desiredSize, false);
			} else if(c == 1)
			{
				desiredSize.Y = h2;
				((Pole)poles[i]).SetSize(desiredSize, true);
			}
			c++;
		}
	}


	public static void Init(ref Node pole)
	{
		width = FlappyBird.Width * 0.2f;
		var poles = pole.GetChildren();
		foreach(Pole p in poles)
		{
			p.Visible = false;
		}
	}

}
