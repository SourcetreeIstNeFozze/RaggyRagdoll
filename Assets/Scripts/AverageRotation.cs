using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageRotation : MonoBehaviour
{
    Cinemachine.CinemachineTargetGroup.Target[] targets;
    Vector3 averageRotation;
    float i;

    // Start is called before the first frame update
    void Start()
    {
        targets = this.GetComponent<Cinemachine.CinemachineTargetGroup>().m_Targets;
    }

    // Update is called once per frame
    void Update()
    {
        averageRotation = new Vector3();
        i = 0;
        foreach(Cinemachine.CinemachineTargetGroup.Target target in targets)
        {
            averageRotation += target.target.eulerAngles;
            i++;
        }
        averageRotation = averageRotation / i;
        averageRotation = new Vector3(0, averageRotation.y, 0);
        print("aveRot: " + averageRotation);

        this.transform.localEulerAngles = averageRotation;
    }
}
