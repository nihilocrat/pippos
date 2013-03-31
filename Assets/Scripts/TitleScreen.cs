using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
	public Transform[] chargers = new Transform[4];
	public Transform starter;
	public GUIText startText;
	public AudioClip joinSound;
	public AudioClip countSound;
	
	private float startTime = 5.0f;
	private float chargeSpeed = 1.0f;
	private float[] charge = new float[4];
	
	private bool startGame = false;
	private int countdown = 5;
	private float lastCount = 0.0f;
	private int readyPlayers = 0;
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
		for(int p = 1; p <= 4; p++)
		{
			int i = p-1;
			
			if(Input.GetButton("Pickup_p"+p))
			{
				if(charge[i] < 1.0f)
				{
					charge[i] += Time.deltaTime * chargeSpeed;
				}
			}
			else
			{
				if(charge[i] > 0.0f && charge[i] < 1.0f)
				{
					charge[i] -= Time.deltaTime * chargeSpeed * 2.0f;
				}
			}
			
			chargers[i].localScale = Vector3.Lerp(new Vector3(0.0f, 1.0f, 1.0f), Vector3.one, charge[i]);
			
			
			if(charge[i] >= 1.0f && GameSettings.activePlayers[i] == false)
			{
				readyPlayers += 1;
				GameSettings.activePlayers[i] = true;
				audio.PlayOneShot(joinSound);
			}
		}
		
		if(!startGame && readyPlayers > 1)
		{
			// start the game countdown thingamabobber
			//StartCoroutine(doStartGame());
			startGame = true;
			startText.text = countdown.ToString();
			lastCount = Time.time;
			audio.PlayOneShot(countSound);
		}
		
		if(startGame)
		{
			if(Time.time - lastCount > 1.0f)
			{
				countdown -= 1;
				startText.text = countdown.ToString();
				lastCount = Time.time;
				audio.PlayOneShot(countSound);
			}
		}
		if(startGame && countdown <= 0)
		{
			for(int i=0; i < 4; i++)
			{
				if(charge[i] >= 1.0f)
				{
					GameSettings.activePlayers[i] = true;
				}
				else
				{
					GameSettings.activePlayers[i] = false;
				}
			}
			
			Application.LoadLevel(1);
		}
	}
	
	IEnumerator doStartGame()
	{
		iTween.ScaleTo(starter.gameObject, new Vector3(1.0f, 0.0f, 1.0f) * 50.0f, startTime);
		
		yield return new WaitForSeconds(startTime);
		Application.LoadLevel(1);
	}
}
