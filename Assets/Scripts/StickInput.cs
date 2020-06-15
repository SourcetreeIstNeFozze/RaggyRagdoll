using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickInput
{
	public Vector2 value;
	public List<InputTuple> historicInput = new List<InputTuple>();

	public System.Action OnReleased_X;
	public System.Action OnReleased_Y;

	public float released_X_timer;
	public float released_Y_timer;

    // Update is called once per frame
    public void Update()
    {
		released_X_timer += Time.deltaTime;
		released_Y_timer += Time.deltaTime;
		
		//TO DO: set the maximal size for the list to avoid memory leaks
		historicInput.Add(new InputTuple(value, Time.deltaTime));
	}

	/// <summary>
	/// Tigger OnReleased_X and OnReleased_Y when the stick is released
	/// </summary>
	/// <param name="timeWindowtoCheck"> how much into history to look for spikes</param>
	/// <param name="fromValue"> value considered as "stick released"</param>
	/// <param name="toValue">  value consideret as "stick at full"</param>
	public void CheckForFastStickReleases(float timeWindowtoCheck, float fromValue, float toValue, float coolDown)
	{

		// X VALUE

		// if the X value is low enough for checking
		if (value.x <= toValue && released_X_timer > coolDown)
		{
			float inspectedTime = 0f;

			// loop backwards throuh input history up untill the given time
			for (int i = historicInput.Count - 1; i >= 0; i--)
			{
				inspectedTime += historicInput[i].deltaTime;

				// if desired time reached, stop checking
				if (inspectedTime > timeWindowtoCheck)
				{
					break;
				}

				// else fire event if "spike" was found
				if (historicInput[i].value.x >= fromValue)
				{
					OnReleased_X?.Invoke();
					released_X_timer = 0;
				}
			}
		}

		//Y VALUE 
		// if the Y value is low enough for checking
		if (value.y <= toValue && released_Y_timer > coolDown)
		{
			float inspectedTime = 0f;

			// loop backwards throuh input history up untill the given time
			for (int i = historicInput.Count - 1; i >= 0; i--)
			{
				inspectedTime += historicInput[i].deltaTime;

				// if desired time reached, stop checking
				if (inspectedTime > timeWindowtoCheck)
				{
					break;
				}

				// else fire event if "spike" was found
				if (historicInput[i].value.y >= fromValue)
				{
					OnReleased_Y?.Invoke();
					released_Y_timer = 0;
				}
			}
		}
	}
}
