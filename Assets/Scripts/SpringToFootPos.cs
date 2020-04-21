using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringToFootPos : MonoBehaviour
{
    public Transform indexFinger, middleFinger;
    public float connectedAnchorHeight = 6.4f;
    public float maxHandForce = 1f;

    Vector3 springPos;
    Rigidbody rigid;
    SpringJoint springJoint;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        springJoint = this.GetComponent<SpringJoint>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // get position between finger tips
        springPos = (indexFinger.position + middleFinger.position) / 2f;
        springPos.y = connectedAnchorHeight;

        // limit hand-force
        springPos = Vector3.MoveTowards(this.transform.TransformPoint(springJoint.anchor), springPos, maxHandForce);

        // assign
        springJoint.connectedAnchor = springPos;

        Debug.DrawLine(Vector3.zero, springJoint.anchor);
        Debug.DrawLine(this.transform.TransformPoint(springJoint.anchor), springPos, Color.green);
    }
}
