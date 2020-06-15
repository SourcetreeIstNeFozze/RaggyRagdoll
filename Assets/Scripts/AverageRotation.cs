using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageRotation : MonoBehaviour
{

    Cinemachine.CinemachineTargetGroup.Target[] targets;
    bool twoPlayers;

    // Start is called before the first frame update
    void Start()
    {
        targets = this.GetComponent<Cinemachine.CinemachineTargetGroup>().m_Targets;
        if (targets.Length == 2)
            twoPlayers = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (twoPlayers)
        {
            // get position of players
            Vector3 player1pos = targets[0].target.transform.position;
            Vector3 player2pos = targets[1].target.transform.position;

            // calc direction from players' mid point to where the camera should be
            Vector3 lineBetweenPlayers = player1pos - player2pos;
            Vector3 direction2camera = Vector3.Cross(lineBetweenPlayers, Vector3.up).normalized;

            // translate into angle
            Quaternion finalAngle = Quaternion.FromToRotation(Vector3.forward, direction2camera);
            this.transform.rotation = finalAngle;
        }
    }
}
