using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
	public int playerNum = 0;
	public bool randomize = true;
	
	// Use this for initialization
	void Start ()
	{	
		if(randomize)
		{
			// HACK GAAAH
			int totalPlayers = 4;
	
			playerNum = Random.Range(0,totalPlayers) + 1;
			renderer.sharedMaterial = Resources.Load("Materials/block_p" + playerNum) as Material;
		}
	}
	
	void OnKill()
	{
		Destroy(gameObject);
	}
}
