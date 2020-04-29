using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

	[Header("Input")]
	public Animator handAnimator;
	public HingeJoint handTopJoint;
	public HingeJoint wristJoint;
	public float rotationSpeed;
	public float maxRotation = 50;
	private Vector2 leftSrickInput = new Vector2();

	[Header("Hand Stabilization & Fall Down")]
	public bool improvedHandStabilization = false;
	public bool handsCanFallDown = false;
	public Vector3 restartForce = Vector3.up * 50f;
	public Rigidbody handRigid;
	public float handSpring_connectedAnchorHeight = 4.1f;

	// Start is called before the first frame update
	void Start()
    {

	
	
    }

    // Update is called once per frame
    void Update()
    {
		Debug.Log(leftSrickInput);

		// only if the hand doesn't use the new stabilization (cause it wouldn't have the needed components)
		if (!improvedHandStabilization && leftSrickInput != Vector2.zero)
		{
			
			if (leftSrickInput.x !=  0f)
			{
				BendVertically(leftSrickInput.x * -rotationSpeed);
			}
			else if (leftSrickInput.y !=  0f)
			{
				BendHorizontally(leftSrickInput.y * rotationSpeed);
			}

		}
	}

	public void BendHorizontally(float bendValue)
	{
		JointSpring spring = wristJoint.spring;
		spring.targetPosition = Mathf.Clamp(spring.targetPosition + bendValue, -maxRotation, maxRotation);
		handTopJoint.spring = spring;
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
		handAnimator.Play("indexStraightUP", 1);
	}

	public void OnIndexFingerDOWN()
	{
		handAnimator.Play("indexStraightDOWN", 1);
	}

	public void OnIndexFingerIN()
	{
		handAnimator.Play("indexCurvedUP", 1);
	}

	public void OnIndexFingerOUT()
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

	public void OnMiddleFingerIN()
	{
		handAnimator.Play("middleCurvedUP", 2);
	}

	public void OnMiddleFingerOUT()
	{
		handAnimator.Play("middleCurvedDOWN", 2);
	}

	public void OnStandUp()
	{
		// (only if standUp is activated)
		if (handsCanFallDown)
		{
			StartCoroutine(StandUp());
		}
	}

	public void OnBodyBending(InputValue value)
	{
		leftSrickInput = value.Get<Vector2>();

		
	}



	// Stand Up-Coroutine
	IEnumerator StandUp()
	{
		bool standUp = true;
		bool standUp_stabilize = false;

		// 1) handfläche darf höchstens zur hälfte aufgerichtet sein, force nach oben adden
		while (standUp)
		{
			handRigid.AddForce(restartForce);
			//print("phase 1");

			if (handRigid.position.y > (handSpring_connectedAnchorHeight * 0.9f))
			{
				standUp = false;
				standUp_stabilize = true;
			}
			yield return new WaitForFixedUpdate();
		}

		// 2) handfläche ist mind. zur hälfte aufgerichtet, jetzt velocity bremsen
		while (standUp_stabilize)
		{
			handRigid.velocity /= 1.08f;
			//print("phase 2");
			if (handRigid.velocity.magnitude < 0.2f)
			{
				standUp_stabilize = false;
			}
			yield return new WaitForFixedUpdate();
		}

		yield return null;
	}






	//--- UTILITY FINCTIONS --- //


}

