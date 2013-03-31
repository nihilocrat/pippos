using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	public float life = 10.0f;
	public AudioClip bounceSound;
	
	void Start()
	{
		Hashtable args = new Hashtable()
		{
			{"time", life},
			{"scale", Vector3.zero},
			{"easeType", iTween.EaseType.linear}
		};
		
		var lifeIndicator = transform.Find("indicator").gameObject;
		
		iTween.ScaleTo(lifeIndicator, args);
		Destroy(gameObject, life);
	}
	
	void OnCollisionEnter(Collision col)
	{
		var other = col.gameObject;
		if(other.CompareTag("Goal"))
		{
			// tell the player and the director that this player "scored"
			var pongDude = other.GetComponentInChildren<PongDude>();
			
			if(pongDude != null)
			{
				pongDude.SendMessage("OnScore", 1);
				Destroy(gameObject);
			}
			else
			{
				audio.PlayOneShot(bounceSound);
			}
		}
		else
		{
			audio.PlayOneShot(bounceSound);
		}
	}
}
