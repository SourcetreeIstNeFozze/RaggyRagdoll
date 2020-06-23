using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputController : MonoBehaviour
{
	[HideInInspector] public HandReferences activeAvatar;
	private bool invertControls;

	[Header("References")]
	public Animator handAnimator;
	public PlayerInputController otherPlayer;

	[Space]
	private bool _rightBumperHeld;
	private bool _leftBumperHeld;

	[Header("Interaction with other player")]
	public float timeSinceLastContact;
	public System.Action onSideStep;
	public System.Action oncontactWithOtherPlayer;


    Vector3 invertX = new Vector3(-1f, 1f);
    ConfigurableJoint configJoint;

	Settings settings { get { return Settings.instance; } }

	[Header("Resetting")]
	[HideInInspector] public List<Transform> childTransofrms = new List<Transform>();
	private List<Quaternion> originalRotations = new List<Quaternion>();
	private List<Vector3> originalPositions = new List<Vector3>();

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
		// get initial state the hand
		foreach (Transform child in transform)
		{
			childTransofrms.Add(child);
			originalRotations.Add(child.rotation);
			originalPositions.Add(child.position);
		}


        if (this.tag.Equals("player_right"))
        {
            activeAvatar = settings.RIGHT;
            invertControls = true;
            configJoint = activeAvatar.GetComponent<ConfigurableJoint>();
        }
        else if (this.tag.Equals("player_left"))
        {
            activeAvatar = settings.LEFT;
            invertControls = false;
            configJoint = activeAvatar.GetComponent<ConfigurableJoint>();
        }
    }

	// Start is called before the first frame update
	void Start()
	{
		

		activeAvatar.playerRoot.SetActive(true);

		//SET-UP
		activeAvatar.middleFinger.stickInput = new StickInput();
		activeAvatar.indexFinger.stickInput = new StickInput();
		activeAvatar.middleFinger.stickInput_original = new StickInput();
		activeAvatar.indexFinger.stickInput_original = new StickInput();


		activeAvatar.balance.lookAtTarget = otherPlayer.activeAvatar.playerRoot;
		AssignPlayersToHandlers();


		// WIRE EVENTS
		if (  settings.amplifyJump)
		{
			activeAvatar.middleFinger.stickInput.OnReleased_Y += () =>
			{  if (GetGroundedState() !=  GroundedState.inAir)
				{
					Debug.Log("RY released");
					Jump(activeAvatar.playerRoot.transform.up, settings.jumpForce);
				}
			};

			activeAvatar.middleFinger.stickInput.OnReleased_X += () =>
			{
				Debug.Log("RX released");
			};

			activeAvatar.indexFinger.stickInput.OnReleased_Y += () =>
			{
				if ( GetGroundedState() != GroundedState.inAir)
				{
					Debug.Log("LY released");
					Jump(activeAvatar.playerRoot.transform.up, settings.jumpForce);
				}
			};
			activeAvatar.indexFinger.stickInput.OnReleased_X += () =>
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

		activeAvatar.middleFinger.stickInput.Update();
		activeAvatar.indexFinger.stickInput.Update();

		activeAvatar.middleFinger.stickInput.CheckForFastStickReleases(settings.stickReleaseTimeWindow, 1f, 0f, settings.stickReleaseTimeWindow);
		activeAvatar.indexFinger.stickInput.CheckForFastStickReleases(settings.stickReleaseTimeWindow, 1f, 0f, settings.stickReleaseTimeWindow);

		if (settings.useFK)
		{
			//MANAGE POSES 
			handAnimator.SetFloat("XInput_R", activeAvatar.middleFinger.stickInput.value.x);
			handAnimator.SetFloat("YInput_R", activeAvatar.middleFinger.stickInput.value.y);
			handAnimator.SetFloat("XInput_L", activeAvatar.indexFinger.stickInput.value.x);
			handAnimator.SetFloat("YInput_L", activeAvatar.indexFinger.stickInput.value.y);


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

			if (invertControls)
			{
				bendDirection *= -1f;
			}

			//2.BENDING
			// Bend the body if getting input and on the floor 
			if (!(GetGroundedState() == GroundedState.inAir) && !settings.compassBending)
			{
				BendVertically(bendDirection * settings.hipRotationSpeed);
			}
			// When in air push the player
			else if ((GetGroundedState() == GroundedState.inAir) || !(GetGroundedState() == GroundedState.inAir && settings.compassBending))
			{
				SetPlayerPushForce(new Vector3(bendDirection, 0, 0), settings.maxHipPushForce);

			}
			else
			{
				SetPlayerPushForce(Vector3.zero, 0);
			}


            // NEW STABILISATION
            if (settings.fallMode == Settings.FallMode.spring)
            {
                configJoint.connectedAnchor = new Vector3(
                    activeAvatar.transform.position.x, 
                    activeAvatar.transform.position.y, 
                    activeAvatar.transform.position.z)
                    + settings.configJoint_Y_Offset * activeAvatar.transform.up;
            }

        }


		else
		{

			// MANAGE BENDING AND MOVEMENT

			// Bend the body if getting input and on the floor 
			if (activeAvatar.indexFinger.stickInput.value.x != 0f && !(GetGroundedState() == GroundedState.inAir))
			{
				BendVertically(activeAvatar.indexFinger.stickInput.value.x * settings.hipRotationSpeed * (invertControls ? -1 : 1));
			}
			// When in air push the player
			else if (activeAvatar.indexFinger.stickInput.value.x != 0f && GetGroundedState() == GroundedState.inAir)
			{
				SetPlayerPushForce(new Vector3(activeAvatar.indexFinger.stickInput.value.x, 0, 0), settings.maxHipPushForce);

			}
			else
			{
				SetPlayerPushForce(Vector3.zero, 0);
			}
		}

        // GLOBAL & LOCAL SPACE
        // -> rotate the input-Vector2 by the rotation of the hand in order to fake global space
        if (settings.poseSpace == Settings.TransformType.global)
        {
            float bodyRotation = activeAvatar.playerRoot.transform.localEulerAngles.x;
            if (bodyRotation > 180)
                bodyRotation -= 360;
            if (bodyRotation < -180)
                bodyRotation += 360;

            Vector2 index_stickInput_global = activeAvatar.indexFinger.stickInput_original.value.Rotate(-bodyRotation);
            Vector2 middle_stickInput_global = activeAvatar.middleFinger.stickInput_original.value.Rotate(-bodyRotation);
			activeAvatar.indexFinger.stickInput.value = index_stickInput_global;
			activeAvatar.middleFinger.stickInput.value = middle_stickInput_global;

            Debug.DrawLine(Vector3.zero, activeAvatar.indexFinger.stickInput_original.value * new Vector2(-2f, 2f), Color.black, 0.3f);
            Debug.DrawLine(Vector3.zero, index_stickInput_global * new Vector2(-2f, 2f), Color.blue, 0.3f);
            //print("bodyRot: " + bodyRotation + ", newRotation: " + Vector2.Angle(Vector2.up, index_stickInput_global) + ", " + index_stickInput_global);
        }
    }

	public void BendVertically(float bendValue)
	{
		JointSpring spring = activeAvatar.torsoJoint.spring;
		spring.targetPosition = Mathf.Clamp(spring.targetPosition + bendValue, -settings.maxHipRotation, settings.maxHipRotation);
		activeAvatar.torsoJoint.spring = spring;
	}

	public void SetPlayerPushForce(Vector3 direction, float constancforceValue)
	{
		direction = direction.normalized;
		activeAvatar.playerConstanctForce.force = direction * constancforceValue;
	}

	public void Jump(Vector3 direction, float forceValue)
	{
		Debug.Log("Jump");
		direction = direction.normalized;
		activeAvatar.playerRigidbody.AddForce(direction * forceValue, ForceMode.Impulse);
	}

	// TO DO add some delays and stuff
	private GroundedState GetGroundedState()
	{
		if (activeAvatar.indexFinger.fingerBottom.touchesGround && activeAvatar.middleFinger.fingerBottom.touchesGround)
		{
			return GroundedState.bothFeetOnTheFloor;
		}
		else if (activeAvatar.indexFinger.fingerBottom.touchesGround)
		{
			return GroundedState.leftFootOnThefloor;
		}
		else if (activeAvatar.middleFinger.fingerBottom.touchesGround)
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
		if (!settings.useFK)
			handAnimator.Play("IndexUP", 2);

        _rightBumperHeld = true;
    }

	public void OnIndexFingerDOWN()
	{
        if (!settings.useFK)
            handAnimator.Play("IndexDOWN", 2);

       _rightBumperHeld = false;
	}

	public void OnIndexFingerCurlIN()
	{
		if (!settings.useFK || settings.useTriggersCurl)
		{
			handAnimator.Play("IndexCURLin", 3);
		}
	}

	public void OnIndexFingerCurlOUT()
	{
		if (!settings.useFK || settings.useTriggersCurl)
		{
			handAnimator.Play("IndexCURLout", 3);
		}
	}

	public void OnMiddleFingerUP()
	{
		if (!settings.useFK)
			handAnimator.Play("MiddleUP", 4);

        _leftBumperHeld = true;
    }

	public void OnMiddleFingerDOWN()
	{
		if (!settings.useFK)
			handAnimator.Play("MiddleDOWN", 4);

        _leftBumperHeld = false;
    }

	public void OnMiddleFingerCurlIN(InputValue value)
	{
		if (!settings.useFK || settings.useTriggersCurl)
		{
			handAnimator.Play("MiddleIN", 5);
            print("middleCurlIn; value: " + value.Get<float>());
		}
	}

	public void OnMiddleFingerCurlOUT()
	{
		if (!settings.useFK || settings.useTriggersCurl)
		{
			handAnimator.Play("MiddleOUT", 5);
		}
	}

	public void OnLeftStick(InputValue value)
	{
		//indexFinger.stickInput.value = value.Get<Vector2>();
		activeAvatar.indexFinger.stickInput_original.value = value.Get<Vector2>(); // -> stickInput.value gets assigned in update; keep using this for calculations

        // mirror if right player
        if (this.tag.Equals("player_right"))
			activeAvatar.indexFinger.stickInput_original.value *= invertX;

        // local space
        if (settings.poseSpace == Settings.TransformType.local)
			activeAvatar.indexFinger.stickInput.value = activeAvatar.indexFinger.stickInput_original.value;
    }

	public void OnRightStick(InputValue value)
	{
		//middleFinger.stickInput.value = value.Get<Vector2>();
		activeAvatar.middleFinger.stickInput_original.value = value.Get<Vector2>(); // -> stickInput.value gets assigned in update; keep using this for calculations

        // mirror if right player
        if (this.tag.Equals("player_right"))
			activeAvatar.middleFinger.stickInput_original.value *= invertX;

        // local space
        if (settings.poseSpace == Settings.TransformType.local)
			activeAvatar.middleFinger.stickInput.value = activeAvatar.middleFinger.stickInput_original.value;
    }

    private void AssignPlayersToHandlers()
	{
		// convert to List
		for (int i = 0; i < activeAvatar.childHandlers.Length; i++)
		{
			activeAvatar.childHandlers[i].thisPlayer = this;
			activeAvatar.childHandlers[i].otherPlayer = otherPlayer;
		}
	}

    public void Reset()
    {
        for (int i = 0; i < childTransofrms.Count; i++)
        {
            childTransofrms[i].position = originalPositions[i];
            childTransofrms[i].rotation = originalRotations[i];
        }

        for (int i = 0; i < activeAvatar.childHandlers.Length; i++)
        {
            Rigidbody childRigidbody = activeAvatar.childHandlers[i].rigid;
            if (childRigidbody != null)
            {
                childRigidbody.velocity = Vector3.zero;
            }
        }
    }
}

