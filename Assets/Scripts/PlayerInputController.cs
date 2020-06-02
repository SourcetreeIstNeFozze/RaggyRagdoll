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

	// Start is called before the first frame update
	void Start()
    {

	
	
    }

    // Update is called once per frame
    void Update()
    {

		//Debug.Log(leftSrickInput);

		// only if the hand doesn't use the new stabilization (cause it wouldn't have the needed components)
		if (leftStickInput != Vector2.zero)
		{
			if (leftStickInput.x !=  0f)
			{
				BendVertically(leftStickInput.x * rotationSpeed * (invertControlls? -1: 1));
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
		Debug.Log("bending Vertically");
		JointSpring spring = hipJoint.spring;
		spring.targetPosition = Mathf.Clamp(spring.targetPosition + bendValue, -maxRotation, maxRotation);
		hipJoint.spring = spring;
	}

	// --- ACTION FUNCTIONS ---//

	public void OnIndexFingerUP()
	{
		handAnimator.Play("indexStraightUP", 1);
	}

	public void OnIndexFingerDOWN()
	{
		handAnimator.Play("indexStraightDOWN", 1);
	}

	public void OnIndexFingerCurledIN()
	{
		handAnimator.Play("indexCurvedUP", 1);
	}

	public void OnIndexFingerCurledOUT()
	{
		handAnimator.Play("indexCurvedDOWN", 1);
	}

	public void OnMiddleFingerUP()
	{
		handAnimator.Play("middleStraightUP", 2);
	}

	public void OnMiddleFingerDOWN()
	{
		handAnimator.Play("middleStraightDOWN", 2);
	}

	public void OnMiddleFingerCurledIN()
	{
		handAnimator.Play("middleCurvedUP", 2);
	}

	public void OnMiddleFingerCurledOUT()
	{
		handAnimator.Play("middleCurvedDOWN", 2);
	}

	public void OnLeftStick(InputValue value)
	{
		leftStickInput = value.Get<Vector2>();
	}




	//--- UTILITY FINCTIONS --- //


}

