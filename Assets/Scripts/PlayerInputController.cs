using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

	[Header("Config")]
	public bool useFK;
    public bool useTriggersCurl = true;
	public bool invertControlls;
	public bool amplifyJump;
	public bool bendingCoupledToMovement;


	[Header("Body")]
	public ActiveFinger indexFinger;
	public ActiveFinger middleFinger;
	public CollisionHandler[] wristColliders;

	[Header("References")]
	public Animator handAnimator;
	public HingeJoint torsoJoint;
	public GameObject playerRoot;	
	CollisionHandler[] playerColliders;
	public OrientationAndBalance balance;
	public PlayerInputController otherPlayer;

	[Header("Input")]
	public float rotationSpeed;
	public float maxRotation = 50;

	[Space]
	private bool _rightBumperHeld;
	private bool _leftBumperHeld;

	[Header("Physics Correction")]
	private Rigidbody playerRigidbody;
	private ConstantForce playerConstanctForce;
	public float maxForceInFlight = 10;
	public float jumpForce = 10;

	[Header("Detect fast stick releases")]
	public float stickReleaseTimeWindow = 0.1f;

	[Header("Interaction with other player")]
	public float timeSinceLastContact;
	public System.Action onSideStep;
	public System.Action oncontactWithOtherPlayer;


    Vector3 invertX = new Vector3(-1f, 1f);

	[Header("Resetting")]
	List<Transform> childTransofrms = new List<Transform>();
	List<Quaternion> originalRotations = new List<Quaternion>();
	List<Vector3> originalPositions = new List<Vector3>();

	public enum GroundedState
	{
		inAir,
		bothFeetOnTheFloor,
		rightFootOnTheFloor,
		leftFootOnThefloor,
		transitioning
	}

	private void Awake()
	{
		InitializeColliders();

		middleFinger.stickInput = new StickInput();
		indexFinger.stickInput = new StickInput();

		// get initial state the hand
		foreach (Transform child in transform)
		{
			childTransofrms.Add(child);
			originalRotations.Add(child.rotation);
			originalPositions.Add(child.position);
		}

	}

	// Start is called before the first frame update
	void Start()
	{
		playerRigidbody = playerRoot.GetComponent<Rigidbody>();
		playerConstanctForce = playerRoot.GetComponent<ConstantForce>();

		// WIRE EVENTS
		

		if (amplifyJump)
		{
			middleFinger.stickInput.OnReleased_Y += () =>
			{  if (GetGroundedState() !=  GroundedState.inAir)
				{
					Debug.Log("RY released");
					Jump(playerRoot.transform.up, jumpForce);
				}
			};

			middleFinger.stickInput.OnReleased_X += () =>
			{
				Debug.Log("RX released");
			};

			indexFinger.stickInput.OnReleased_Y += () =>
			{
				if ( GetGroundedState() != GroundedState.inAir)
				{
					Debug.Log("LY released");
					Jump(playerRoot.transform.up, jumpForce);
				}
			};
			indexFinger.stickInput.OnReleased_X += () =>
			{
				Debug.Log("LX released");
			};
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Reset();
		}

		// timers
		timeSinceLastContact += Time.deltaTime;

		// gather data

		middleFinger.stickInput.Update();
		indexFinger.stickInput.Update();

		middleFinger.stickInput.CheckForFastStickReleases(stickReleaseTimeWindow, 1f, 0f, stickReleaseTimeWindow);
		indexFinger.stickInput.CheckForFastStickReleases(stickReleaseTimeWindow, 1f, 0f, stickReleaseTimeWindow);

		if (useFK)
		{
			//MANAGE POSES 
			handAnimator.SetFloat("XInput_R", middleFinger.stickInput.value.x);
			handAnimator.SetFloat("YInput_R", middleFinger.stickInput.value.y);
			handAnimator.SetFloat("XInput_L", indexFinger.stickInput.value.x);
			handAnimator.SetFloat("YInput_L", indexFinger.stickInput.value.y);


			//MANAGE BENDING AND PUSHING

			float bendDirection;

			if (_rightBumperHeld && !_leftBumperHeld)
			{
				bendDirection = -1f;
			}
			else if (!_rightBumperHeld && _leftBumperHeld)
			{
				bendDirection = 1;
			}
			else
			{
				bendDirection = 0f;
			}

			if (invertControlls)
			{
				bendDirection *= -1f;
			}

			//2.BENDING
			// Bend the body if getting input and on the floor 
			if (!(GetGroundedState() == GroundedState.inAir))
			{
				BendVertically(bendDirection * rotationSpeed);
			}
			// When in air push the player
			else if (GetGroundedState() == GroundedState.inAir)
			{
				SetPlayerPushForce(new Vector3(bendDirection, 0, 0), maxForceInFlight);

			}
			else
			{
				SetPlayerPushForce(Vector3.zero, 0);
			}

		}


		else
		{

			// MANAGE BENDING AND MOVEMENT

			// Bend the body if getting input and on the floor 
			if (indexFinger.stickInput.value.x != 0f && !(GetGroundedState() == GroundedState.inAir))
			{
				BendVertically(indexFinger.stickInput.value.x * rotationSpeed * (invertControlls ? -1 : 1));
			}
			// When in air push the player
			else if (indexFinger.stickInput.value.x != 0f && GetGroundedState() == GroundedState.inAir)
			{
				SetPlayerPushForce(new Vector3(indexFinger.stickInput.value.x, 0, 0), maxForceInFlight);

			}
			else
			{
				SetPlayerPushForce(Vector3.zero, 0);
			}
		}


	}

	public void BendVertically(float bendValue)
	{
		JointSpring spring = torsoJoint.spring;
		spring.targetPosition = Mathf.Clamp(spring.targetPosition + bendValue, -maxRotation, maxRotation);
		torsoJoint.spring = spring;
	}

	public void SetPlayerPushForce(Vector3 direction, float constancforceValue)
	{
		direction = direction.normalized;
		playerConstanctForce.force = direction * constancforceValue;
	}

	public void Jump(Vector3 direction, float forceValue)
	{
		Debug.Log("Jump");
		direction = direction.normalized;
		playerRigidbody.AddForce(direction * forceValue, ForceMode.Impulse);
	}

	// TO DO add some delays and stuff
	private GroundedState GetGroundedState()
	{
		if (indexFinger.fingerBottom.touchesGround && middleFinger.fingerBottom.touchesGround)
		{
			return GroundedState.bothFeetOnTheFloor;
		}
		else if (indexFinger.fingerBottom.touchesGround)
		{
			return GroundedState.leftFootOnThefloor;
		}
		else if (middleFinger.fingerBottom.touchesGround)
		{
			return GroundedState.rightFootOnTheFloor;
		}
		else
		{
			return GroundedState.inAir;
		}
	}

	// --- ACTION FUNCTIONS ---//

	public void OnIndexFingerUP()
	{
		if (!useFK)
			handAnimator.Play("IndexUP", 2);

        _rightBumperHeld = true;
    }

	public void OnIndexFingerDOWN()
	{
        if (!useFK)
            handAnimator.Play("IndexDOWN", 2);

       _rightBumperHeld = false;
	}

	public void OnIndexFingerCurlIN()
	{
		if (!useFK || useTriggersCurl)
		{
			handAnimator.Play("IndexCURLin", 3);
		}
	}

	public void OnIndexFingerCurlOUT()
	{
		if (!useFK || useTriggersCurl)
		{
			handAnimator.Play("IndexCURLout", 3);
		}
	}

	public void OnMiddleFingerUP()
	{
		if (!useFK)
			handAnimator.Play("MiddleUP", 4);

        _leftBumperHeld = true;
    }

	public void OnMiddleFingerDOWN()
	{
		if (!useFK)
			handAnimator.Play("MiddleDOWN", 4);

        _leftBumperHeld = false;
    }

	public void OnMiddleFingerCurlIN(InputValue value)
	{
		if (!useFK || useTriggersCurl)
		{
			handAnimator.Play("MiddleIN", 5);
            print("middleCurlIn; value: " + value.Get<float>());
		}
	}

	public void OnMiddleFingerCurlOUT()
	{
		if (!useFK || useTriggersCurl)
		{
			handAnimator.Play("MiddleOUT", 5);
		}
	}

	public void OnLeftStick(InputValue value)
	{
		indexFinger.stickInput.value = value.Get<Vector2>();

        if (this.tag.Equals("player_right"))
			indexFinger.stickInput.value *= invertX;

	}

	public void OnRightStick(InputValue value)
	{
		middleFinger.stickInput.value = value.Get<Vector2>();

        if (this.tag.Equals("player_right"))
			middleFinger.stickInput.value *= invertX;

    }

	private void InitializeColliders()
	{
        playerColliders = this.GetComponentsInChildren<CollisionHandler>();
        for (int i = 0; i < playerColliders.Length; i++)
		{
			playerColliders[i].thisPlayer = this;
			playerColliders[i].otherPlayer = otherPlayer;

		}
	}

	public void Reset()
	{
		for (int i = 0; i < childTransofrms.Count; i++)
		{
			childTransofrms[i].position = originalPositions[i];
			childTransofrms[i].rotation = originalRotations[i];

			Rigidbody childRigidbody = childTransofrms[i].GetComponent<Rigidbody>();
			if (childRigidbody != null)
			{
				childRigidbody.velocity = Vector3.zero;
			}
		}
	}


}

