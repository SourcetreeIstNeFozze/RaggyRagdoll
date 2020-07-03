using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseDetector : MonoBehaviour
{
	public bool showDebug;

    public List<Pose> poseLibrary;
	private Dictionary<string, Transform> jointTranforms = new Dictionary<string, Transform>();
    public Pose currentPose;
	public string currentPoseName; 

	private void Awake()
	{
		GetNeededTransforms();
	}
	// Start is called before the first frame update
	void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        currentPose = GetCurrentPose();
		currentPoseName = GetCurrentPoseName();
    }

    private void GetNeededTransforms() 
    {
		// for each available pose
		for (int i = 0; i < poseLibrary.Count; i++)
		{
			// check each joint
			for (int j = 0; j < poseLibrary[i].jointPositions.Count; j++)
			{
				string jointName = poseLibrary[i].jointPositions[j].jointName;

				// if the dictionary does not include joint with that name add it to the dictionary for tracking, otherwise skip
				if (!jointTranforms.ContainsKey(jointName))
				{
					Transform transformToAdd = ExtensionMethods.FindChild(this.transform, jointName);

					jointTranforms.Add(jointName, transformToAdd);

					if (showDebug)
						Debug.Log($"Adding joint with name {jointName} joint is null:{ transformToAdd == null}", transformToAdd);
				}
			}
		}
	}


	public bool CurrentPoseIs(string poseName) 
	{
		if (currentPose != null)
		{
			return currentPose.name == poseName;
		}
		else 
		{
			return false;
		}
			
	}

	public string GetCurrentPoseName() 
	{
		if (currentPose != null)
		{
			return currentPose.name;
		}
		else
		{
			return "Null";
		}
	}
	private Pose GetCurrentPose()
	{
        // for each available pose
		for (int i = 0; i < poseLibrary.Count; i++)
		{
			bool missmatch = false;
			for (int j = 0; j < poseLibrary[i].jointPositions.Count; j++)
			{
				//check if the current position of the transforms match with the angles of the ckeckked pose
				// if at least one joint does not match, break
				string jointName = poseLibrary[i].jointPositions[j].jointName;

				if (!TransformFitsPosition(jointTranforms[jointName], poseLibrary[i].jointPositions[j]))
				{
					if (showDebug)
						Debug.Log($"No match at {jointName}, pose {poseLibrary[i]}");
					missmatch = true;
					break;
				}
				else
				{
					if (showDebug)

						Debug.Log($"Match at {jointName}, pose {poseLibrary[i]}");
				}
			}

			// if no mismatch on the way return curren pose
			if (!missmatch) 
			{
				Debug.Log("returning pose:" + poseLibrary[i].name);

				return poseLibrary[i];
			}
	
		}
		Debug.Log("returning null");
		return null;
	}

	private bool TransformFitsPosition(Transform transform, JointRotation targetRotation)
	{
		return IsWithinRange(ExtensionMethods.Vector3To180Spectrum(transform.localEulerAngles), targetRotation.targetRotation, targetRotation.tolerance);
	}

	public bool IsWithinRange(float value, float target, float tolerance) 
	{
		Debug.Log($"value { value}, target {target}, tolerance {tolerance} is withing range {value < target + tolerance && value > target - tolerance}");
		return value < target + tolerance && value > target - tolerance;
	}

	public bool IsWithinRange(Vector3 value, Vector3 target, Vector3 tolerance)
	{

		return IsWithinRange(value.x, target.x, tolerance.x)
			&& IsWithinRange(value.y, target.y, tolerance.y)
			&& IsWithinRange(value.z, target.z, tolerance.z);
	}


}