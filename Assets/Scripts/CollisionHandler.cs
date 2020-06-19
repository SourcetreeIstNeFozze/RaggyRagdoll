using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
	[Header("Ground Detection")]
	public bool touchesGround;
	public System.Action OnTouchedGround;
	public System.Action OnLeftGound;
	public System.Action OnLeftBounds;

	[Space]
	[Header("Collision Amplification")]
	
	[Tooltip("transform used for traking the movement vector of this object")]
	public Transform trackingpoint;
	private Vector3 _lastPosition;
	private Vector3 _currentposition;
	private Rigidbody _thisRigidbody;

	[Tooltip("how much strength should be applied when THIS rigidbody hits another")]
	public float applidedStrenght;

	[HideInInspector] public PlayerInputController thisPlayer;
	[HideInInspector] public PlayerInputController otherPlayer;


	// Start is called before the first frame update
	void Start()
    {
		if (trackingpoint == null)
		{
			trackingpoint = this.transform;
		}
		_lastPosition = _currentposition = trackingpoint.position;
		_thisRigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		_lastPosition = _currentposition;
		_currentposition = trackingpoint.position;
    }

	public Vector3 GetMovementVector()
	{
		return _currentposition - _lastPosition;
	}

	public Vector3 GetMovementVectorNormalized()
	{
		return (_currentposition - _lastPosition).normalized;
	}

	private void OnCollisionEnter(Collision collision)
	{

		// GROUND DETECTION
		if (collision.collider.tag == "Environment")
		{
			touchesGround = true;
			OnTouchedGround?.Invoke();

		}


		if (!collision.gameObject.tag.Equals("Environment"))
		    Debug.Log($"collision: from {collision.gameObject.name} to {this.gameObject.name}");

		if (collision.gameObject.tag.Contains("Player"))
		{
			CollisionHandler collisionHandler = collision.gameObject.GetComponent<CollisionHandler>();

			if (collisionHandler != null)
			{
				// CONTACT WITH OTHE RPLAYER
				if (otherPlayer.playerColliders.Contains(collisionHandler))
				{
					thisPlayer.timeSinceLastContact = 0;
				

					// COLLISION AMPLIFICATION

					// when THIS object is kicked, the force applied to THIS object will be multipied by the appliedStrength of the OTHER OBECT
					// eg when this object is hit by "foot" and foot's applied strength is 50, the strength of this collision will be amplified by 50

					//Debug.Log("Amplifying collision on: " + gameObject.name + "(" + this.transform.root.tag + "), velocity = " + this.GetComponent<Rigidbody>().velocity);
					_thisRigidbody.AddForceAtPosition(
						collisionHandler.applidedStrenght * collisionHandler.GetComponent<Rigidbody>().velocity, // * collisionAmplifier.GetMovementVector(),
						collision.contacts[0].point,
						ForceMode.VelocityChange);

					//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + collisionAmplifier.GetComponent<Rigidbody>().velocity * collisionAmplifier.applidedStrenght, Color.red, 3f);
					//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + Vector3.right*5, Color.black);
					//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + Vector3.forward*5, Color.black);
				}

			}


		}
	}

	private void OnCollisionExit(Collision collision)
	{
		// GROUND DETECTION
		if (collision.collider.tag == "Environment")
		{
			touchesGround = false;
			OnLeftGound?.Invoke();
		}
	}


	private void OnTriggerEnter(Collider collider)
	{
		// GROUND DETECTION
		if (collider.tag == "Environment")
		{
			touchesGround = true;
			OnTouchedGround?.Invoke();
		}

		//BOUNDARY DETECTION
		if (collider.tag == "Boundary")
		{
			OnLeftBounds?.Invoke();
		}
	}
	private void OnTriggerExit(Collider collider)
	{
		// GROUND DETECTION
		if (collider.tag == "Environment")
		{
			touchesGround = false;
			OnLeftGound?.Invoke();
		}
	}
}

