using UnityEngine;
using System.Collections;

public class CopyAnimation : MonoBehaviour
{

	public Joint jointToMove;
	public Transform target;

	[Header("If Hinge Joint")]
	[Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
	public bool x;
	public bool y;
	public bool z;
	public bool invert;

	[Header("ic configurable joint")]
	private Quaternion initialRotation;
	private Vector3 initialLocalrotation; 

	void Start()
	{
		initialRotation = this.transform.rotation;
		initialLocalrotation = this.transform.localEulerAngles;
	}

	void Update()
	{
		if (jointToMove != null && target != null)
		{

			/// use the axis in the joint to rotation azis -.-
			if (jointToMove is HingeJoint)
			{
				HingeJoint hj = (HingeJoint)jointToMove;
				JointSpring js;
				js = hj.spring;

				if (x)
				{
					js.targetPosition = target.transform.localEulerAngles.x;
				}
				else if (y)
				{
					js.targetPosition = target.transform.localEulerAngles.y;
				}
				else if (z)
				{
					js.targetPosition = target.transform.localEulerAngles.z;
				}

				if (js.targetPosition > 180)
					js.targetPosition = js.targetPosition - 360;
				if (invert)
					js.targetPosition = js.targetPosition * -1;

				js.targetPosition = Mathf.Clamp(js.targetPosition, hj.limits.min + 5, hj.limits.max - 5);

				hj.spring = js;
			}

			else if (jointToMove is ConfigurableJoint)
			{
				ConfigurableJoint cj = (ConfigurableJoint)jointToMove;
				cj.targetRotation = Quaternion.Euler(initialLocalrotation - target.localEulerAngles);

			}

		}
	}
}

