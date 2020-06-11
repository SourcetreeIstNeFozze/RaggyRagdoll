using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifyCollision : MonoBehaviour
{
	private Rigidbody _thisRigidbody;
	[Tooltip("transform used for traking the movement vector of this object")]
	public Transform trackingpoint;
	private  Vector3 _lastPosition;
	private Vector3 _currentposition;

	[Tooltip("how much strength should be applied when THIS rigidbody hits another")]
	public float applidedStrenght;

	[HideInInspector] public PlayerInputController thisPlayer;
	[HideInInspector] public PlayerInputController otherPlayer;


	// Start is called before the first frame update
	void Start()
    { if (trackingpoint == null)
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
		Debug.Log($"collision: {this.gameObject.name} and {collision.gameObject.name}");




		if (collision.gameObject.tag.Equals("Player"))
		{

			//DESTABILISATION

			thisPlayer.balance.SetFallTimer(5f);
			thisPlayer.balance.SetFallTimer(5f);





			// COLLISION AMPLIFICATION

			// when THIS object is kicked, the force applied to THIS object will be multipied by the appliedStrength of the OTHER OBECT
			// eg when this object is hit by "foot" and foot's applied strength is 50, the strength of this collision will be amplified by 50
			AmplifyCollision collisionAmplifier = collision.gameObject.GetComponent<AmplifyCollision>();
			if (collisionAmplifier != null)
			{
				Debug.Log("Amplifying collision on the object:" + gameObject.name);
				_thisRigidbody.AddForceAtPosition(
					collisionAmplifier.applidedStrenght * collisionAmplifier.GetMovementVector(),
					collision.contacts[0].point,
					ForceMode.Impulse);
			}

		}
	}
}

