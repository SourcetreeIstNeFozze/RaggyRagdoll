using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
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



}
