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
			// I asume two collisions are two calls of the same collision if they have the same set of contact points
			// technically is not true but the possibility for it being wrong is negligible
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
	List<CollisionHandler> collisionObjects;

	
	public ExtendedCollision( Collision collision, List<CollisionHandler> collisionObjects)
	{
		this.collision = collision;
		this.collisionObjects = collisionObjects;
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

