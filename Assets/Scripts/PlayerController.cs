using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigid;
    public Vector3 restartForce;
    SpringToFootPos stabilizeScript;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        stabilizeScript = this.GetComponent<SpringToFootPos>();
    }

    // Update is called once per frame
    void Update()
    {
        ManagePlayerInput();
    }

    void ManagePlayerInput()
    {
        // Stand Up
        if (Input.GetButtonDown("A Button") || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StandUp());
        }
    }

    IEnumerator StandUp()
    {
        bool standUp = true;
        bool standUp_stabilize = false;

        // 1) handfläche darf höchstens zur hälfte aufgerichtet sein, force nach oben adden
        while (standUp)
        {
            rigid.AddForce(restartForce);
            //print("phase 1");
            
            if (rigid.position.y > (stabilizeScript.connectedAnchorHeight * 0.9f))
            {
                standUp = false;
                standUp_stabilize = true;
            }
            yield return new WaitForFixedUpdate();
        }
        
        // 2) handfläche ist mind. zur hälfte aufgerichtet, jetzt velocity bremsen
        while (standUp_stabilize)
        {
            rigid.velocity /= 1.08f;
            //print("phase 2");
            if (rigid.velocity.magnitude < 0.2f)
            {
                standUp_stabilize = false;
            }
            yield return new WaitForFixedUpdate();
        }
        
        yield return null;
    }
}
