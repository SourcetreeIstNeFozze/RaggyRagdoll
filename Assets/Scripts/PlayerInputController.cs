using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

	[Header("Input")]
	public bool useFK;
	public bool invertControlls;
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

	public bool leftKneeCurled = false;
	public bool rightKneeCurled = false;
	public float leftKneeCurlTimer = 0;
	public float rightKneeCurlTimer = 0;
	public float lastLeftKneeCurl = 0;
	public float lastRightKneeCurl = 0;
	public float jumpReactionTime = 0.5f;

	[SerializeField] GroundDetector leftFoot;
	[SerializeField] GroundDetector rightFoot;


	public float maxForceInFlight = 10;
	public float jumpForce = 10;

	System.Action onLanded;
	public bool onLandedTriggered;
	public bool landed; // this should be done with a property but Im to dumb atm

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

		// subscribe to events
		onLanded += () =>
		{
			Debug.Log("Jump");
			Jump(Vector3.up, jumpForce);
		};

	}

	// Update is called once per frame
	void Update()
	{

		
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

	public void BendHorizontally(float bendValue)
	{
		//JointSpring spring = wristJoint.spring;
		//spring.targetPosition = Mathf.Clamp(spring.targetPosition + bendValue, -maxRotation, maxRotation);
		//handTopJoint.spring = spring;
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
}

