using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifyCollision : MonoBehaviour
{
	private Rigidbody thisRigidbody;
	[Tooltip("transform used for traking the movement vector of this object")]
	public Transform trackingpoint;
	private  Vector3 lastPosition;
	private Vector3 currentposition;
	[Tooltip("how much strength should be applied when THIS rigidbody hits anothe")]
	public float applidedStrenght;

	
	// Start is called before the first frame update
    void Start()
    { if (trackingpoint == null)
		{
			trackingpoint = this.transform;
		}
		lastPosition = currentposition = trackingpoint.position;
		thisRigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		lastPosition = currentposition;
		currentposition = trackingpoint.position;
    }

	public Vector3 GetMovementVector()
	{
		return currentposition - lastPosition;
	}

	public Vector3 GetMovementVectorNormalized()
	{
		return (currentposition - lastPosition).normalized;
	}

	private void OnCollisionEnter(Collision collision)
	{
		AmplifyCollision collisionAmplifier = collision.gameObject.GetComponent<AmplifyCollision>();
		if (collisionAmplifier!= null)
		{
			thisRigidbody.AddForceAtPosition(
				collisionAmplifier.applidedStrenght * collisionAmplifier.GetMovementVector(),
				collision.contacts[0].point,
				ForceMode.Impulse);
		}
	}
}
