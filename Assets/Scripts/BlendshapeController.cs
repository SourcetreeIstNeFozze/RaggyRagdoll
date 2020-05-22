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
    public int blendShapeIndex;

    [Range(-1, 1)]
    public float minAngle;
    [Range(-1, 1)]
    public float maxAngle;

    [Header ("Debug - Setup")]

    public Quaternion currentRotation;

    void Update()
    {
        UpdateBlendshapes();
    }

    void UpdateBlendshapes()
    {
        currentRotation = this.gameObject.transform.localRotation;
        if(deformedObject != null)
        {
            deformedObject.SetBlendShapeWeight(blendShapeIndex, Mathf.InverseLerp(minAngle, maxAngle, this.gameObject.transform.localRotation.x) * 100);
        }
    }
}
        // spaeter einbauen ! - vielleicht blendshape by name aktivieren?

//public string[] getBlendShapeNames(GameObject obj)
//{
//    SkinnedMeshRenderer head = obj.GetComponent<SkinnedMeshRenderer>();
//    Mesh m = head.sharedMesh;
//    string[] arr;
//    arr = new string[m.blendShapeCount];
//    for (int i = 0; i < m.blendShapeCount; i++)
//    {
//        string s = m.GetBlendShapeName(i);
//        print("Blend Shape: " + i + " " + s); // Blend Shape: 4 FightingLlamaStance
//        arr[i] = s;
//    }
//    return arr;
//}


