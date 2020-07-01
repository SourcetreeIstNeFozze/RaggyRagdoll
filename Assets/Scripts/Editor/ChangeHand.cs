using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ChangeHand : EditorWindow
{
    public static GameObject handParent;
    public static GameObject geometry;
    public static GameObject torso;
    public static GameObject middle3;
    public static GameObject middle2;
    public static GameObject middle1;
    public static GameObject index3;
    public static GameObject index2;
    public static GameObject index1;


    int capacity;

    public static List<GameObject> objectReferenceList;
    public static List<GameObject> objectPreviousList;

    public static List<GameObject> objectInputList;
    public static List<string> objectFieldListNames;


    [MenuItem("DevTools/Change Hand Model")]
    static void OpenWindow()
    {
        ChangeHand window = (ChangeHand)GetWindow(typeof(ChangeHand));
        window.minSize = new Vector2(700, 200);
        window.Show();

        objectInputList = new List<GameObject>() {handParent ,geometry, torso, middle3, middle2, middle1, index3, index2, index1 };
        objectFieldListNames = new List<string>() {"hand parent", "geometry", "torso", "middle3", "middle2", "middle1", "index3", "index2", "index1" };
        objectReferenceList= new List<GameObject>();
        objectPreviousList= new List<GameObject>();

        foreach (GameObject gO in objectInputList) // set the length of the List to the length of objectFieldList
        {
            objectReferenceList.Add(null);
            objectPreviousList.Add(null);
        }
    }

    private void OnGUI()
    {
        ObjectFieldCreator(objectInputList, objectReferenceList, objectPreviousList , objectFieldListNames);

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Set new Inputs")) // ADD ICONDITION SO THAT EVERYTHING HAS TO BE FILLED IN
        {
            SetInputHandToReferernce();
        }
        if (GUILayout.Button("Delete old Objects") && !objectPreviousList.Contains(null)) // ADD ICONDITION SO THAT EVERYTHING HAS TO BE FILLED IN
        {
            DeleteOldObjects();
        }
    }

    public void ObjectFieldCreator(List<GameObject> _objectInputList, List<GameObject> _objectReferenceList, List<GameObject> _objectPreviousList, List<string> _objectFieldNameList)
    {
        capacity = _objectInputList.Count; //get the amount of fields that has to be drawn
                                                        //capacity = EditorGUILayout.IntField("Capacity", capacity); //VISUALIZE THE FIELD
        capacity = Mathf.Max(0, capacity);

        //add any new fields
        for (int m = _objectInputList.Count; m < capacity; m++)
        {
            _objectInputList.Add(null);
            _objectReferenceList.Add(null);
            _objectPreviousList.Add(null);
        }
            

        //remove extra fields
        for (int m = capacity; m < _objectInputList.Count; m++)
        {
            _objectInputList.RemoveAt(_objectInputList.Count - 1);
            _objectReferenceList.RemoveAt(_objectReferenceList.Count - 1);
            _objectPreviousList.RemoveAt(_objectReferenceList.Count - 1);
        }

        //display the field
        for (int m = 0; m < _objectInputList.Count; m++)
        {
            EditorGUILayout.BeginHorizontal();
            _objectReferenceList[m] = (EditorGUILayout.ObjectField(_objectFieldNameList[m] + " Reference", _objectReferenceList[m], typeof(GameObject), true, GUILayout.Width(300))) as GameObject;
            _objectInputList[m] = (EditorGUILayout.ObjectField(_objectFieldNameList[m] + " Input", _objectInputList[m], typeof(GameObject), true, GUILayout.Width(300))) as GameObject;
            _objectPreviousList[m] = (EditorGUILayout.ObjectField(_objectFieldNameList[m] + " Prev. Assigned", _objectPreviousList[m], typeof(GameObject), true, GUILayout.Width(300))) as GameObject;

            EditorGUILayout.EndHorizontal();
        }
    }


    void SetPositionToInput(GameObject _reference, GameObject _input)
    {
        _reference.transform.position = _input.transform.position;
    }

    void SetAVGPositionToInput(GameObject _reference, GameObject _input1 , GameObject _input2)
    {
        Vector3 avgVector = (_input1.transform.position + _input2.transform.position) / 2;
        _reference.transform.position = avgVector;
    }

    void SetObjectAsParent(GameObject _parent, GameObject _child)
    {
        _child.transform.SetParent(_parent.transform, true); // sets the parent WHILE maintaining WORLD pos.
    }

    void DeleteObject(GameObject _objectToDelete)
    {
        DestroyImmediate(_objectToDelete);
    }

    void SetInputHandToReferernce()
    {
        Debug.Log("Clicked the button");
        //                        0          1        2       3        4        5        6       7      8
        //objectInputList -> {handParent ,geometry, torso, middle3, middle2, middle1, index3, index2, index1 };

        Undo.RegisterFullObjectHierarchyUndo(objectReferenceList[0], "Reference Object Hierachy"); 
        Undo.RegisterFullObjectHierarchyUndo(objectInputList[0], "Input Object Hierachy");

        SetAVGPositionToInput(objectReferenceList[0], objectInputList[6], objectInputList[3]); // set torso Reference to avg position based on index3 and middle 3
        SetObjectAsParent(objectReferenceList[2],  objectInputList[2]); // set torso Input as child of Torso Reference


        SetObjectAsParent(objectReferenceList[0], objectInputList[1]); // set Geo Input as Child of HandParent

        for (int i = 3; i < 8; i++) // hardcoded !!!! beware
        {
            SetPositionToInput(objectReferenceList[i], objectInputList[i]); // middle 3 - 1 ----- index 3 - 1
            SetObjectAsParent(objectReferenceList[i], objectInputList[i]);
        }

    }

    void DeleteOldObjects()
    {

        for (int i = 0; i < objectFieldListNames.Count; i++)
        {
            Undo.RecordObject(objectPreviousList[i], "Deleting Objects");
            DeleteObject(objectPreviousList[i]);
        }
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }
}
