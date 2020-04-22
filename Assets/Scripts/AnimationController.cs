using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	public Animator handAnimator;
	public HingeJoint handTopJoint;
	public float rotationSpeed;
	public float maxRotation = 50;

    [Header("Hand Stabilization & Fall Down")]
    public bool improvedHandStabilization = false;
    public bool handsCanFallDown = false;
    public Vector3 restartForce = Vector3.up * 50f;
    public Rigidbody handRigid;
    public float handSpring_connectedAnchorHeight = 4.1f;



    bool rightTriggerIsActive, leftTriggerIsActive = false;

	// Update is called once per frame
	void Update()
	{

        // INDEX FINGER

        if (Input.GetKeyDown(KeyCode.Q) || (Input.GetAxis("Left Trigger") != 0 && !leftTriggerIsActive))
        {
            handAnimator.Play("indexStraightUP", 1);
            leftTriggerIsActive = true;
        }
		if (Input.GetKeyUp(KeyCode.Q) || Input.GetAxis("Left Trigger") == 0 && leftTriggerIsActive)
		{
			handAnimator.Play("indexStraightDOWN", 1);
            leftTriggerIsActive = false;
		}
		if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("Left Bumper"))
		{
			handAnimator.Play("indexCurvedUP", 1);
        }
		if (Input.GetKeyUp(KeyCode.A) || Input.GetButtonUp("Left Bumper"))
		{
			handAnimator.Play("indexCurvedDOWN", 1);
            
        }


		// MIDDLE FINGER
		if (Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Right Trigger") != 0 && !rightTriggerIsActive))
		{
			handAnimator.Play("middleStraightUP", 2);
            rightTriggerIsActive = true;
        }
		if (Input.GetKeyUp(KeyCode.W) || (Input.GetAxis("Right Trigger") == 0 && rightTriggerIsActive))
		{
			handAnimator.Play("middleStraightDOWN", 2);
            rightTriggerIsActive = false;
        }
		if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Right Bumper"))
		{
			handAnimator.Play("middleCurvedUP", 2);
		}
		if (Input.GetKeyUp(KeyCode.S) || Input.GetButtonUp("Right Bumper"))
		{
			handAnimator.Play("middleCurvedDOWN", 2);
		}

        // angle

        // only if the hand doesn't use the new stabilization (cause it wouldn't have the needed components)
        if (!improvedHandStabilization)
        {
            JointSpring spring = handTopJoint.spring;
            float valueToAdd = 0;
            if (Input.GetKey(KeyCode.C))
            {
                valueToAdd -= rotationSpeed;

            }
            else if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Y))
            {
                valueToAdd += rotationSpeed;

            }
            spring.targetPosition = Mathf.Clamp(spring.targetPosition + valueToAdd, -maxRotation, maxRotation);
            handTopJoint.spring = spring;
        }

        // Stand Up
        // (only if standUp is activated)
        if (handsCanFallDown)
        {
            if (Input.GetButtonDown("A Button") || Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(StandUp());
            }
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

    }
}
