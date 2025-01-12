using Godot;
using System.Linq;


public partial class Pole : Area2D
{

	// used to determine when to spawn new pole
	private static double _lastSpawnTime = 10.0;
	private static double _spawnCounter = 0.0;

	private static float width;

	private Sprite2D sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// sprite = (Sprite2D)GetNode<Node>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!FlappyBird.IsStateEqual(FlappyBird.GameState.PLAYING))
			return;
			
		if(!Visible) return;
		Position -= new Vector2((float)(FlappyBird.Speed * delta), 0.0f);
		if(Position.X <= -width && Visible) {
			Bird.Score++;
			var label = GetParent().GetParent().GetNode<Label>("CurrentScore");
			label.Text = Bird.Score.ToString();
			Game.AnimateCurrentScore(ref label);
			Visible = false;
		}
	}

	public void SetSize(Vector2 desiredSize, bool shouldRotate = false)
	{
		var oldSize = new Vector2(23, 159);	// size of the rect region
		Scale = desiredSize / oldSize;
		Rotation = 0;
		Position = new Vector2(FlappyBird.WindowSize.X + width, desiredSize.Y * 0.5f);
		if(shouldRotate)
		{
			Rotation = 3.14159f;
			Position = new Vector2(FlappyBird.WindowSize.X + width, FlappyBird.WindowSize.Y - desiredSize.Y * 0.5f);
		}
	}

	public static void Spawn(double delta, ref Node pole)
	{
		_spawnCounter += delta;
		if(_spawnCounter < _lastSpawnTime) return;

		_spawnCounter = 0.0;
		_lastSpawnTime = FlappyBird.GetRandRange(5, 10);
		
		double maxHeight = FlappyBird.WindowSize.Y * 0.82;
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
		width = FlappyBird.WindowSize.Y * 0.15f;
		var poles = pole.GetChildren();
		foreach(Pole p in poles.Cast<Pole>())
			p.Visible = false;
	}

}
