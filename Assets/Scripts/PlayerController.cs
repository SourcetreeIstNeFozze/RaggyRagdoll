using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigid;
    public Vector3 restartForce;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ManagePlayerInput();
    }

    void ManagePlayerInput()
    {
        if (Input.GetButtonDown("A Button") || Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(restartForce);
            print("input force");
        }
    }
}
