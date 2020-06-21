using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationAndBalance : MonoBehaviour
{
	Settings settings { get { return Settings.instance; } }

	[Header("Orientation")]
	public GameObject orientationCentre;
	public GameObject configurableJoint;
	public GameObject lookAtTarget;
	public float hightToLookAt;
	public bool lookAtActive;

	[Header("Balance")]

	public bool fallinAndGettingUp;
	private float canFallTimer;

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
		lookAtActive = settings.lookAtActive;
    }

	// Update is called once per frame
	void Update()
	{

		// ORIENTATION
		orientationCentre.transform.position = configurableJoint.transform.position;

		if (lookAtActive && lookAtTarget != null)
		{
			orientationCentre.transform.LookAt(new Vector3(lookAtTarget.transform.position.x, orientationCentre.transform.position.y + hightToLookAt, lookAtTarget.transform.position.z));
		}

		if (settings.fallMode == Settings.FallMode.getUpAutomatically)
		{

			// BALANCE
			// update timers
			timer += Time.deltaTime;
			tick += Time.deltaTime;

			// on every tick
			if (tick >= maxTickValue)
			{
				tick = 0f;
				pastAngles.Enqueue(transform.eulerAngles); // IMPORTANT this needs to be global rotation

				// start dequeing when a delay is reached (eg. after 3 seconds)
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
		}

		if (settings.fallMode == Settings.FallMode.neverFall)
		{
			SetXDrive(maxdrive);
			SetYZDrive(maxdrive);

		}
		else if (settings.fallMode == Settings.FallMode.dontGetUp)
		{
			SetXDrive(0f);

			if (settings.fallDirection == Settings.FallDirection.XandZ)
			{
				SetYZDrive(0f);
			}
			else
			{
				SetYZDrive(maxdrive);
			}	
		}
	}

	private void SetSprings(Vector3 angle)
	{
		Debug.Log("setting springs");

		SetXDrive(minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.x)) / 180f)));

		if (settings.fallDirection == Settings.FallDirection.XandZ)
		{
			SetYZDrive(minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.z)) / 180f)));
		}
		else
		{
			SetYZDrive(maxdrive);
		}

		//Debug.Log($"x value: {FloatTo180Spectrum(angle.x)}, x drive: {minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.x)) / 180f))}, z value  {FloatTo180Spectrum(angle.z)},  y drive: {minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.z)) / 180f))} ");

	}

	private void SetXDrive(float value)
	{
		JointDrive XDrive = affectedJoint.angularXDrive;
		XDrive.positionSpring = value;
		affectedJoint.angularXDrive = XDrive;
	}

	private void SetYZDrive(float value)
	{
		JointDrive YZDrive = affectedJoint.angularYZDrive;
		YZDrive.positionSpring = value;
		affectedJoint.angularYZDrive = YZDrive;
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

	public void SetFallTimer( float time)
	{
		canFallTimer = time;
	}

	public void IncreaseFallTimer(float time)
	{
		canFallTimer += time;
	}
}
