using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

	[Header("Input")]
	public bool invertControlls;
	public Animator handAnimator;
	public HingeJoint hipJoint;
	public float rotationSpeed;
	public float maxRotation = 50;
	public Vector2 leftStickInput = new Vector2();

	[Header("Physics Correction")]
	public GameObject PlayerRoot;
	private Rigidbody playerRigidbody;
	private ConstantForce playerConstanctForce;

	public float leftKneeCurlTimer = 10;
	public float rightKneeCurlTimer = 10;
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
		onLanded += () => {
			Debug.Log("Jump");
			Jump(Vector3.up, jumpForce);
		};
	
    }

    // Update is called once per frame
    void Update()
    {
		// 1.MANAGE JUMPS
		rightKneeCurlTimer += Time.deltaTime;
		leftKneeCurlTimer += Time.deltaTime;

		if (GetGroundedState() == GroundedState.bothFeetonTheFloor && !onLandedTriggered && leftKneeCurlTimer <= jumpReactionTime && rightKneeCurlTimer <= jumpReactionTime)
		{
			onLandedTriggered = true;
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

	public void BendHorizontally(float bendValue)
	{
		//JointSpring spring = wristJoint.spring;
		//spring.targetPosition = Mathf.Clamp(spring.targetPosition + bendValue, -maxRotation, maxRotation);
		//handTopJoint.spring = spring;
	}

	public void BendVertically(float bendValue)
	{
		Debug.Log("bending Vertically");
		JointSpring spring = hipJoint.spring;
		spring.targetPosition = Mathf.Clamp(spring.targetPosition + bendValue, -maxRotation, maxRotation);
		hipJoint.spring = spring;
	}

	public void SetPlayerPushForce( Vector3 direction, float constancforceValue)
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
		handAnimator.Play("IndexUP", 1);
	}

	public void OnIndexFingerDOWN()
	{
		handAnimator.Play("IndexDOWN", 1);
	}

	public void OnIndexFingerCurledIN()
	{
		handAnimator.Play("IndexIN", 2);
	}

	public void OnIndexFingerCurledOUT()
	{
		handAnimator.Play("IndexOUT", 2);
		leftKneeCurlTimer = 0;
	}

	public void OnMiddleFingerUP()
	{
		handAnimator.Play("MiddleUP", 3);
	}

	public void OnMiddleFingerDOWN()
	{
		handAnimator.Play("MiddleDOWN", 3);
		
	}

	public void OnMiddleFingerCurledIN()
	{
		handAnimator.Play("MiddleIN", 4);
	}

	public void OnMiddleFingerCurledOUT()
	{
		handAnimator.Play("MiddleOUT", 4);
		rightKneeCurlTimer = 0;
	}

	public void OnLeftStick(InputValue value)
	{
		leftStickInput = value.Get<Vector2>();
	}

	//--- UTILITY FINCTIONS --- //


}

