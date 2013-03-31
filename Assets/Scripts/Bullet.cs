using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Pushable"))
		{
			// transfer momentum then disable myself
			// we are bypassing builtin physics, because we only want to push the object in one exact direction
			other.rigidbody.AddForce(rigidbody.velocity);
			Destroy(gameObject);
		}
	}
}
