using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationAndBalance : MonoBehaviour
{
	Settings settings { get { return Settings.instance; } }

	public PlayerInstance thisPlayer;
	public PlayerInstance otherPlayer;
	public ConfigurableJoint configJoint;

	[Header("Orientation")]
	public GameObject orientationCentre;
	public GameObject lookAtTarget;
	public float hightToLookAt;
	public bool lookAtActive;

	[Header("Balance")]
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

	float angularXDrive_targetValue;
	float angularXDrive_startValue;
	float angularXDrive_startDamper;

	// center of mass & angle-stabilization

	[Header("COM")]
	public List<RigidbodyTuple> rigidsForVisuaCOMCalculation = new List<RigidbodyTuple>();
	private List<Rigidbody> rigidsForMassBasedCOMCalculation = new List<Rigidbody>();
	public Vector3 COM;
	public GameObject com_obj;

	FightManager fightManager;



	// Start is called before the first frame update
	void Start()
    {
		lookAtActive = settings.lookAtActive;
		fightManager = FindObjectOfType<FightManager>();

		if (configJoint == null)
		{
			configJoint = this.GetComponent<ConfigurableJoint>();
			Debug.Log("getting the joint");
		}
		if (com_obj == null) 
		{
			com_obj = transform.Find("COM").gameObject;		
		}

		angularXDrive_startValue = configJoint.angularXDrive.positionSpring;
		angularXDrive_startDamper = configJoint.angularXDrive.positionDamper;

		GetRigidsForMassBasedCOM();
	}

	// Update is called once per frame
	void Update()
	{

        // ORIENTATION
        orientationCentre.transform.position = configJoint.transform.position;

		bool dontRotateCondition = (thisPlayer.activeAvatar.isDown || otherPlayer.activeAvatar.isDown) && fightManager.distanceBetweenPlayers <= 3f; // if at least one of the players lying down and the distance between them is small, dont rotate

		if (lookAtActive && lookAtTarget != null && !dontRotateCondition )
		{
			orientationCentre.transform.LookAt(new Vector3(lookAtTarget.transform.position.x, orientationCentre.transform.position.y + hightToLookAt, lookAtTarget.transform.position.z));
		}


		// BALANCE

        if (settings.fallMode != Settings.FallMode.spring_backFoot && settings.fallMode != Settings.FallMode.spring_feet && settings.fallMode != Settings.FallMode.autoBend && settings.fallMode != Settings.FallMode.angularDriveAndCOM)
        {
            SetXDrive(0);
            SetYDrive(0);
            SetZDrive(0);
        }

		if (settings.fallMode == Settings.FallMode.getUpAutomatically)
		{
			// update timers
			timer += Time.deltaTime;
			tick += Time.deltaTime;

			// on every tick
			if (tick >= maxTickValue)
			{
				tick = 0f;
				pastAngles.Enqueue(transform.eulerAngles); // Add current rotation to the queue

				// start taking out the rotatins from the que when delay is rached
				if (timer >= reactionDelay)
				{
					Vector3 pastAngle = pastAngles.Dequeue();
					SetSprings(pastAngle);
				}
			}
		}

		if (settings.fallMode == Settings.FallMode.constantAngularDrive)
		{
			SetAngularXDrive(maxdrive);
			SetAngularYZDrive(maxdrive);

		}
		else if (settings.fallMode == Settings.FallMode.noAngularDrives)
		{
			SetAngularXDrive(settings.springForce);

			if (settings.fallDirection == Settings.FallDirection.XandZ)
			{
				SetAngularYZDrive(settings.springForce);
			}
			else
			{
				SetAngularYZDrive(maxdrive);
			}
		}


		//  COM TREATMENT

		if (settings.fallMode == Settings.FallMode.angularDriveAndCOM) 
		{
			com_obj.transform.position =  GetWorldSpaceCOM();
			//COMBalance();
		}


	}

	public void SetLookAt(bool value)
	{
		lookAtActive = value;
	}
	private void SetSprings(Vector3 angle)
	{
		Debug.Log("setting springs");

		SetAngularXDrive(minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.x)) / 180f)));

		if (settings.fallDirection == Settings.FallDirection.XandZ)
		{
			SetAngularYZDrive(minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.z)) / 180f)));
		}
		else
		{
			SetAngularYZDrive(maxdrive);
		}

		//Debug.Log($"x value: {FloatTo180Spectrum(angle.x)}, x drive: {minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.x)) / 180f))}, z value  {FloatTo180Spectrum(angle.z)},  y drive: {minDrive + ((maxdrive - minDrive) * sprinngRelToAngle.Evaluate(Mathf.Abs(FloatTo180Spectrum(angle.z)) / 180f))} ");

	}

	private void SetAngularXDrive(float value)
	{
		JointDrive XDrive = affectedJoint.angularXDrive;
		XDrive.positionSpring = value;
		affectedJoint.angularXDrive = XDrive;
	}

	private void SetAngularXDamper(float value)
	{
		JointDrive XDrive = affectedJoint.angularXDrive;
		XDrive.positionDamper = value;
		affectedJoint.angularXDrive = XDrive;
	}

	private void SetAngularYZDrive(float value)
	{
		JointDrive YZDrive = affectedJoint.angularYZDrive;
		YZDrive.positionSpring = value;
		affectedJoint.angularYZDrive = YZDrive;
	}

    private void SetXDrive(float value)
    {
        JointDrive xDrive = affectedJoint.xDrive;
        xDrive.positionSpring = value;
        affectedJoint.xDrive = xDrive;
    }

    private void SetYDrive(float value)
    {
        JointDrive yDrive = affectedJoint.yDrive;
        yDrive.positionSpring = value;
        affectedJoint.yDrive = yDrive;
    }

    private void SetZDrive(float value)
    {
        JointDrive zDrive = affectedJoint.zDrive;
        zDrive.positionSpring = value;
        affectedJoint.zDrive = zDrive;
    }

	private Vector3 GetWorldSpaceCOM()
	{
		// declaration
		Vector3 mass_multipliedBy_position = Vector3.zero;
		float masses = 0;

		if (settings.comMode == Settings.ComMode.BasedOnActualMass)
		{
			// calculation
			for (int i = 0; i < rigidsForMassBasedCOMCalculation.Count; i++)
			{
				Rigidbody rigid = rigidsForMassBasedCOMCalculation[i];
				mass_multipliedBy_position += (rigid.mass * rigid.worldCenterOfMass);
				masses += rigid.mass;
			}

		}
		else if (settings.comMode == Settings.ComMode.BasedOnVisualMass)
		{ 
			// calculation
			for (int i = 0; i < rigidsForVisuaCOMCalculation.Count; i++)
			{
				RigidbodyTuple tuple = rigidsForVisuaCOMCalculation[i];
				mass_multipliedBy_position += (tuple.assumedMass * tuple.rigid.worldCenterOfMass);
				masses += tuple.assumedMass;
			}
		}

		COM = mass_multipliedBy_position / masses;
		return COM;
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


	void GetRigidsForMassBasedCOM()
	{
		rigidsForMassBasedCOMCalculation = new List<Rigidbody>();
		rigidsForMassBasedCOMCalculation.Add(thisPlayer.activeAvatar.indexFinger.fingerTop.GetComponent<Rigidbody>());
		rigidsForMassBasedCOMCalculation.Add(thisPlayer.activeAvatar.indexFinger.fingerMiddle.GetComponent<Rigidbody>());
		rigidsForMassBasedCOMCalculation.Add(thisPlayer.activeAvatar.indexFinger.fingerBottom.GetComponent<Rigidbody>());
		rigidsForMassBasedCOMCalculation.Add(thisPlayer.activeAvatar.middleFinger.fingerTop.GetComponent<Rigidbody>());
		rigidsForMassBasedCOMCalculation.Add(thisPlayer.activeAvatar.middleFinger.fingerMiddle.GetComponent<Rigidbody>());
		rigidsForMassBasedCOMCalculation.Add(thisPlayer.activeAvatar.middleFinger.fingerBottom.GetComponent<Rigidbody>());
		rigidsForMassBasedCOMCalculation.Add(thisPlayer.activeAvatar.torsoJoint.GetComponent<Rigidbody>());

		if (settings.useAllFingersForCOM)
		{
			Rigidbody[] remainingFingerRigids = thisPlayer.activeAvatar.torsoJoint.GetComponentsInChildren<Rigidbody>();
			foreach (Rigidbody rigid in remainingFingerRigids)
				rigidsForMassBasedCOMCalculation.Add(rigid);
		}
	}

	private void COMBalance()
	{
		//adjuct com obj position
		com_obj.transform.eulerAngles = new Vector3(0f, com_obj.transform.eulerAngles.y, 0f);
		com_obj.transform.localEulerAngles = new Vector3(com_obj.transform.localEulerAngles.x, 0f, com_obj.transform.localEulerAngles.z);
		
		Vector3 indexTipToCom = com_obj.transform.InverseTransformPoint(thisPlayer.activeAvatar.indexFinger.fingerTip.transform.position);
		Vector3 middleTipToCom = com_obj.transform.InverseTransformPoint(thisPlayer.activeAvatar.middleFinger.fingerTip.transform.position);

		if (settings.angularDriveBreaking == Settings.AngularDriveBreaking.FromAnimationCurve)
		{
			// get the value of the closer foot

			//set angulardrive based on animation curve
		}

		else
		{
			if (indexTipToCom.z > settings.fallDistance && middleTipToCom.z > settings.fallDistance ||
				indexTipToCom.z < -settings.fallDistance && middleTipToCom.z < -settings.fallDistance)
			{
				// break angular drives
				if (settings.angularDriveBreaking == Settings.AngularDriveBreaking.SuddenBreak)
				{
					SetAngularXDrive(0);
					SetAngularXDamper(0);
				}
				else if (settings.angularDriveBreaking == Settings.AngularDriveBreaking.TargetValueLerp)
				{
					angularXDrive_targetValue = 0;
					LerpAngularDrive();
				}
			}
			else
			{
				// set angular drive
				if (settings.angularDriveBreaking == Settings.AngularDriveBreaking.SuddenBreak)
				{
					SetAngularXDrive(angularXDrive_startValue);
					SetAngularXDamper(angularXDrive_startDamper);
				}

				else if (settings.angularDriveBreaking == Settings.AngularDriveBreaking.TargetValueLerp)
				{
					angularXDrive_targetValue = angularXDrive_startValue;
					LerpAngularDrive();
				}
			}
		}
	}

	private void LerpAngularDrive()
	{
		float newDriveValue = Mathf.Lerp(configJoint.angularXDrive.positionSpring, angularXDrive_targetValue, settings.lerpSpeed * Time.deltaTime);
		SetAngularXDrive(newDriveValue);
	}
}

[System.Serializable]
public class RigidbodyTuple
{
	public Rigidbody rigid;
	public float assumedMass;
}