using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	public Animator handAnimator;

	// Update is called once per frame
	void Update()
	{

		// INDEX FINGER

		if (Input.GetKeyDown(KeyCode.Q))
		{
			handAnimator.Play("indexStraightUP", 1);
		}
		if (Input.GetKeyUp(KeyCode.Q))
		{
			handAnimator.Play("indexStraightDOWN", 1);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			handAnimator.Play("indexCurvedUP", 1);
		}
		if (Input.GetKeyUp(KeyCode.A))
		{
			handAnimator.Play("indexCurvedDOWN", 1);
		}


		// MIDDLE FINGER
		if (Input.GetKeyDown(KeyCode.W))
		{
			handAnimator.Play("middleStraightUP", 2);
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			handAnimator.Play("middleStraightDOWN", 2);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			handAnimator.Play("middleCurvedUP", 2);
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			handAnimator.Play("middleCurvedDOWN", 2);
		}
	}
}
