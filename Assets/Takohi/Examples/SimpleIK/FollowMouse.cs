using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        curPosition.z = 0f;
        transform.position = curPosition;
	}

    void OnGUI () {
        if(GUI.Button(new Rect(20f, 20f, 200f, 60f), "-- See Tutorial --\nLearn how to use Simple IK"))
            Application.OpenURL("http://www.takohi.com/use-simple-ik-on-unity/");
    }
}
