using Godot;
using System;


public class FlappyBird
{
	private static Random random;
	
	public enum GameState
	{
		MENU,
		PAUSED,
		PLAYING,
		OVER
	};
	
	public static GameState _state;
	public static double Speed;
	public static Vector2 WindowSize{ get; set; }

	public static bool IsDay{ get; set; }

	public static double GetRandRange(double min, double max)
	{
		return random.NextDouble() * (max - min + 1) + min;
	}


	public static GameState GetState()
	{
		return _state;
	}

	public static void SetState(GameState state)
	{
		// do not pause what is not playing
		if(state == GameState.PAUSED && _state != GameState.PLAYING)
			return;
		_state = state;
	}

	public static bool IsStateEqual(GameState state)
	{
		return _state == state;
	}


	static FlappyBird()
	{
		random = new Random();
		SetState(GameState.MENU);
		IsDay = true;
		Speed = 40.0;
		WindowSize = new(364, 600);
	}

}
