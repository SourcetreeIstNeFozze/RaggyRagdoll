using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balance : MonoBehaviour
{
    public GameObject body;
    void Update()
    {
        transform.position = body.transform.position;
    }
}
