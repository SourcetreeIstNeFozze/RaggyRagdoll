﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class IKGoal
{
    public GameObject goal;
    public bool left;


    public Vector3 startPosition;
    //[Range(0.0f, 2f)]
    //public float depthAxis;
}



public class IKController : MonoBehaviour
{
    public List<IKGoal> IKGoals;

    [SerializeField]
    public InputController inputC;

    void Awake()
    {
        for (int i = 0; i < IKGoals.Count; i++)
        {
            IKGoals[i].startPosition = IKGoals[i].goal.transform.position;
        }
    }

    void MoveRightGoal(Vector2 direction)
    {
        print("SOMETHING");
        IKGoals[0].goal.transform.position = direction;
    }

    public void OnIKControllLEFT(InputValue value)
    {
        Vector3 newPosition = new Vector3(IKGoals[0].startPosition.x + value.Get<Vector2>().x, IKGoals[0].startPosition.y + value.Get<Vector2>().y, IKGoals[1].startPosition.z);
        IKGoals[0].goal.transform.position = newPosition;
    }

    public void OnIKControllRIGHT(InputValue value)
    {
        Vector3 newPosition = new Vector3 (IKGoals[1].startPosition.x + value.Get<Vector2>().x, IKGoals[1].startPosition.y + value.Get<Vector2>().y, IKGoals[1].startPosition.z);
        IKGoals[1].goal.transform.position = newPosition;
    }

}