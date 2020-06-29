using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ChangeHandModelWindow : EditorWindow
{
    public GameObject handModelPrefab;

    GameObject childGameObjects;
    public List<GameObject> childObjects = new List<GameObject>();
    //GameObject replacementGameObject;

    private int capacity;

    [MenuItem("DevTools/Change Hand Model")]
    static void OpenWindow()
    {
        ChangeHandModelWindow window = (ChangeHandModelWindow)GetWindow(typeof(ChangeHandModelWindow));
        window.minSize = new Vector2(200, 200);
        window.Show();
    }

    private void OnGUI()
    {
        handModelPrefab = (GameObject)EditorGUILayout.ObjectField("Hand Model to Copy From", handModelPrefab, typeof(GameObject));
        childObjectLoader();

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Copy Components"))
        {
            CopyComponents();
        }
        if (GUILayout.Button("Copy Values"))
        {
            CopyValues();
        }

        EditorGUILayout.EndHorizontal();
    }

    public void CopyComponents()
    {
        Debug.Log("copied all non existing components");

        List<GameObject> ObjectsToCopyFrom = new List<GameObject>();
        if(handModelPrefab != null)
            ObjectsToCopyFrom = GetAllChilds(handModelPrefab);


        for (int i = 0; i < ObjectsToCopyFrom.Count; i++)
        {
            foreach (Component component in ObjectsToCopyFrom[i].GetComponents(typeof(Component)))
            {
                if(childObjects[i]!= null)
                { 
                   UnityEditorInternal.ComponentUtility.CopyComponent(component);
                   UnityEditorInternal.ComponentUtility.PasteComponentAsNew(childObjects[i]);
                }
            } /// INSERT !!! if Object is null -> Create Object in hierachy, name it correctly and copy components
        }
    }

    public void CopyValues()
    {
        // make a setting to override Values
    }

    public void childObjectLoader()
    {
        if(handModelPrefab != null)
        {
            List<GameObject> childNamingObjects = GetAllChilds(handModelPrefab);

            capacity = GetAllChilds(handModelPrefab).Count; //get the amount of fields that has to be drawn
            //capacity = EditorGUILayout.IntField("Capacity", capacity); //VISUALIZE THE FIELD
            capacity = Mathf.Max(0, capacity);

            //add any new fields
            for (int m = childObjects.Count; m < capacity; m++)
                childObjects.Add(null);

            //remove extra fields
            for (int m = capacity; m < childObjects.Count; m++)
                childObjects.RemoveAt(childObjects.Count - 1);

            for (int m = 0; m < childObjects.Count; m++)
            {
                //display the field
                childObjects[m] = (EditorGUILayout.ObjectField(childNamingObjects[m].transform.name , childObjects[m], typeof(GameObject), true, GUILayout.Width(500))) as GameObject;

            }
        }
    }


    private List<GameObject> GetAllChilds(GameObject root)
    {
        List<GameObject> result = new List<GameObject>();
        if(root != null) { 
            if (root.transform.childCount > 0)
            {
                foreach (Transform VARIABLE in root.transform)
                {
                    ChildSearcher(result, VARIABLE.gameObject);
                }
            }
        }
        return result;
    }

    private void ChildSearcher(List<GameObject> list, GameObject root)
    {
        list.Add(root);
        if (root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in root.transform)
            {
                ChildSearcher(list, VARIABLE.gameObject);
            }
        }
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

}


