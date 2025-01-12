using System;
using System.Linq;


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
	
	public static GameState State;
	public static double Speed;

	public static int Width, Height;

	public static bool IsDay;

	public static double GetRandRange(double min, double max)
	{
		return random.NextDouble() * (max - min + 1) + min;
	}

	static FlappyBird()
	{
		random = new Random();
		State = GameState.MENU;
		IsDay = true;
		Speed = 40.0;
		Width = 364;
		Height = 650;
	}

}
