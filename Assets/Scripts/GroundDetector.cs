using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
	public bool touchesGround;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	/// <summary>
	/// FROM UNITY GUY
	/// If you just want a measurement of how strong the hit was (like, for example for damage calculations), the dot product of collision normal and collision velocity (ie the velocity of the two bodies relative to each other), times the mass of the other collider should give you useful values. 
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Environment")
		{
			touchesGround = true;
			//Debug.Log("ground collision strength:" + collision.impactForceSum);
		} 
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.collider.tag == "Environment")
		{
			touchesGround = false;
		}
	}

}
