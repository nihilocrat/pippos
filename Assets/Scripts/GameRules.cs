using UnityEngine;
using System.Collections;

public class GameRules : MonoBehaviour
{
	public Transform gameoverFX;
	public AudioClip deadSound;
	public PongDude[] players = new PongDude[4];
	public bool[] activePlayers = new bool[4];
	
	private float gameOverTime = 3.0f;
	private Material deadMaterial;
	
	void Start ()
	{
		GameSettings.Init();
		
		if(!GameSettings.IsDebug())
		{
			activePlayers = GameSettings.activePlayers;
		}
		
		gameoverFX.gameObject.SetActive(false);
		
		for(int i=0; i<4; i++)
		{
			if(!activePlayers[i])
			{
				players[i].transform.parent.renderer.sharedMaterial = GameSettings.deadMaterial; 
				Destroy(players[i].gameObject);
			}
		}
	}
	
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel(0);
		}
	}
	
	public void OnPlayerDeath(int playerNum)
	{
		playerNum -= 1;
		activePlayers[playerNum] = false;
		
		int living = 0;
		int winnarNum = 0;
		for(int i=0; i < activePlayers.Length; i++)
		{
			if(activePlayers[i])
			{
				winnarNum = i+1;
				living += 1;
			}
		}
		
		audio.PlayOneShot(deadSound);
		
		if(living <= 1)
		{
			// WINNAR!
			OnGameOver(winnarNum);
		}
	}
	
	void OnGameOver(int winnarNum)
	{
		Debug.Log("WINNAR : " + winnarNum);
		
		StartCoroutine(doGameOver());
	}
	
	IEnumerator doGameOver()
	{
		gameoverFX.gameObject.SetActive(true);
		yield return new WaitForSeconds(gameOverTime);
		Application.LoadLevel(0);
	}
}
