using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DitzelGames.FastIK;

public class PlayerInputControllerRealisticHand : MonoBehaviour
{
    [Header("Input method")]
    public bool IK_controls = false;
    public FastIKFabric IK_solver_index;
    public FastIKFabric IK_solver_middle;
    public bool conAnchorForce = false;
    public ConfigurableJoint wristConfJoint;
    public bool hingeForce = false;
    public HingeJoint wristHingeJoint;
    [Header("Input")]
	public Animator handAnimator;
	public HingeJoint handTopJoint;
	public HingeJoint wristJoint;
	public float rotationSpeed;
	public float maxRotation = 50;
	private Vector2 leftSrickInput = new Vector2();
    public ConstantForce moveArmForce;
    public float moveArmForce_strength = 10f;


	// Start is called before the first frame update
	void Start()
	{

    }

	// Update is called once per frame
	void Update()
	{
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




        if (wristConfJoint != null)
        {
            wristConfJoint.connectedAnchor += Vector3.forward * leftSrickInput.x * moveArmForce_strength;
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
        if (!IK_controls)
        {
            Debug.Log("on INDEX UP called");
            handAnimator.Play("Index UP", 4);
        }
	}

	public void OnIndexFingerDOWN()
	{
        if (!IK_controls)
        {
            Debug.Log("on INDEX DOWN called");
            handAnimator.Play("Index DOWN", 4);
        }
	}

	public void OnIndexFingerCurledIN()
	{
        if (!IK_controls)
        {
            handAnimator.Play("Index IN", 5);
        }
	}

	public void OnIndexFingerCurledOUT()
	{
        if (!IK_controls)
        {
            handAnimator.Play("Index OUT", 5);
        }
	}

	public void OnMiddleFingerUP()
	{
        if (!IK_controls)
        {
            handAnimator.Play("Middle UP", 6);
        }
	}

	public void OnMiddleFingerDOWN()
	{
        if (!IK_controls)
        {
            handAnimator.Play("Middle DOWN", 6);
        }
	}

	public void OnMiddleFingerCurledIN()
	{
        if (!IK_controls)
        {
            handAnimator.Play("Middle IN", 7);
        }
	}

	public void OnMiddleFingerCurledOUT()
	{
        if (!IK_controls)
        {
            handAnimator.Play("Middle OUT", 7);
        }
	}



	public void OnBodyBending(InputValue value)
	{
        if (conAnchorForce)
        {
            leftSrickInput = value.Get<Vector2>();
        }
        else if (hingeForce)
        {

        }
    }
}
