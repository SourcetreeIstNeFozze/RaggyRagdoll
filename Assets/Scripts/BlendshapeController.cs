using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendshapeController : MonoBehaviour
{
    [Header("Mesh")]
    public SkinnedMeshRenderer deformedObject;

    public enum rotAxis {
        X,
        Y,
        Z
    };


    [Header("Constraint")]

    public rotAxis rotationAxis;

    public bool useBlendShapeIndex = false;

    public int blendShapeIndex = 0;

    public string blendShapeName = "ENTER BLENDSHAPE NAME HERE";

    [Tooltip("Minimum Angle of Axis(as a Quaternion)- Angle where Blendshape is 0")]
    [Range(-1, 1)]
    public float minAngle = 0;

    [Tooltip("Maximum Angle of Axis(as a Quaternion)- Angle where Blendshape is 1")]
    [Range(-1, 1)]
    public float maxAngle = 1;



    [Header ("Debug - Setup")]

    public Quaternion currentRotation;

    public string[] blendShapeNames;

    bool stopUpdating = false;



    void Awake()
    {
        if (!useBlendShapeIndex)
        {
            blendShapeNames = getBlendShapeNames(deformedObject);
            if (blendShapeNames.Contains(blendShapeName))
            {
                blendShapeIndex = Array.FindIndex(blendShapeNames, item => item == blendShapeName);
            }
            else
            {
                Debug.LogWarning("Blendshapename " + blendShapeName + " was not Found or was typed incorrect, check this gameobject : " + this.gameObject.transform.name);
                stopUpdating = true;
            }
        }
    }

    void Update()
    {
        UpdateBlendshapes();
    }

    void UpdateBlendshapes()
    {
        currentRotation = this.gameObject.transform.localRotation;
        if(deformedObject != null && stopUpdating == false)
        {
            deformedObject.SetBlendShapeWeight(blendShapeIndex, Mathf.InverseLerp(minAngle, maxAngle, this.gameObject.transform.localRotation.x) * 100);
        }
    }


    public string[] getBlendShapeNames(SkinnedMeshRenderer _SkMeRe)
    {
        Mesh m = _SkMeRe.sharedMesh;
        string[] arr;
        arr = new string[m.blendShapeCount];
        for (int i = 0; i < m.blendShapeCount; i++)
        {
            string s = m.GetBlendShapeName(i);
            arr[i] = s;
        }
        return arr;
    }
}




