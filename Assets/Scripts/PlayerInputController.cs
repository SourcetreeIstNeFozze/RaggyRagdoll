using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

	[Header("Config")]
	public bool useFK;
	public bool invertControlls;
	public bool amplifyJump;
	public bool handCanFall;
	public bool bendingCoupledToMovement;

	[Header("Input")]
	public Animator handAnimator;
	public HingeJoint hipJoint;
	public float rotationSpeed;
	public float maxRotation = 50;
	public Vector2 leftStickInput = new Vector2();
	public Vector2 rightStickInput = new Vector2();
	public bool _rightBumperHeld;
	public bool _leftBumperHeld;

	[Header("Physics Correction")]

	public GameObject PlayerRoot;
	private Rigidbody playerRigidbody;
	private ConstantForce playerConstanctForce;
	public Vector3 playerVelocity;
	[Space]

	[SerializeField] GroundDetector leftFoot;
	[SerializeField] GroundDetector rightFoot;
	[Space]

	public float maxForceInFlight = 10;
	public float jumpForce = 10;
	[Space]


	[Header("bulshit jump parametres")]
	public bool leftKneeCurled = false;
	public bool rightKneeCurled = false;
	public float leftKneeCurlTimer = 0;
	public float rightKneeCurlTimer = 0;
	public float lastLeftKneeCurl = 0;
	public float lastRightKneeCurl = 0;
	public float jumpReactionTime = 0.5f;
	System.Action onLanded;
	public bool onLandedTriggered;
	public bool landed; // this should be done with a property but Im to dumb atm

	[Header("Detect fast stick releases")]
	// to do may need to meke the list have max N items to prevent memory leaks
	public float stickReleaseTimeWindow = 0.1f;
	List<InputTuple> historicLeftStick = new List<InputTuple>();
	List<InputTuple> historicRightStick = new List<InputTuple>();
	System.Action OnReleased_RX;
	System.Action OnReleased_RY;
	System.Action OnReleased_LX;
	System.Action OnReleased_LY;
	float Released_RX_timer;
	float Released_RY_timer;
	float Released_LX_timer;
	float Released_LY_timer;




	public enum GroundedState
	{
		inAir,
		bothFeetonTheFloor,
		rightFootOnTheFloor,
		leftFootOnThefloor,
		transitioning

	}

	// Start is called before the first frame update
	void Start()
	{
		playerRigidbody = PlayerRoot.GetComponent<Rigidbody>();
		playerConstanctForce = PlayerRoot.GetComponent<ConstantForce>();

		CJBalancingwithFalling balance = PlayerRoot.GetComponent<CJBalancingwithFalling>();
		balance.enabled = handCanFall;

		// subscribe to events
		onLanded += () =>
		{
			Debug.Log("Jump");
			Jump(Vector3.up, jumpForce);
		};


		if (amplifyJump)
		{
			OnReleased_RY += () =>
			{  if (Released_RY_timer >= stickReleaseTimeWindow && GetGroundedState() !=  GroundedState.inAir)
				{
					Debug.Log("RY released");
					Released_RY_timer = 0f;
					Jump(PlayerRoot.transform.up, jumpForce);
				}
			};

			OnReleased_RX += () =>
			{
				Debug.Log("RX released");
			};

			OnReleased_LY += () =>
			{
				if (Released_LY_timer >= stickReleaseTimeWindow && GetGroundedState() != GroundedState.inAir)
				{
					Debug.Log("LY released");
					Released_LY_timer = 0f;
					Jump(PlayerRoot.transform.up, jumpForce);
				}
			};
			OnReleased_LX += () =>
			{
				Debug.Log("LX released");
			};
		}
	}

	// Update is called once per frame
	void Update()
	{
		// gather data
		playerVelocity = playerRigidbody.velocity;
		historicRightStick.Add(new InputTuple(rightStickInput, Time.deltaTime));
		historicLeftStick.Add(new InputTuple(leftStickInput, Time.deltaTime));

		// fire Stick release Events
		Released_RX_timer += Time.deltaTime;
		Released_RY_timer += Time.deltaTime;
		Released_LX_timer += Time.deltaTime;
		Released_LY_timer += Time.deltaTime;

		CheckForFastStickReleases(stickReleaseTimeWindow, 1f, 0f, rightStickInput, historicRightStick, OnReleased_RX, OnReleased_RY);
		CheckForFastStickReleases(stickReleaseTimeWindow, 1f, 0f, leftStickInput, historicLeftStick, OnReleased_LX, OnReleased_LY);

		if (useFK)
		{
			//MANAGE POSES 
			handAnimator.SetFloat("XInput_L", leftStickInput.x);
			handAnimator.SetFloat("YInput_L", leftStickInput.y);
			handAnimator.SetFloat("XInput_R", rightStickInput.x);
			handAnimator.SetFloat("YInput_R", rightStickInput.y);

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

			//3.JUMPING

		}


		else
		{
			// 1.MANAGE JUMPS
			if (rightKneeCurled)
			{
				rightKneeCurlTimer += Time.deltaTime;
			}
			if (leftKneeCurled)
			{
				leftKneeCurlTimer += Time.deltaTime;
			}

			if (GetGroundedState() == GroundedState.bothFeetonTheFloor && !onLandedTriggered && lastLeftKneeCurl >= jumpReactionTime && lastRightKneeCurl >= jumpReactionTime)
			{
				onLandedTriggered = true;
				lastRightKneeCurl = 0;
				lastLeftKneeCurl = 0;
				onLanded?.Invoke();
			}
			else if (!(GetGroundedState() == GroundedState.bothFeetonTheFloor))
			{
				onLandedTriggered = false;
			}


			// 2. MANAGE BENDING AND MOVEMENT
			// Bend the body if getting input and on the floor 
			if (leftStickInput.x != 0f && !(GetGroundedState() == GroundedState.inAir))
			{
				BendVertically(leftStickInput.x * rotationSpeed * (invertControlls ? -1 : 1));
			}
			// When in air push the player
			else if (leftStickInput.x != 0f && GetGroundedState() == GroundedState.inAir)
			{
				SetPlayerPushForce(new Vector3(leftStickInput.x, 0, 0), maxForceInFlight);

			}
			else
			{
				SetPlayerPushForce(Vector3.zero, 0);
			}
		}


	}

	public void BendVertically(float bendValue)
	{
		JointSpring spring = hipJoint.spring;
		spring.targetPosition = Mathf.Clamp(spring.targetPosition + bendValue, -maxRotation, maxRotation);
		hipJoint.spring = spring;
	}

	public void SetPlayerPushForce(Vector3 direction, float constancforceValue)
	{
		direction = direction.normalized;
		playerConstanctForce.force = direction * constancforceValue;
	}

	public void Jump(Vector3 direction, float forceValue)
	{
		direction = direction.normalized;
		playerRigidbody.AddForce(direction * forceValue, ForceMode.Impulse);
	}

	// TO DO add some delays and stuff
	private GroundedState GetGroundedState()
	{
		if (leftFoot.touchesGround && rightFoot.touchesGround)
		{
			return GroundedState.bothFeetonTheFloor;
		}
		else if (leftFoot.touchesGround)
		{
			return GroundedState.leftFootOnThefloor;
		}
		else if (rightFoot.touchesGround)
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
			handAnimator.Play("IndexUP", 1);

	}

	public void OnIndexFingerDOWN()
	{
		if (!useFK)
			handAnimator.Play("IndexDOWN", 1);
	}

	public void OnIndexFingerCurledIN()
	{
		if (!useFK)
		{
			handAnimator.Play("IndexIN", 2);
			leftKneeCurled = true;
		}

		_rightBumperHeld = true;
	}

	public void OnIndexFingerCurledOUT()
	{
		if (!useFK)
		{
			handAnimator.Play("IndexOUT", 2);
			leftKneeCurled = false;
			lastLeftKneeCurl = leftKneeCurlTimer;
			leftKneeCurlTimer = 0;
		}

		_rightBumperHeld = false;
	}

	public void OnMiddleFingerUP()
	{
		if (!useFK)
			handAnimator.Play("MiddleUP", 3);
	}

	public void OnMiddleFingerDOWN()
	{
		if (!useFK)
			handAnimator.Play("MiddleDOWN", 3);

	}

	public void OnMiddleFingerCurledIN()
	{
		if (!useFK)
		{
			handAnimator.Play("MiddleIN", 4);
			rightKneeCurled = true;
		}

		_leftBumperHeld = true;
	}

	public void OnMiddleFingerCurledOUT()
	{
		if (!useFK)
		{
			handAnimator.Play("MiddleOUT", 4);
			rightKneeCurled = false;
			lastRightKneeCurl = rightKneeCurlTimer;
			rightKneeCurlTimer = 0;
		}

		_leftBumperHeld = false;
	}

	public void OnLeftStick(InputValue value)
	{
		leftStickInput = value.Get<Vector2>();
		if (invertControlls)
		{
			leftStickInput = leftStickInput * new Vector2(-1f, 1f);
		}

	}

	public void OnRightStick(InputValue value)
	{
		rightStickInput = value.Get<Vector2>();
		if (invertControlls)
		{
			rightStickInput = rightStickInput * new Vector2(-1f, 1f);
		}

	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="timetoCheck"> how much into history to look for spikes</param>
	/// <param name="fromValue"> value considered as "stick released"</param>
	/// <param name="toValue">  value consideret as "stick at full"</param>
	/// <param name="valueTocheck"> which value to check</param>
	/// <param name="historyToCheckAgainst">against what list to check</param>
	/// <param name="XEventToFire">event to fire when a spike in X axis is found </param>
	/// <param name="YEventToFire">event to fire when a spike in Y axis is found </param>
	public void CheckForFastStickReleases(float timetoCheck, float fromValue, float toValue, Vector2 valueTocheck,  List<InputTuple> historyToCheckAgainst, System.Action XEventToFire, System.Action YEventToFire)
	{

		// X VALUE
		
		// if the X value is low enough for checking
		if (valueTocheck.x <= toValue)
		{
			float inspectedTime = 0f;

			// loop backwards throuh input history up untill the given time
			for (int i = historyToCheckAgainst.Count - 1; i >= 0; i--)
			{
				inspectedTime += historyToCheckAgainst[i].deltaTime;

				// if desired time reached, stop checking
				if (inspectedTime > timetoCheck)
				{
					break;
				}

				// else fire event if "spike" was found
				if (historyToCheckAgainst[i].value.x >= fromValue)
				{
					XEventToFire?.Invoke();
				}
			}
		}

		//Y VALUE 
		// if the Y value is low enough for checking
		if (valueTocheck.y <= toValue)
		{
			float inspectedTime = 0f;

			// loop backwards throuh input history up untill the given time
			for (int i = historyToCheckAgainst.Count - 1; i >= 0; i--)
			{
				inspectedTime += historyToCheckAgainst[i].deltaTime;

				// if desired time reached, stop checking
				if (inspectedTime > timetoCheck)
				{
					break;
				}

				// else fire event if "spike" was found
				if (historyToCheckAgainst[i].value.y >= fromValue)
				{
					YEventToFire?.Invoke();
				}
			}
		}
	}

	public struct InputTuple
	{
		public Vector2 value;
		public float deltaTime;

		public InputTuple(Vector2 value, float deltaTime)
		{
			this.value = value;
			this.deltaTime = deltaTime;
		}
	}	
}

