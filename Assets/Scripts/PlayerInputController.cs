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


	[Space]
	[Header("References")]
	public Animator handAnimator;
	public HingeJoint torsoJoint;
	public GameObject playerRoot;
	[SerializeField] GroundDetector leftFoot;
	[SerializeField] GroundDetector rightFoot;
	public List<AmplifyCollision> playerColliders;
	public Orientation orientation;
	public CJBalancingwithFalling balance;
	public PlayerInputController otherPlayer;

	[Space]
	[Header("Input")]
	public float rotationSpeed;
	public float maxRotation = 50;

	[Space]
	private Vector2 _leftStickInput = new Vector2();
	private Vector2 _rightStickInput = new Vector2();
	private bool _rightBumperHeld;
	private bool _leftBumperHeld;

	[Space]
	[Header("Physics Correction")]
	private Rigidbody playerRigidbody;
	private ConstantForce playerConstanctForce;
	private Vector3 playerVelocity;
	public float maxForceInFlight = 10;
	public float jumpForce = 10;

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

	[Header("Interaction with other player")]

	public System.Action onSideStep;
	public System.Action oncontactWithOtherPlayer;
	

	public enum GroundedState
	{
		inAir,
		bothFeetonTheFloor,
		rightFootOnTheFloor,
		leftFootOnThefloor,
		transitioning
	}

	private void Awake()
	{
		InitializeColliders();
	}

	// Start is called before the first frame update
	void Start()
	{
		playerRigidbody = playerRoot.GetComponent<Rigidbody>();
		playerConstanctForce = playerRoot.GetComponent<ConstantForce>();

		balance = playerRoot.GetComponent<CJBalancingwithFalling>();
		balance.enabled = handCanFall;

		if (amplifyJump)
		{
			OnReleased_RY += () =>
			{  if (Released_RY_timer >= stickReleaseTimeWindow && GetGroundedState() !=  GroundedState.inAir)
				{
					Debug.Log("RY released");
					Released_RY_timer = 0f;
					Jump(playerRoot.transform.up, jumpForce);
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
					Jump(playerRoot.transform.up, jumpForce);
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
		historicRightStick.Add(new InputTuple(_rightStickInput, Time.deltaTime));
		historicLeftStick.Add(new InputTuple(_leftStickInput, Time.deltaTime));

		// fire Stick release Events
		Released_RX_timer += Time.deltaTime;
		Released_RY_timer += Time.deltaTime;
		Released_LX_timer += Time.deltaTime;
		Released_LY_timer += Time.deltaTime;

		CheckForFastStickReleases(stickReleaseTimeWindow, 1f, 0f, _rightStickInput, historicRightStick, OnReleased_RX, OnReleased_RY);
		CheckForFastStickReleases(stickReleaseTimeWindow, 1f, 0f, _leftStickInput, historicLeftStick, OnReleased_LX, OnReleased_LY);

		if (useFK)
		{
			//MANAGE POSES 
			handAnimator.SetFloat("XInput_L", _leftStickInput.x);
			handAnimator.SetFloat("YInput_L", _leftStickInput.y);
			handAnimator.SetFloat("XInput_R", _rightStickInput.x);
			handAnimator.SetFloat("YInput_R", _rightStickInput.y);

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
			if (_leftStickInput.x != 0f && !(GetGroundedState() == GroundedState.inAir))
			{
				BendVertically(_leftStickInput.x * rotationSpeed * (invertControlls ? -1 : 1));
			}
			// When in air push the player
			else if (_leftStickInput.x != 0f && GetGroundedState() == GroundedState.inAir)
			{
				SetPlayerPushForce(new Vector3(_leftStickInput.x, 0, 0), maxForceInFlight);

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
		}

		_rightBumperHeld = true;
	}

	public void OnIndexFingerCurledOUT()
	{
		if (!useFK)
		{
			handAnimator.Play("IndexOUT", 2);
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
		}

		_leftBumperHeld = true;
	}

	public void OnMiddleFingerCurledOUT()
	{
		if (!useFK)
		{
			handAnimator.Play("MiddleOUT", 4);
		}

		_leftBumperHeld = false;
	}

	public void OnLeftStick(InputValue value)
	{
		_leftStickInput = value.Get<Vector2>();
		if (invertControlls)
		{
			_leftStickInput = _leftStickInput * new Vector2(-1f, 1f);
		}

	}

	public void OnRightStick(InputValue value)
	{
		_rightStickInput = value.Get<Vector2>();
		if (invertControlls)
		{
			_rightStickInput = _rightStickInput * new Vector2(-1f, 1f);
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

	private void InitializeColliders()
	{
		for (int i = 0; i < playerColliders.Count; i++)
		{
			playerColliders[i].thisPlayer = this;
			playerColliders[i].otherPlayer = otherPlayer;

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

