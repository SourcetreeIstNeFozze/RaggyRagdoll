using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandReferences : MonoBehaviour
{

	public ActiveFinger indexFinger;
	public ActiveFinger middleFinger;
	public CollisionHandler[] wristTriggers;
	public HingeJoint torsoJoint;
	public GameObject playerRoot;

	[HideInInspector] public CollisionHandler[] childHandlers;
	[HideInInspector] public OrientationAndBalance balance;
	[HideInInspector] public Rigidbody playerRigidbody;
	[HideInInspector] public ConstantForce playerConstanctForce;

	[Header("Resetting")]
	[HideInInspector] public List<Transform> childTransofrms = new List<Transform>();
	private List<Quaternion> originalRotations = new List<Quaternion>();
	private List<Vector3> originalPositions = new List<Vector3>();

	private void Awake()
	{
		//get References
		childHandlers = GetComponentsInChildren<CollisionHandler>();
		playerRigidbody = playerRoot.GetComponent<Rigidbody>();
		playerConstanctForce = playerRoot.GetComponent<ConstantForce>();
		balance = playerRoot.GetComponent<OrientationAndBalance>();

		// get initial state the hand
		foreach (Transform child in transform)
		{
			childTransofrms.Add(child);
			originalRotations.Add(child.rotation);
			originalPositions.Add(child.position);
		}
	}

	public void Reset()
	{
		for (int i = 0; i < childTransofrms.Count; i++)
		{
			childTransofrms[i].position = originalPositions[i];
			childTransofrms[i].rotation = originalRotations[i];

			Rigidbody childRigidbody = childHandlers[i].rigid;
			if (childRigidbody != null)
			{
				childRigidbody.velocity = Vector3.zero;
			}
		}
	}

}
