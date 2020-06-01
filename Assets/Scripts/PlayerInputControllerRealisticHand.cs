using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DitzelGames.FastIK;
using UnityEditor;

public class PlayerInputControllerRealisticHand : MonoBehaviour
{
    [Header("Input method")]
    public bool IK_controls = false;
    public FastIKFabric IK_solver_index;
    public FastIKFabric IK_solver_middle;
    [Space]
    public bool blendTreePoses = false;
    public Animator IK_animator;
    [Space]
    public bool conAnchorForce = false;
    public float moveArmForce_strength = 10f;
    public Vector2 conAnchor_pos = Vector2.up;
    public ConfigurableJoint wristConfJoint;
    [Space]
    public bool hingeForce = false;
    public float hingeTargetPos_min = 0f;
    public float hingeTargetPos_max = -45f;
    public HingeJoint wristHingeJoint;
    private JointSpring hingeSpring;

    [Header("Input")]
	public Animator handAnimator;
	public HingeJoint handTopJoint;
	public HingeJoint wristJoint;
	public float rotationSpeed;
	public float maxRotation = 50;
	private Vector2 leftStickInput = new Vector2();
    private Vector2 rightStickInput = new Vector2();
    private Vector2 leftStickInput_mirrored = new Vector2();
    private Vector2 rightStickInput_mirrored = new Vector2();

    private Vector2 mirrorX;


	// Start is called before the first frame update
	void Start()
	{
        mirrorX = new Vector2(-1f, 1f);

        if (!IK_controls)
        {
            IK_solver_index.enabled = false;
            IK_solver_middle.enabled = false;
        }
        else
        {
            IK_solver_index.enabled = true;
            IK_solver_middle.enabled = true;
        }

        // POSES - activate
        if (blendTreePoses)
        {
            if (IK_controls)
            {
                IK_animator.Play("Blend Tree_Poses", 0);
                IK_animator.Play("Blend Tree_Poses", 1);
            }
            else
            {
                handAnimator.Play("Blend Tree_Poses", 8);
                handAnimator.Play("Blend Tree_Poses", 9);
            }
        }
        else
        // Set default position
        {
            handAnimator.Play("index default", 4);
            handAnimator.Play("middle default", 6);
        }
    }

	// Update is called once per frame
	void Update()
	{
        // MOVE-FORCES

        // Move Connected Anchor
        if (conAnchorForce && wristConfJoint != null)
        {
            Vector3 newPos = new Vector3(conAnchor_pos.x, conAnchor_pos.y, wristConfJoint.transform.position.z + (leftStickInput.x + rightStickInput.x) * moveArmForce_strength);
            wristConfJoint.connectedAnchor = newPos;
        }

        // HingeJoint-Biegung
        if (hingeForce && wristHingeJoint != null)
        {
            float newTargetPos = (leftStickInput.x + rightStickInput.x).Remap(-2f, 2f, hingeTargetPos_max, hingeTargetPos_min);
            hingeSpring = wristHingeJoint.spring;
            hingeSpring.targetPosition = newTargetPos;
            wristHingeJoint.spring = hingeSpring;
        }

        // poses
        // set animator attributes
        if (blendTreePoses)
        {
            if (IK_controls)
            {
                IK_animator.SetFloat("XInput_L", leftStickInput_mirrored.x);
                IK_animator.SetFloat("YInput_L", leftStickInput_mirrored.y);
                IK_animator.SetFloat("XInput_R", rightStickInput_mirrored.x);
                IK_animator.SetFloat("YInput_R", rightStickInput_mirrored.y);
            }
            else
            {
                handAnimator.SetFloat("XInput_L", leftStickInput_mirrored.x);
                handAnimator.SetFloat("YInput_L", leftStickInput_mirrored.y);
                handAnimator.SetFloat("XInput_R", rightStickInput_mirrored.x);
                handAnimator.SetFloat("YInput_R", rightStickInput_mirrored.y);
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

		JointSpring spring = handTopJoint.spring;
		spring.targetPosition = Mathf.Clamp(spring.targetPosition + bendValue, -maxRotation, maxRotation);
		handTopJoint.spring = spring;

	}



	// --- ACTION FUNCTIONS ---//
	public void OnIndexFingerUP()
	{
        if (!IK_controls && !blendTreePoses)
        {
            handAnimator.Play("Index UP", 4);
        }
	}

	public void OnIndexFingerDOWN()
	{
        if (!IK_controls && !blendTreePoses)
        {
            handAnimator.Play("Index DOWN", 4);
        }
	}

	public void OnIndexFingerCurledIN()
	{
        if (!IK_controls && !blendTreePoses)
        {
            handAnimator.Play("Index IN", 5);
        }
	}

	public void OnIndexFingerCurledOUT()
	{
        if (!IK_controls && !blendTreePoses)
        {
            handAnimator.Play("Index OUT", 5);
        }
	}

	public void OnMiddleFingerUP()
	{
        if (!IK_controls && !blendTreePoses)
        {
            handAnimator.Play("Middle UP", 6);
        }
	}

	public void OnMiddleFingerDOWN()
	{
        if (!IK_controls && !blendTreePoses)
        {
            handAnimator.Play("Middle DOWN", 6);
        }
	}

	public void OnMiddleFingerCurledIN()
	{
        if (!IK_controls && !blendTreePoses)
        {
            handAnimator.Play("Middle IN", 7);
        }
	}

	public void OnMiddleFingerCurledOUT()
	{
        if (!IK_controls && !blendTreePoses)
        {
            handAnimator.Play("Middle OUT", 7);
        }
	}



	public void OnLeftStick(InputValue value)
	{
        leftStickInput = value.Get<Vector2>();
        leftStickInput_mirrored = leftStickInput;

        // Wenn linker Spieler, spiegle Input
        if (this.tag.Equals("player_left"))
            leftStickInput_mirrored *= mirrorX;
    }

    public void OnRightStick(InputValue value)
    {
        rightStickInput = value.Get<Vector2>();
        rightStickInput_mirrored = rightStickInput;

        // Wenn linker Spieler, spiegle Input
        if (this.tag.Equals("player_left"))
            rightStickInput_mirrored *= mirrorX;
    }
}
