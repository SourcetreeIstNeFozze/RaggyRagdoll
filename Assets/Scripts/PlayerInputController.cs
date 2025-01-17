﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputController : MonoBehaviour
{
    public bool initializeOnStart = true;
    private bool initialized = false;

    [HideInInspector] public PlayerInstance thisPlayer;
    [HideInInspector] public PlayerInstance otherPlayer;

    [HideInInspector] public HandReferences activeAvatar;
    private bool invertControls;

    [Header("References")]
    public Animator handAnimator;
    ConfigurableJoint configJoint;

    [Space]
    private bool _rightBumperHeld;
    private bool _leftBumperHeld;

    [Header("Interaction with other player")]
    public float timeSinceLastContact;
    public System.Action onSideStep;
    public System.Action oncontactWithOtherPlayer;

    Vector3 invertX = new Vector3(-1f, 1f);
    
    Settings settings { get { return Settings.instance; } }

    [Header("Resetting")]
    [HideInInspector] public List<Transform> childTransofrms = new List<Transform>();
    private List<Quaternion> originalRotations = new List<Quaternion>();
    private List<Vector3> originalPositions = new List<Vector3>();

    // Achor stabilization
    private Vector3 backFingerTip, frontFingerTip, relevantFingerTip, indexTipPos, middleTipPos;
    float inputDirection;
    Vector3 playerMidPoint;
    float jointDrive_startXValue;
    float jointAngDrive_startXValue;
    float jointAngDrive_startYZValue;
    float jointDrive_startMaxForce;
    enum AnchorState { connected, broken};
    AnchorState anchorState = AnchorState.connected;
    Vector3 lookDirection, newAchorPosition;

    public enum GroundedState
    {
        inAir,
        bothFeetOnTheFloor,
        rightFootOnTheFloor,
        leftFootOnThefloor,
        transitioning
    }

    // Start is called before the first frame update
    void Start()
	{
        if (initializeOnStart)
            Initialize();
      }

	public void Initialize()
	{
        // invert Controls?
        if (this.tag.Equals("player_right"))
            invertControls = true;

        else if (this.tag.Equals("player_left"))
            invertControls = false;

        // get initial state the hand
        foreach (Transform child in transform)
		{
			childTransofrms.Add(child);
			originalRotations.Add(child.rotation);
			originalPositions.Add(child.position);
		}

        //prep Avatar
		activeAvatar = GetComponentInChildren<HandReferences>();
        configJoint = activeAvatar.GetComponent<ConfigurableJoint>();

		activeAvatar.playerRoot.SetActive(true);

		//SET-UP
		activeAvatar.middleFinger.stickInput = new StickInput();
		activeAvatar.indexFinger.stickInput = new StickInput();
		activeAvatar.middleFinger.stickInput_original = new StickInput();
		activeAvatar.indexFinger.stickInput_original = new StickInput();

		AssignPlayersToHandlers();


		// WIRE EVENTS
		if (settings.amplifyJump)
		{
			activeAvatar.middleFinger.stickInput.OnReleased_Y += () =>
			{
				if (GetGroundedState() != GroundedState.inAir)
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
				if (GetGroundedState() != GroundedState.inAir)
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
		jointDrive_startXValue = configJoint.xDrive.positionSpring;
        jointAngDrive_startXValue = configJoint.angularXDrive.positionSpring;
        jointAngDrive_startYZValue = configJoint.angularYZDrive.positionSpring;

        initialized = true;
        //if (settings.fallMode = Settings.FallMode.angleAndCOM)
	}
     

    // Update is called once per frame
    void Update()
    {
		if (initialized)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				ResetPosition();
			}

			// timers
			timeSinceLastContact += Time.deltaTime;

			//// gather data

			activeAvatar.middleFinger.stickInput.Update();
			activeAvatar.indexFinger.stickInput.Update();

			activeAvatar.middleFinger.stickInput.CheckForFastStickReleases(settings.stickReleaseTimeWindow, 1f, 0f, settings.stickReleaseTimeWindow);
			activeAvatar.indexFinger.stickInput.CheckForFastStickReleases(settings.stickReleaseTimeWindow, 1f, 0f, settings.stickReleaseTimeWindow);

			// IF USING POSES
			if (settings.useFK)
			{
				//MANAGE POSES 
				handAnimator.SetFloat("XInput_L", activeAvatar.middleFinger.stickInput.value.x);
				handAnimator.SetFloat("YInput_L", activeAvatar.middleFinger.stickInput.value.y);
				handAnimator.SetFloat("XInput_R", activeAvatar.indexFinger.stickInput.value.x);
				handAnimator.SetFloat("YInput_R", activeAvatar.indexFinger.stickInput.value.y);


				////MANAGE BENDING AND PUSHING
				ManageManualBending();


				// NEW ANCHOR-STABILISATION -> full of flaws
				//if (settings.fallMode == Settings.FallMode.spring_backFoot)
				//{
				//    AnchorStabilization();
				//    AnchorBreakForce();
				//}

				if (settings.fallMode == Settings.FallMode.spring_feet)
				{
					GetFingerTipData();
					Anchor_AutomaticFeetBalance();
					AnchorInputAmplification();
					AnchorBreakForce();
				}

				if (settings.fallMode == Settings.FallMode.autoBend)
				{
					// tbd
				}

                else if (settings.fallMode == Settings.FallMode.COM)
                {
                    if (activeAvatar.softened == false)
                    {
                        GetFingerTipData();

                        if (settings.pushAmplification == Settings.PushAmplificationMode.anchor)
                        {
                            COM_inputAmplification_angle();
                            COM_inputAmplification_anchor();
                        }
                    }

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
					SetPlayerPushForce(new Vector3(activeAvatar.indexFinger.stickInput.value.x, 0, 0), settings.maxHipPushForce * (invertControls ? -1 : 1));
				}
				else
				{
					SetPlayerPushForce(Vector3.zero, 0);
				}
			}

			// GLOBAL & LOCAL SPACE
			// -> rotate the input-Vector2 in order to fake global space
			if (settings.poseSpace == Settings.TransformType.global)
			{
				FakeGlobalSpace();
			}
		}
	}

	void FakeGlobalSpace()
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

    void ManageManualBending()
    {

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
            BendVertically(bendDirection * settings.hipRotationSpeed * (invertControls ? 1 : -1));
        }
        // When in air push the player
        else if ((GetGroundedState() == GroundedState.inAir) || !(GetGroundedState() == GroundedState.inAir && settings.compassBending))
        {
            SetPlayerPushForce(new Vector3(bendDirection, 0, 0), settings.maxHipPushForce * (invertControls ? 1 : -1));
        }
        else
        {
            SetPlayerPushForce(Vector3.zero, 0);
        }
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

    public void OnJump()
    {
        Jump(Vector3.up, settings.jumpForce);
    }

    private void AssignPlayersToHandlers()
    {
        // convert to List
        for (int i = 0; i < activeAvatar.childHandlers.Length; i++)
        {
            activeAvatar.childHandlers[i].thisPlayer = this;
            activeAvatar.childHandlers[i].otherPlayer = otherPlayer.inputController;
        }
    }

    public void ResetPosition()
    {
        activeAvatar.HardenFingers();
        HardenTorso();

        //reset positions
        for (int i = 0; i < childTransofrms.Count; i++)
        {
            childTransofrms[i].position = originalPositions[i];
            childTransofrms[i].rotation = originalRotations[i];
        }

        // nullify velocity
        for (int i = 0; i < activeAvatar.childHandlers.Length; i++)
        {
            Rigidbody childRigidbody = activeAvatar.childHandlers[i].rigid;
            if (childRigidbody != null)
            {
                childRigidbody.velocity = Vector3.zero;
            }
        }

        // reset configurable joint
        if (settings.fallMode == Settings.FallMode.spring_backFoot || settings.fallMode == Settings.FallMode.spring_feet)
        {
            SetXYZDriveOfConfigjoint(jointDrive_startXValue);
            anchorState = AnchorState.connected;
        }

        // reset positions and drives of child joints
    }


    // FULL OF FLAWS
    //void AnchorStabilization()
    //{
    //    // -- Suche Finger, der am weitesten hinten ist in Relation zur Bewegungsrichtung --

    //    // 1. GET INPUT DIRECTION
    //    GetFingerTipData();
    //    inputDirection = Mathf.Clamp01(activeAvatar.indexFinger.stickInput.value.x + activeAvatar.middleFinger.stickInput.value.x);


    //    // 2. SETZE FINGER, DER AM WEITESTEN HINTEN IST IN RELATION ZUR BEWEGUNGS-RICHTUNG
    //    //Vector3 playerMidPoint = (settings.LEFT.transform.position - settings.RIGHT.transform.position) / 2f + settings.RIGHT.transform.position;
    //    if (inputDirection > 0)
    //    {
    //        // Spieler bewegt sich nach VORNE, ermittle Finger der am weitesten HINTEN ist
    //        if ((indexTipPos - playerMidPoint).magnitude > (middleTipPos - playerMidPoint).magnitude)
    //            relevantFingerTip = indexTipPos;
    //        else
    //            relevantFingerTip = middleTipPos;
    //        // checke, dass relevantFinger nicht VOR knuckles ist
    //        if ((relevantFingerTip - playerMidPoint).magnitude < (activeAvatar.transform.position - playerMidPoint).magnitude)
    //        {
    //            relevantFingerTip = activeAvatar.transform.position;
    //            print("sonder");
    //        }
    //    }
    //    else if (inputDirection < 0)
    //    {
    //        // Spieler bewegt sich nach HINTEN, ermittle Finger der am weitesten VORNE ist
    //        if ((indexTipPos - playerMidPoint).magnitude < (middleTipPos - playerMidPoint).magnitude)
    //            relevantFingerTip = indexTipPos;
    //        else
    //            relevantFingerTip = middleTipPos;

    //        // checke, dass relevantFinger nicht HINTER knuckles ist
    //        if ((relevantFingerTip - playerMidPoint).magnitude > (activeAvatar.transform.position - playerMidPoint).magnitude)
    //        {
    //            relevantFingerTip = activeAvatar.transform.position;
    //            print("sonder");
    //        }
    //    }
    //    else
    //        // kein Input
    //        relevantFingerTip = (indexTipPos + middleTipPos) / 2f;

    //    // 3. Vector holen

    //    Vector3 lookDirection = (otherPlayer.activeAvatar.transform.position - activeAvatar.transform.position);
    //    lookDirection = new Vector3(lookDirection.x, 0, lookDirection.z);
    //    Plane plane = new Plane(lookDirection, activeAvatar.transform.position);
    //    Vector3 direction = plane.ClosestPointOnPlane(relevantFingerTip) - relevantFingerTip;
    //    Vector3 newAchorPosition = activeAvatar.transform.InverseTransformPoint(activeAvatar.transform.position + direction);
    //    Debug.DrawLine(plane.ClosestPointOnPlane(relevantFingerTip), relevantFingerTip, Color.blue);
    //    Debug.DrawLine(activeAvatar.transform.position, activeAvatar.transform.position + new Vector3(0, -3, 0), Color.magenta);
    //    Debug.DrawLine(relevantFingerTip, Vector3.zero, Color.black);


    //    // 4. CONNECTED ANCHOR SETZEN
    //    configJoint.connectedAnchor = new Vector3(
    //        newAchorPosition.x,
    //        settings.configJoint_Y_Offset, // nur y bleibt
    //        newAchorPosition.z) -
    //        activeAvatar.transform.InverseTransformVector(lookDirection.normalized * inputDirection * settings.anchorInputStrength); // * strength;

    //    Debug.DrawLine(activeAvatar.transform.TransformPoint(configJoint.connectedAnchor), Vector3.zero, Color.gray);
    //    Debug.DrawLine(activeAvatar.transform.InverseTransformVector(lookDirection.normalized * inputDirection), Vector3.zero, Color.red);
    //}

    void AnchorBreakForce()
    {
        // V1: use distance
        //float anchorDistance = (activeAvatar.transform.TransformPoint(configJoint.connectedAnchor) - activeAvatar.transform.position).magnitude;
        //if (anchorDistance > settings.anchorBreakDistanceLimit && anchorState == AnchorState.connected)
        //{
        //    anchorState = AnchorState.broken;
        //    SetXYZDrive(0);
        //    print("BREAK " + anchorDistance);
        //}

        // V2: use angle
        float handAngle = activeAvatar.transform.eulerAngles.x;
        if (handAngle > 180f)
            handAngle -= 360f;
        else if (handAngle < -180f)
            handAngle += 360f;

        // Break Anchor
        if (settings.breakAnchorAtLimit)
        {
            if (Mathf.Abs(handAngle) > settings.anchorBreakAngleLimit)
            {
                anchorState = AnchorState.broken;
                configJoint.connectedAnchor = Vector3.zero;// - Vector3.up * settings.configJoint_Y_Offset;
                //SetXYZDrive(0);
                print("BREAK " + handAngle);
            }
        }

     }

    public void GetFingerTipData() // KAROLINA: I dont know where this function belongs, so I'll let it here till I have another refactoring fever. Probably Christmass 2021
    {
        indexTipPos = activeAvatar.indexFinger.fingerTip.transform.position;
        middleTipPos = activeAvatar.middleFinger.fingerTip.transform.position;
        playerMidPoint = (otherPlayer.activeAvatar.transform.position - activeAvatar.transform.position) / 2f + activeAvatar.transform.position;
        lookDirection = (otherPlayer.activeAvatar.transform.position - activeAvatar.transform.position);
        lookDirection = new Vector3(lookDirection.x, 0, lookDirection.z);

        inputDirection = Mathf.Clamp(activeAvatar.indexFinger.stickInput.value.x + activeAvatar.middleFinger.stickInput.value.x, -2f, 2f);
    }

    private void Anchor_AutomaticFeetBalance()
    {
        GroundedState groundedState = GetGroundedState();
        float conAnchorYPos = activeAvatar.transform.position.y + settings.configJoint_Y_Offset;

        switch (groundedState)
        {
            case GroundedState.inAir:
                newAchorPosition = activeAvatar.transform.position;
                break;

            case GroundedState.bothFeetOnTheFloor:
                Vector3 fingerMiddlePos = (indexTipPos + middleTipPos) / 2f;
                newAchorPosition = new Vector3(fingerMiddlePos.x, conAnchorYPos, fingerMiddlePos.z);
                break;

            case GroundedState.leftFootOnThefloor:
                newAchorPosition = HandPlusFingertipDirection(indexTipPos);
                newAchorPosition = new Vector3(newAchorPosition.x, conAnchorYPos, newAchorPosition.z);
                break;

            case GroundedState.rightFootOnTheFloor:
                newAchorPosition = HandPlusFingertipDirection(middleTipPos);
                newAchorPosition = new Vector3(newAchorPosition.x, conAnchorYPos, newAchorPosition.z);
                break;
        }

        // set & convert to local space
        configJoint.connectedAnchor = configJoint.connectedBody.transform.InverseTransformPoint(newAchorPosition);

        Debug.DrawLine(activeAvatar.transform.TransformPoint(configJoint.connectedAnchor), Vector3.zero, Color.blue);
    }

    private void AnchorInputAmplification()
    {
        inputDirection = Mathf.Clamp(activeAvatar.indexFinger.stickInput.value.x + activeAvatar.middleFinger.stickInput.value.x, -1f, 1f);
        // calc
        newAchorPosition = newAchorPosition + lookDirection.normalized * inputDirection * settings.anchorInputStrength;
        // convert & set
        configJoint.connectedAnchor = configJoint.connectedBody.transform.InverseTransformPoint(newAchorPosition);
    }

    private void COM_inputAmplification_anchor()
    {
        // I could write the existing code better instead of just duplicating existing functions but FUCK THIS SCRIPT

        // set anchor up
        newAchorPosition = activeAvatar.transform.position + Vector3.up * settings.anchorYOffset;

        // input force; only backwarts
        newAchorPosition += lookDirection.normalized * Mathf.Clamp(inputDirection, -2f, settings.anchorForwardForce) * settings.anchorInputStrength2;

        // convert & set
        configJoint.connectedAnchor = configJoint.connectedBody.transform.InverseTransformPoint(newAchorPosition);
    }


    private Vector3 HandPlusFingertipDirection(Vector3 fingerTipPos)
    {
        // too weird zu explain; just a bit math 
        // (berechne die x-distance von fingerTip zu Avatar, funktionierend der ständigen bewegung auf einer anderen achse)
        Plane plane = new Plane(lookDirection, activeAvatar.transform.position);
        Vector3 fingerTipToHand_xDistance = fingerTipPos - plane.ClosestPointOnPlane(fingerTipPos);
        Vector3 newAchorPosition = activeAvatar.transform.position + fingerTipToHand_xDistance;

        Debug.DrawLine(plane.ClosestPointOnPlane(fingerTipPos), fingerTipPos, Color.blue);
        Debug.DrawLine(activeAvatar.transform.position, activeAvatar.transform.position + new Vector3(0, -3, 0), Color.magenta);
        Debug.DrawLine(fingerTipPos, Vector3.zero, Color.black);

        return newAchorPosition;
    }

    private float FingertipToHandDistance(Vector3 fingerTip, Vector3 handPoint)
    {
        // das gleiche wie HandPlusFingertipDirection(), aber nur distance berechnen
        Plane plane = new Plane(lookDirection, handPoint);
        float distance = (fingerTip - plane.ClosestPointOnPlane(fingerTip)).magnitude;
        Debug.DrawLine(fingerTip, plane.ClosestPointOnPlane(fingerTip), Color.green);

        return distance;
    }

    private void SetXYZDriveOfConfigjoint(float value)
    {

        JointDrive drive = configJoint.xDrive;
        drive.positionSpring = value;
        //drive.maximumForce = jointDrive_startMaxForce;
        configJoint.xDrive = drive;
        configJoint.yDrive = drive;
        configJoint.zDrive = drive;
    }



    private void SetAngularXYZDrive (float value)
    {

        JointDrive drive = configJoint.angularXDrive;
        drive.positionSpring = value;
        //drive.maximumForce = jointDrive_startMaxForce;
        configJoint.angularXDrive = drive;
        configJoint.angularYZDrive = drive;
    }

    private void COM_inputAmplification_angle()
    {
        // bend the hand in the direction both sticks are pressed into (left or right) to support walking
        inputDirection = Mathf.Clamp(activeAvatar.indexFinger.stickInput.value.x + activeAvatar.middleFinger.stickInput.value.x, -2f, settings.angleForwardForce);
        float targetXAngle_euler = inputDirection.Remap(-2f, 2f, -settings.maxAutoBendAngle, settings.maxAutoBendAngle);

        Quaternion targetAngle = Quaternion.Euler(targetXAngle_euler, 0, 0);
        configJoint.targetRotation = targetAngle;

        //if (this.tag == "player_right")
        //    print("targetAngle: " + targetXAngle_euler);
    }

	private void OnDrawGizmos()
	{
        Gizmos.DrawSphere(indexTipPos, 0.1f);
        Gizmos.DrawSphere(middleTipPos, 0.1f);
    }

    public void SofterTorso()
	{
        Debug.Log("Softening Torso");
        JointDrive Xdrive = configJoint.angularXDrive;
        Xdrive.positionSpring = 0;

        JointDrive YZdrive = configJoint.angularYZDrive;
        YZdrive.positionSpring = 0;
    }

    public void HardenTorso()
    {
        Debug.Log("Hardening Torso");
        JointDrive Xdrive = configJoint.angularXDrive;
        Xdrive.positionSpring = jointAngDrive_startXValue;

        JointDrive YZdrive = configJoint.angularYZDrive;
        YZdrive.positionSpring = jointAngDrive_startYZValue;
    }

    public void Push(Vector3 force) 
    {
        
    }
}

