using UnityEngine;
using System.Collections;

public enum Orientation
{
	Horizontal,
	Vertical
}

public class PongDude : MonoBehaviour
{
	public int playerNum = 0;
	public Orientation orientation = Orientation.Vertical;
	public float moveDirection = 1.0f;
	public GameObject bulletPrefab;
	public GameObject ballPrefab;
	public Material playerMaterial;
	public AudioClip scoreSound;
	public AudioClip shootSound;
	public AudioClip ballSound;
	
	// pongdude-global settings
	private float moveSpeed = 5.0f;
	private float bulletForce = 500.0f;
	private float bulletLife = 0.6f;
	private float shootCooldown = 0.1f;
	private float ballForce = 50.0f;
	private float lateralForce = 10.0f;
	
	private float ballCharge = 0.0f;
	private float ballChargeDelay = 2.0f;
	private Transform ballChargeProgress;
	private Vector3 progressOrigin;
	
	private int maxLives = 3;
	private int lives;
	private bool ready = true;
	private Vector3 moveVec = Vector3.zero;
	
	private CharacterController motor;
	private Transform hotspot;
	private GameRules gameRules;
	
	void Awake ()
	{
		motor = GetComponent<CharacterController>();
		hotspot = transform.Find("hotspot");
		ballChargeProgress = transform.Find("charge_progress");
		progressOrigin = ballChargeProgress.localPosition;
		gameRules = FindObjectOfType(typeof(GameRules)) as GameRules;
		
		lives = maxLives;
	}
	
	// Update is called once per frame
	void Update ()
	{
		string orientationStr = orientation.ToString();
		
		float moveAmt = Input.GetAxis(orientationStr + "_p" + playerNum) * moveSpeed;
		/*
		if(orientation == Orientation.Horizontal)
		{
			moveVec = new Vector3(moveAmt, 0.0f, 0.0f);
		}
		else
		{
			moveVec = new Vector3(0.0f, 0.0f, moveAmt);
		}
		*/
		//transform.Translate(new Vector3(-moveAmt, 0.0f, 0.0f));
		moveVec = transform.right * moveDirection * moveAmt;
		var flags = motor.Move(moveVec * Time.deltaTime);
		
		// if I'm not moving, I need to reset my moveVec since ball shooting relies on it
		if(flags != CollisionFlags.None)
		{
			moveVec = Vector3.zero;
		}
		
		// shoot out a push beam
		if(Input.GetButton("Blast_p"+playerNum))
		{
			if(ready)
			{
				DoShoot();
			}
		}
		else if(Input.GetButton("Pickup_p"+playerNum) && ready)
		{
			ballCharge += Time.deltaTime;
			
			if(!ballChargeProgress.gameObject.activeInHierarchy)
			{
				ballChargeProgress.gameObject.SetActive(true);
			}
			
			ballChargeProgress.localPosition = Vector3.Lerp(progressOrigin, hotspot.localPosition, (ballCharge/ballChargeDelay));
			
			if(ballCharge >= ballChargeDelay)
			{
				DoThrowBall();
				ResetProgress();
				ready = false;
			}
		}
		
		// ugh, this is ugly
		if((!Input.GetButton("Pickup_p"+playerNum) && ballCharge > 0.0f) ||
			(Input.GetButton("Blast_p"+playerNum) && ballCharge > 0.0f))
		{
			ResetProgress();
		}
		
		if(Input.GetButtonUp("Pickup_p"+playerNum))
		{
			ready = true;
		}
	}
	
	void ResetProgress()
	{
		ballCharge = 0.0f;
		ballChargeProgress.localPosition = progressOrigin;
		ballChargeProgress.gameObject.SetActive(false);
	}
	
	void DoShoot()
	{
		audio.PlayOneShot(shootSound);
		
		var bulletObj = Instantiate(bulletPrefab, hotspot.position, hotspot.rotation) as GameObject;
		//Physics.IgnoreCollision(bulletObj.collider, collider);
		bulletObj.rigidbody.AddForce(transform.forward * bulletForce);
		bulletObj.rigidbody.AddTorque(Vector2.up * bulletForce);
		
		bulletObj.renderer.sharedMaterial = playerMaterial;
		
		iTween.FadeTo(bulletObj, 0.1f, bulletLife);
		Destroy(bulletObj, bulletLife);
		
		ready = false;
		Invoke("OnReady", shootCooldown);
	}
	
	void OnReady()
	{
		ready = true;
	}
	
	void DoThrowBall()
	{
		audio.PlayOneShot(ballSound);
		
		var ballObj = Instantiate(ballPrefab, hotspot.position, hotspot.rotation) as GameObject;
		//Physics.IgnoreCollision(bulletObj.collider, collider);
		ballObj.rigidbody.AddForce(transform.forward * ballForce);
		ballObj.rigidbody.AddForce(moveVec * lateralForce);
	}
	
	void OnScore(int amount)
	{
		audio.PlayOneShot(scoreSound);
		
		lives -= amount;
		for(int i=1; i<=maxLives; i++)
		{
			if(i <= lives)
			{
				transform.Find("life_" + i).gameObject.SetActive(true);
			}
			else
			{
				transform.Find("life_" + i).gameObject.SetActive(false);
			}
		}
		
		if(lives <= 0)
		{
			OnKill();
		}
	}
	
	void OnKill()
	{
		gameRules.OnPlayerDeath(playerNum);
		
		transform.parent.renderer.sharedMaterial = GameSettings.deadMaterial;
		
		Destroy(gameObject);
	}
}
