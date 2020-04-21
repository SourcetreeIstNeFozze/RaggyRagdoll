using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerB : MonoBehaviour
{
	public Animator handAnimator;
	public HingeJoint handTopJoint;
	public float rotationSpeed;
	public float maxRotation = 50;

	// Update is called once per frame
	void Update()
	{

		// INDEX FINGER

		if (Input.GetKeyDown(KeyCode.P))
		{
			handAnimator.Play("indexStraightUP", 1);
		}
		if (Input.GetKeyUp(KeyCode.P))
		{
			handAnimator.Play("indexStraightDOWN", 1);
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			handAnimator.Play("indexCurvedUP", 1);
		}
		if (Input.GetKeyUp(KeyCode.L))
		{
			handAnimator.Play("indexCurvedDOWN", 1);
		}


		// MIDDLE FINGER
		if (Input.GetKeyDown(KeyCode.O))
		{
			handAnimator.Play("middleStraightUP", 2);
		}
		if (Input.GetKeyUp(KeyCode.O))
		{
			handAnimator.Play("middleStraightDOWN", 2);
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			handAnimator.Play("middleCurvedUP", 2);
		}
		if (Input.GetKeyUp(KeyCode.K))
		{
			handAnimator.Play("middleCurvedDOWN", 2);
		}

		// angle

		JointSpring spring = handTopJoint.spring;
		float valueToAdd = 0;
		if (Input.GetKey(KeyCode.B))
		{
			valueToAdd -= rotationSpeed;

		}
		else if (Input.GetKey(KeyCode.M))
		{
			valueToAdd += rotationSpeed;

		}

		spring.targetPosition = Mathf.Clamp(spring.targetPosition + valueToAdd, -maxRotation, maxRotation);
		handTopJoint.spring = spring;
	}
}
