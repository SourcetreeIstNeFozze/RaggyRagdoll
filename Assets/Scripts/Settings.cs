using UnityEngine;

public class Settings : MonoBehaviour
{

	public static Settings instance;

	[Header("Movement")]
	public bool useFK;
	public bool useTriggersCurl = true;
	public bool amplifyJump;
	public float jumpForce = 10;
	public bool compassBending;
	public bool lookAtActive;
	public enum TransformType { global, local };
	public TransformType poseSpace = TransformType.global;
	public float hipRotationSpeed;
	public float maxHipRotation = 50;
	public float maxHipPushForce = 10;
	

	[Header("Falling and Stabilisation")]
	public FallDirection fallDirection;
	public enum FallDirection { X, XandZ }
	public enum FallMode { neverFall, getUpAutomatically, dontGetUp, spring_backFoot, spring_feet, bend, autoBend, angularDriveAndCOM}
	public FallMode fallMode;
	public float springForce;

	[Header("Collision Amplification")]
	public ColisionAmplificationMode colisionAmplificationMode;
	public enum ColisionAmplificationMode { none, velocityChange, velocityAddition }
	public enum VelocityMode { accuratePhysics, actualVelocityWrongDirection,velocityAndDirectionPulledFromYourAss }
	public VelocityMode velocityMode;
	[Range(0, 100)] public float fingerTipsAdditionalForce = 3;
	[Range(0, 100)] public float otherPartsAdditionalForce = 1;

	[Header("Joint weakening")]
	[Range(0, 50)] public float jointSpringForceWhenWeek = 20;
	public enum JointWeakening { none, instantReturn, gradualReturn}
	public JointWeakening jointsWeakening;
	public float durationOfJointWeakness = 3f;
	
	//[Header("Loosing orientation")]
	//public bool loseOrientationOnCollision = false;
	//public float timeOfLostOrientation = 3;
	
	[Header("Detect fast stick releases")]
	public float stickReleaseTimeWindow = 0.1f;

    [Header("If spring fall mode")]
    public float configJoint_Y_Offset = 2f;
    public float anchorInputStrength = 0.1f;
    public bool breakAnchorAtLimit = true;
    //public float anchorBreakDistanceLimit = 0.8f;
    public float anchorBreakAngleLimit = 45f;

    [Header("If AngleAndCOM fall mode")]
    public bool useAllFingersForCOM = true;
    public float fallDistance = 0.3f;

	[Header("CountDown")]
	public float otherPlayerInfluenceTime = 5;
	public float coundDownTime = 5;
	public enum CountDownMode { resetting, continuous }
	public CountDownMode countDownMode;
	private void Awake()
	{
		if (Settings.instance == null)
		{
			Settings.instance = this;
		}
	}
}
