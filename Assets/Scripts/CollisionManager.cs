using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
	public static CollisionManager instance;
	private List<ExtendedCollision> collisionsThisFrame = new List<ExtendedCollision>();

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		if (this != instance)
		{
			Destroy(this);
		}
	}
	private void LateUpdate()
	{
		collisionsThisFrame.Clear();
	}

	public bool AddCollision(ExtendedCollision collisionToAdd)
	{
		for (int i = 0; i < collisionsThisFrame.Count; i++)
		{
			if (collisionToAdd.IsTheSameCollisionAs(collisionsThisFrame[i]))
			{
				return false;
			}
		}
		collisionsThisFrame.Add(collisionToAdd);
		return true;
	}
}

public class ExtendedCollision
{
	public Collision collision;
	public Rigidbody receiverRigidbody;
	public Collider receiverCollider;
	public GameObject receiverGameobject;

	public ExtendedCollision(GameObject receiverGameobject, Rigidbody receiverRigidbody, Collider receiverCollider, Collision collision)
	{
		this.collision = collision;
		this.receiverRigidbody = receiverRigidbody;
		this.receiverCollider = receiverCollider;
		this.receiverGameobject = receiverGameobject;
	}

	public bool IsTheSameCollisionAs(ExtendedCollision otherCollision)
	{
		if (this.collision.contacts == otherCollision.collision.contacts)
		{
			Debug.Log("Doubled collision found");
			return true;
		}

		return false;
	}
}
