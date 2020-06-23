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


	[HideInInspector] public CollisionHandler[] childHandlers;
	[HideInInspector] public OrientationAndBalance balance;
	[HideInInspector] public Rigidbody playerRigidbody;
	[HideInInspector] public ConstantForce playerConstanctForce;



	private void Awake()
	{
		//get References
		childHandlers = GetComponentsInChildren<CollisionHandler>();
		playerRigidbody = playerRoot.GetComponent<Rigidbody>();
		playerConstanctForce = playerRoot.GetComponent<ConstantForce>();
		balance = playerRoot.GetComponent<OrientationAndBalance>();

	}



}
