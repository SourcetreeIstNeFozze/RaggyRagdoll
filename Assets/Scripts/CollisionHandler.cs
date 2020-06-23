using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollisionHandler : MonoBehaviour
{
	Settings settings { get { return Settings.instance; } }

	[Header("Ground Detection")]
	public bool touchesGround;
	public System.Action OnTouchedGround;
	public System.Action OnLeftGound;
	public System.Action OnLeftBounds;
	public System.Action OnKickTriggerEntered;

	[Space]
	[Header("Collision Amplification")]
	[HideInInspector] public bool shockWave;
	
	[Tooltip("transform used for traking the movement vector of this object")]
	public Transform trackingpoint;
	private Vector3 _lastPosition;
	private Vector3 _currentposition;
	public Rigidbody rigid;

	[Tooltip("how much strength should be applied when THIS rigidbody hits another")]
	public float applidedStrenght;

	[HideInInspector]
    public PlayerInputController thisPlayer = null;
    [HideInInspector]
    public PlayerInputController otherPlayer = null;

	void Awake()
	{
		rigid = this.GetComponent<Rigidbody>();

		OnKickTriggerEntered += () => { Debug.Log("Shell Collision"); };
	}

	// Start is called before the first frame update
	void Start()
    {
		if (trackingpoint == null)
		{
			trackingpoint = this.transform;
		}
		_lastPosition = _currentposition = trackingpoint.position;

        // Dynamically assign collision references
        if (transform.root.tag.Equals("player_left"))
        {
            thisPlayer = Settings.instance.LEFT.GetComponent<PlayerInputController>();
            otherPlayer = Settings.instance.RIGHT.GetComponent<PlayerInputController>();
        }
        else
        {
            thisPlayer = Settings.instance.RIGHT.GetComponent<PlayerInputController>();
            otherPlayer = Settings.instance.LEFT.GetComponent<PlayerInputController>();
        }

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
				if (otherPlayer.activeAvatar.childHandlers.Contains(collisionHandler))
				{
					thisPlayer.timeSinceLastContact = 0;


					// COLLISION AMPLIFICATION

					// when THIS object is kicked, the force applied to THIS object will be multipied by the appliedStrength of the OTHER OBECT
					// eg when this object is hit by "foot" and foot's applied strength is 50, the strength of this collision will be amplified by 50
					if (settings.colisionAmplificationMode == Settings.ColisionAmplificationMode.velocityChange)
					{
						//Debug.Log("Amplifying collision on: " + gameObject.name + "(" + this.transform.root.tag + "), velocity = " + this.GetComponent<Rigidbody>().velocity);
						rigid.AddForceAtPosition(
							collisionHandler.applidedStrenght * collisionHandler.rigid.velocity, // * collisionAmplifier.GetMovementVector(),
							collision.contacts[0].point,
							ForceMode.VelocityChange);

						//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + collisionAmplifier.GetComponent<Rigidbody>().velocity * collisionAmplifier.applidedStrenght, Color.red, 3f);
						//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + Vector3.right*5, Color.black);
						//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + Vector3.forward*5, Color.black);
					}

					else if (settings.colisionAmplificationMode == Settings.ColisionAmplificationMode.velocityChange)
					{
						rigid.velocity = collisionHandler.applidedStrenght * collisionHandler.rigid.velocity;
					}  



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

		//BOUNDARY DETECTION
		if (collider.tag == "Shell")
		{
			CollisionHandler collisionHandler = collider.gameObject.GetComponent<CollisionHandler>();

			if (collisionHandler != null)
			{
				// CONTACT WITH OTHE RPLAYER
				if (otherPlayer.activeAvatar.shokwavetriggers.Contains(collisionHandler))
				{
					OnKickTriggerEntered?.Invoke();
					
				}
			}
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

