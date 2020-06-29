using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ChangeHand : EditorWindow
{
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

    public static List<GameObject> objectFieldList;
    public static List<string> objectFieldListNames;


    [MenuItem("DevTools/Change Hand Model")]
    static void OpenWindow()
    {
        ChangeHand window = (ChangeHand)GetWindow(typeof(ChangeHand));
        window.minSize = new Vector2(200, 200);
        window.Show();

        objectFieldList = new List<GameObject>() { geometry, torso, middle3, middle2, middle1, index3, index2, index1 };
        objectFieldListNames = new List<string>() { "geometry", "torso", "middle3", "middle2", "middle1", "index3", "index2", "index1" };
        objectReferenceList = objectFieldList;
    }

    private void OnGUI()
    {
        ObjectFieldCreator(objectFieldList, objectFieldListNames);
        ObjectFieldCreator(objectReferenceList, objectFieldListNames);
    }

    public void ObjectFieldCreator(List<GameObject> _objectFieldList, List<string> _objectFieldNameList)
    {
        capacity = _objectFieldList.Count; //get the amount of fields that has to be drawn
                                                        //capacity = EditorGUILayout.IntField("Capacity", capacity); //VISUALIZE THE FIELD
        capacity = Mathf.Max(0, capacity);

        //add any new fields
        for (int m = _objectFieldList.Count; m < capacity; m++)
        {
            _objectFieldList.Add(null);
        }
            

        //remove extra fields
        for (int m = capacity; m < _objectFieldList.Count; m++)
        {
            _objectFieldList.RemoveAt(_objectFieldList.Count - 1);
        }

        //display the field
        for (int m = 0; m < _objectFieldList.Count; m++)
        {
            EditorGUILayout.BeginHorizontal();
            _objectFieldList[m] = (EditorGUILayout.ObjectField(_objectFieldNameList[m], _objectFieldList[m], typeof(GameObject), true, GUILayout.Width(500))) as GameObject;
            EditorGUILayout.EndHorizontal();
        }
    }


    Vector3 getPosition(GameObject _gO)
    {
        return _gO.transform.position;
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }
}
