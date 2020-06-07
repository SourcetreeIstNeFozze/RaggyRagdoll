using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientation : MonoBehaviour
{
	public GameObject objectToOrient;
	public GameObject lookAtTarget;
	public float hightToLookAt;

	public bool LookAtActive;

	private void Update()
	{
		this.transform.position = objectToOrient.transform.position;

		if (LookAtActive)
		{
			// to do lerping this
			transform.LookAt( new Vector3(lookAtTarget.transform.position.x, this.transform.position.y + hightToLookAt, lookAtTarget.transform.position.z));
		}
	}


}
