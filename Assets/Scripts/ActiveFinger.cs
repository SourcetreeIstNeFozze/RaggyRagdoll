using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveFinger
{
	public StickInput stickInput;
    public StickInput stickInput_original;
    public CollisionHandler fingerTop;
	public CollisionHandler fingerMiddle;
	public CollisionHandler fingerBottom;

}
