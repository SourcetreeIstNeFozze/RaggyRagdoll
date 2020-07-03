using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[System.Serializable]
public struct JointRotation
{
    public string jointName;
    public Vector3 targetRotation;
    public Vector3 tolerance;

    public JointRotation(string jointName) 
    {
        this.jointName = jointName;
        targetRotation = Vector3.zero;
        tolerance = new Vector3(360, 360, 360);
    }

}
