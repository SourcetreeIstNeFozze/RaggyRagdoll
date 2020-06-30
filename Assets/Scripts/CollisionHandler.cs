using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CollisionHandler : MonoBehaviour
{
	Settings settings { get { return Settings.instance; } }
	CollisionManager collisionManager { get { return CollisionManager.instance; } }

	[Header("Ground Detection")]
	public bool touchesGround;
	public System.Action OnTouchedGround;
	public System.Action OnLeftGound;
	public System.Action OnLeftBounds;
	public System.Action OnKickTriggerEntered;
	public System.Action<Collision> OnTouchedOtherPlayer;
	public System.Action<Collision> OnWasWouchedByOtherPlayer;

	[Space]

	[Tooltip("transform used for traking the movement vector of this object")]
	public Transform trackingpoint;
	private Vector3 _lastPosition;
	private Vector3 _currentposition;
	[Tooltip("how much strength should be applied when THIS rigidbody hits another")]
	public float applidedStrenght;

	[Header("References")]
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public Collider collider;

	public PlayerInputController thisPlayer;
	public PlayerInputController otherPlayer;
	[Header("Joint Softening")]
	[HideInInspector] public Joint joint;
	[HideInInspector] public float originalSpringforce;
	public float jointWeakeningTimer = 0;

	void Awake()
	{
		rigid = this.GetComponent<Rigidbody>();
		collider = this.GetComponent<Collider>();
		joint = this.GetComponent<Joint>();
		if (joint != null && joint is HingeJoint)
		{
			HingeJoint hj = (HingeJoint)joint;
			originalSpringforce = hj.spring.spring;
		}

		OnKickTriggerEntered += () => { Debug.Log("Shell Collision"); };



	}

	// Start is called before the first frame update
	void Start()
	{
		applidedStrenght = settings.otherPartsAdditionalForce;

		if (trackingpoint == null)
		{
			trackingpoint = this.transform;
		}

		_lastPosition = _currentposition = trackingpoint.position;

		//// Dynamically assign collision references
		//if (transform.root.tag.Equals("player_left"))
		//{
		//    thisPlayer = Settings.instance.LEFT.GetComponent<PlayerInputController>();
		//    otherPlayer = Settings.instance.RIGHT.GetComponent<PlayerInputController>();
		//}
		//else
		//{
		//    thisPlayer = Settings.instance.RIGHT.GetComponent<PlayerInputController>();
		//    otherPlayer = Settings.instance.LEFT.GetComponent<PlayerInputController>();
		//}

	}

	// Update is called once per frame
	void Update()
	{
		_lastPosition = _currentposition;
		_currentposition = trackingpoint.position;

		// MANAGE JOINT SOFTENING
		if (settings.jointsWeakening == Settings.JointWeakening.gradualReturn)
		{
			if (jointWeakeningTimer > 0)
			{
				jointWeakeningTimer -= Time.deltaTime;

				if (jointWeakeningTimer <= 0)
				{
					jointWeakeningTimer = 0f;

				}
				else
				{
					float minSpring = settings.jointSpringForceWhenWeek;
					float maxSpring = originalSpringforce;
					SetSpringForce(joint, minSpring + ((settings.durationOfJointWeakness - jointWeakeningTimer) / settings.durationOfJointWeakness) * (maxSpring - minSpring));

				}
			}
		}
		else if (settings.jointsWeakening == Settings.JointWeakening.instantReturn)
		{
			float minSpring = settings.jointSpringForceWhenWeek;
			float maxSpring = originalSpringforce;

			if (jointWeakeningTimer > 0)
			{
				jointWeakeningTimer -= Time.deltaTime;
				
				if (jointWeakeningTimer <= 0)
				{
					jointWeakeningTimer = 0f;
					SetSpringForce(joint, maxSpring);
				}
				else
				{
					SetSpringForce(joint, minSpring);
				}
			}
		}


		////ORIENTATION LOSS

		//if (settings.loseOrientationOnCollision)
		//{
		//	if (jointWeakeningTimer > 0)
		//	{
		//		thisPlayer.activeAvatar.balance.SetLookAt(false);
		//	}
		//	{
		//		thisPlayer.activeAvatar.balance.SetLookAt(true);
		//	}
		//}
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
		//if (!collision.gameObject.tag.Equals("Environment"))
		//Debug.Log($"collision: from {collision.gameObject.name} on {collision.transform.root} to {this.gameObject.name} on {this.gameObject.transform.root}");

		// GROUND DETECTION
		if (collision.collider.tag == "Environment")
		{
			touchesGround = true;
			OnTouchedGround?.Invoke();
		}

		// COLLISION WITH OTHER PLAYER
		if (collision.gameObject.tag.Contains("Player"))
		{
			CollisionHandler otherCollisionHandler = collision.gameObject.GetComponent<CollisionHandler>();

			if (otherCollisionHandler != null)
			{
				if (otherPlayer.activeAvatar.childHandlers.Contains(otherCollisionHandler))
				{
					thisPlayer.timeSinceLastContact = 0;

					// if this is the faster object ie object performing the hit
					if (rigid.velocity.sqrMagnitude > otherCollisionHandler.rigid.velocity.sqrMagnitude)
					{
						OnTouchedOtherPlayer?.Invoke(collision);

						otherCollisionHandler.SetJointSofteningTime(settings.durationOfJointWeakness);
						AmplifyCollision(collision, this, otherCollisionHandler);

					}
					// IF this is the object receiving the hit
					else
					{
						OnWasWouchedByOtherPlayer?.Invoke(collision);


					}
				}

			}
		}
	}

	private void AmplifyCollision(Collision collision, CollisionHandler hittingHandler, CollisionHandler hitHandler)
	{

		//GET HIT VECTOR and FORCE
		Vector3 hitDirection = Vector3.zero;
		float hitMagnitude = 0f;

		if (settings.velocityMode == Settings.VelocityMode.accuratePhysics)
		{
			// use the actual velocity of the 
			hitDirection = hittingHandler.rigid.velocity.normalized;
			hitMagnitude = hittingHandler.rigid.velocity.magnitude;
		}

		else if (settings.velocityMode == Settings.VelocityMode.actualVelocityWrongDirection)
		{
			Vector3 vecBetweenPlayers = hitHandler.thisPlayer.activeAvatar.playerRoot.transform.position - hittingHandler.thisPlayer.activeAvatar.playerRoot.transform.position;
			hitDirection = vecBetweenPlayers.normalized;

			hitMagnitude = hittingHandler.rigid.velocity.magnitude;
		}

		else if (settings.velocityMode == Settings.VelocityMode.velocityAndDirectionPulledFromYourAss)
		{
			Vector3 vecBetweenPlayers = hitHandler.thisPlayer.activeAvatar.playerRoot.transform.position - hittingHandler.thisPlayer.activeAvatar.playerRoot.transform.position;
			hitDirection = vecBetweenPlayers.normalized;

			Vector3 vecBetweenHittingObjectAndPlayerRoot = hittingHandler.thisPlayer.activeAvatar.playerRoot.transform.position - hittingHandler.gameObject.transform.position;
			hitMagnitude = Mathf.Pow(vecBetweenHittingObjectAndPlayerRoot.magnitude, 2);
		}

		Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + hitDirection * 10f, Color.blue, 3f);


		// ACTUALLY AMPLIFY COLLISION

		if (settings.colisionAmplificationMode == Settings.ColisionAmplificationMode.velocityAddition)
		{
			hitHandler.rigid.AddForceAtPosition(
				hittingHandler.applidedStrenght * hitDirection * hitMagnitude, // * collisionAmplifier.GetMovementVector(),
				collision.contacts[0].point,
				ForceMode.VelocityChange);

			Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + hittingHandler.applidedStrenght * hitDirection * hitMagnitude, Color.red, 3f);

			//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + collisionAmplifier.GetComponent<Rigidbody>().velocity * collisionAmplifier.applidedStrenght, Color.red, 3f);
			//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + Vector3.right * 5, Color.black);
			//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + Vector3.forward * 5, Color.black);
		}

		if (settings.colisionAmplificationMode == Settings.ColisionAmplificationMode.velocityAddition)
		{
			hitHandler.rigid.AddForceAtPosition(
				hittingHandler.applidedStrenght * hitDirection * hitMagnitude, // * collisionAmplifier.GetMovementVector(),
				collision.contacts[0].point,
				ForceMode.VelocityChange);
		}

		else if (settings.colisionAmplificationMode == Settings.ColisionAmplificationMode.velocityChange)
		{
			hittingHandler.rigid.velocity = hittingHandler.applidedStrenght * hittingHandler.rigid.velocity;
		}
	}

	//private void SoftenJoint(CollisionHandler hitHandler)
	//{
	//	if (settings.jointsWeakening == Settings.JointWeakening.instantReturn)
	//		StartCoroutine(SoftenJointForTime(hitHandler, 3f));

	//	else if (settings.jointsWeakening == Settings.JointWeakening.gradualReturn)
	//		StartCoroutine(SoftenJointsOverTime(hitHandler, 3f));
	//}

	private void SetJointSofteningTime(float time)
	{
		jointWeakeningTimer = time;
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


	private void SetSpringForce(Joint joint, float springToSet)
	{
		if (joint is HingeJoint)
		{
			HingeJoint hingeJoint = (HingeJoint)joint;
			JointSpring jointSpring = hingeJoint.spring;
			jointSpring.spring = springToSet;
			hingeJoint.spring = jointSpring;
		}
	}


	//private IEnumerator SoftenJointForTime(CollisionHandler jointHandler, float timeToStiffen)
	//{
	//	SetSpringForce(jointHandler.joint, settings.jointSpringForceWhenWeek);

	//	yield return new WaitForSeconds(timeToStiffen);

	//	SetSpringForce(jointHandler.joint, jointHandler.originalSpringforce);
	//}

	//private IEnumerator SoftenJointsOverTime(CollisionHandler jointHandler, float timeToStiffen)
	//{
	//	float minSpring = settings.jointSpringForceWhenWeek;
	//	float maxSpring = jointHandler.originalSpringforce;
	//	float timer = 0f;

	//	while (timer < timeToStiffen)
	//	{
	//		timer += Time.deltaTime;
	//		SetSpringForce(jointHandler.joint, minSpring + (timer / timeToStiffen) * (maxSpring - minSpring));
	//		yield return null;
	//	}

	//	SetSpringForce(jointHandler.joint, maxSpring);

	//}
}




