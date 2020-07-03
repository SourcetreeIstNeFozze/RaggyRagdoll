using UnityEngine;
using System.Collections;

public class CopyAnimation : MonoBehaviour
{

	public Joint jointToMove;
	public Transform source;
	public Vector3 amplificationFactor = new Vector3(1f, 1f, 1f);

	[Header("If Hinge Joint")]
	[Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
	public bool x;
	public bool y;
	public bool z;
	public bool invert;

	[Header("if configurable joint")]
	public Vector3 targetRotation;
	public Vector3 convertedTargetRotation;
	private Quaternion initialRotation;
	private Vector3 initialLocalrotation;

    private Transform[] animatorFingers;

	void Start()
	{
		initialRotation = this.transform.rotation;
		initialLocalrotation = this.transform.localEulerAngles;

        // dynamically get copyAnimation sources
        animatorFingers = this.transform.root.GetComponent<PlayerInputController>().handAnimator.gameObject.GetComponentsInChildren<Transform>();
        foreach(Transform finger in animatorFingers)
        {
            if (finger.name == this.name)
            {
                this.source = finger;
                break;
            }
        }
        if (this.source == null)
            print("MISSING REFERENCE");
	}

	void Update()
	{
		if (jointToMove != null && source != null)
		{

			/// use the axis in the joint to rotation azis -.-
			if (jointToMove is HingeJoint)
			{
				HingeJoint hj = (HingeJoint)jointToMove;
				JointSpring js;
				js = hj.spring;

				if (x)
				{
					js.targetPosition = source.transform.localEulerAngles.x;
				}
				else if (y)
				{
					js.targetPosition = source.transform.localEulerAngles.y;
				}
				else if (z)
				{
					js.targetPosition = source.transform.localEulerAngles.z;
				}

				if (js.targetPosition > 180)
					js.targetPosition = js.targetPosition - 360;
				if (invert)
					js.targetPosition = js.targetPosition * -1;

				// if use limits is on, clamp
				if (hj.useLimits)
				{
					js.targetPosition = Mathf.Clamp(js.targetPosition, hj.limits.min , hj.limits.max);
				}
				hj.spring = js;
			}

			else if (jointToMove is ConfigurableJoint)
			{
				ConfigurableJoint cj = (ConfigurableJoint)jointToMove;
				targetRotation = initialLocalrotation - source.localEulerAngles;
				convertedTargetRotation = ExtensionMethods.Vector3To180Spectrum(targetRotation); // commment this out and enter values manually for debugging
				cj.targetRotation = Quaternion.Euler(new Vector3 (convertedTargetRotation.x * amplificationFactor.x, convertedTargetRotation.y * amplificationFactor.y, convertedTargetRotation.z * amplificationFactor.z));
                //if (this.gameObject.name == "Index_3")
                //    print("targetRotation: " + new Vector3(convertedTargetRotation.x * amplificationFactor.x, convertedTargetRotation.y * amplificationFactor.y, convertedTargetRotation.z * amplificationFactor.z));

			}

		}
	}


}

