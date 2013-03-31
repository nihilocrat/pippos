using UnityEngine;
using System.Collections;

public class GameSettings
{
	public static bool[] activePlayers = new bool[4];
	public static Material deadMaterial;
	
	public static void Init()
	{
		deadMaterial = Resources.Load("Materials/wall_p0") as Material;
	}
	
	public static bool IsDebug()
	{
		int players = 0;
		for(int i=0; i<4; i++)
		{
			if(activePlayers[i])
			{
				players += 1;
			}
		}
		
		if(players <= 1)
		{
			return true;
		}
		
		return false;
	}
}
