using UnityEngine;
using System.Collections;

public class CopyAnimation : MonoBehaviour
{

	public Joint jointToMove;
	public Transform target;
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
				targetRotation = initialLocalrotation - target.localEulerAngles;
				convertedTargetRotation = Vector3To180Spectrum(targetRotation); // commment this out and enter values manually for debugging
				cj.targetRotation = Quaternion.Euler(new Vector3 (convertedTargetRotation.x * amplificationFactor.x, convertedTargetRotation.y * amplificationFactor.y, convertedTargetRotation.z * amplificationFactor.z));
                //if (this.gameObject.name == "Index_3")
                //    print("targetRotation: " + new Vector3(convertedTargetRotation.x * amplificationFactor.x, convertedTargetRotation.y * amplificationFactor.y, convertedTargetRotation.z * amplificationFactor.z));

			}

		}
	}

	private Vector3 Vector3To180Spectrum(Vector3 vector)
	{
		return (new Vector3(FloatTo180Spectrum(vector.x), FloatTo180Spectrum(vector.y), FloatTo180Spectrum(vector.z)));
	}

	// needs reworking for angles bigger than 360???
	private float FloatTo180Spectrum(float value)
	{
		if (value > 180)
		{
			return value - 360;
		}

		if (value < -180)
		{
			return value + 360; 
		}
		return value;



	}
}

