using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJBalancingwithFalling : MonoBehaviour
{
	[SerializeField] ConfigurableJoint affectedJoint;
	[SerializeField] private Vector3 targetAngle;
	[SerializeField] private AnimationCurve sprinngRelToAngle;
	[SerializeField] private float reactionDelay; // how much should the reaction be delayed

	private Queue<Vector3> pastAngles = new Queue<Vector3>();
	private float tick = 0f;
	private float maxTickValue = 0.2f; // how often to check the values?
	private float timer = 0f;

	public float minDrive = 100;
	public float maxdrive = 1000;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// update timers
		timer += Time.deltaTime;
		tick += Time.deltaTime;

		// on every tick
		if (tick >= maxTickValue)
		{
			tick = 0f;
			pastAngles.Enqueue(transform.eulerAngles); // IMPORTANT this needs to be global rotation

			// start dequeing when a delay is reached
			if (timer >= reactionDelay)
			{
				//deque
				Vector3 pastAngle = pastAngles.Dequeue();

				////get difference in rotation. IMPORTANt: DOING THIS IN QUATERNION SMARTER????
				//Vector3 angleDifference = targetAngle - pastAngle; //gets actualdifference
				//angleDifference = new Vector3(Mathf.Abs(angleDifference.x), Mathf.Abs(angleDifference.y), Mathf.Abs(angleDifference.z)); // gets absolue vlaues for teh differenc

				SetSprings(pastAngle);
			}
		}

		// something about adjusting the anchor to point upwards if the rotation is free grrr
    }

	private void SetSprings(Vector3 angle)
	{
		JointDrive XDrive =  affectedJoint.angularXDrive;
		XDrive.positionSpring = minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.x)) / 180f));
		affectedJoint.angularXDrive = XDrive;

		JointDrive YZDrive = affectedJoint.angularYZDrive;
		YZDrive.positionSpring = minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.z)) / 180f));
		affectedJoint.angularYZDrive = YZDrive;

		//Debug.Log($"x value: {FloatTo180Spectrum(angle.x)}, x drive: {minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.x)) / 180f))}, z value  {FloatTo180Spectrum(angle.z)},  y drive: {minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.z)) / 180f))} ");

	}

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
