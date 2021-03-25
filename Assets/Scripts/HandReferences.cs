using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandReferences : MonoBehaviour
{
	public GameObject playerRoot;
	public HingeJoint torsoJoint;

	public ActiveFinger indexFinger;
	public ActiveFinger middleFinger;
	public CollisionHandler[] wristTriggers;
	public CollisionHandler[] shokwavetriggers;

	public bool isDown;
	public bool isOut;
	public bool softened;

	[HideInInspector] public CollisionHandler[] childHandlers;
	[HideInInspector] public OrientationAndBalance balance;
	[HideInInspector] public Rigidbody playerRigidbody;
	[HideInInspector] public ConstantForce playerConstanctForce;


	public void GetReferences() 
	{
		//get References
		childHandlers = GetComponentsInChildren<CollisionHandler>();
		playerRigidbody = playerRoot.GetComponent<Rigidbody>();
		playerConstanctForce = playerRoot.GetComponent<ConstantForce>();
		balance = playerRoot.GetComponent<OrientationAndBalance>();

		Debug.Log("Player Avatar Initialized");
	}

	public void SoftenFingers()
	{
		Debug.Log("softening");
		softened = true;
		for (int i = 0; i < childHandlers.Count(); i++)
		{
			if (childHandlers[i].joint is HingeJoint)
			{
				HingeJoint hingeJoint =(HingeJoint) childHandlers[i].joint;
				JointSpring springDrive = hingeJoint.spring;
				springDrive.spring = 0;
				springDrive.damper = 0;
				hingeJoint.spring = springDrive;
			}
		}
	}

	public void HardenFingers() 
	{

		softened = false;
		for (int i = 0; i < childHandlers.Count(); i++)
		{
			if (childHandlers[i].joint is HingeJoint)
			{
				HingeJoint hingeJoint = (HingeJoint)childHandlers[i].joint;
				JointSpring springDrive = hingeJoint.spring;
				springDrive.spring = childHandlers[i].originalSpringforce;
				springDrive.damper = childHandlers[i].originalDamper;
				hingeJoint.spring = springDrive;

			}
		}
	}



}
