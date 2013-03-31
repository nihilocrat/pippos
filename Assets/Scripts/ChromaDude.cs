using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterMotor))]
[AddComponentMenu ("Character/Platform Input Controller")]
public class ChromaDude : MonoBehaviour
{	
	public int playerNum = 0;
	public Block holdingBlock;
	
	private float grabLength = 0.5f;
	private float grabRadius = 0.4f;
	private float holdDistance = 1.0f;
	
	private Detector detector;
	private CharacterMotor motor;
	
	void Awake()
	{
		detector = gameObject.GetComponentInChildren<Detector>();
		motor = gameObject.GetComponent<CharacterMotor>();
	}
	
	void Start ()
	{
	
	}
	
	void Update ()
	{
		// movement
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal_p"+playerNum), 0.0f, Input.GetAxis("Vertical_p"+playerNum));
		motor.inputMoveDirection = directionVector;
		if(directionVector != Vector3.zero)
		{
			transform.LookAt(transform.position + directionVector);
		}
		
		// destroy block in front of me
		if(Input.GetButtonDown("Blast_p"+playerNum))
		{
			if(detector.target != null)
			{
				if(detector.target.GetComponent<Block>().playerNum == playerNum)
				{
					// BLASTO'D!
					detector.target.SendMessage("OnKill");
				}
			}
		}
		// pick up block in front of me, or drop current block in front of me
		if(Input.GetButtonDown("Pickup_p"+playerNum))
		{
			if(holdingBlock != null)
			{
				DoDrop();
			}
			else if(detector.target != null)
			{
				DoPickup(detector.target.GetComponent<Block>());
			}
		}
	}
	
	void FixedUpdate()
	{
		// fun hack: we don't want to have the currently-held block inherit my rotation, so I'll just make sure
		// it's hovering above me at all times
		if(holdingBlock != null)
		{
			holdingBlock.transform.position = transform.position + (Vector3.up * holdDistance);
		}
	}
	
	void DoPickup(Block block)
	{
		holdingBlock = block;
		holdingBlock.collider.enabled = false;
	}
	
	void DoDrop()
	{
		Vector3 dropPos = GetDropPos();
		
		// first, test if there is anything at my drop position
		if(Physics.CheckSphere(dropPos, grabRadius))
		{
			return;
		}
		
		holdingBlock.collider.enabled = true;
		holdingBlock.transform.position = dropPos;
		holdingBlock = null;
	}
	
	public Vector3 GetDropPos()
	{
		if(detector == null)
		{
			return transform.position;
		}
		
		Vector3 dropPos = detector.transform.position;
		dropPos.x = Mathf.Round(dropPos.x);
		dropPos.z = Mathf.Round(dropPos.z);
		return dropPos;
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(GetDropPos(), grabRadius);
	}
}
