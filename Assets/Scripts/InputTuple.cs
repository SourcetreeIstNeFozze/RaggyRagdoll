using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputTuple
{
	public Vector2 value;
	public float deltaTime;

	public InputTuple(Vector2 value, float deltaTime)
	{
		this.value = value;
		this.deltaTime = deltaTime;
	}
}
