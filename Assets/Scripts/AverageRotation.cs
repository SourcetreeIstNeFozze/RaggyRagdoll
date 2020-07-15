using Cinemachine;
using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageRotation : MonoBehaviour
{

    Cinemachine.CinemachineTargetGroup.Target[] targets;
    bool twoPlayers;

	FightManager fighmanager;
	
	bool rotateCamToPlayers { get { return !fighmanager.players[0].activeAvatar.isDown && !fighmanager.players[1].activeAvatar.isDown; } }

    public bool initializeAtStart = true;
    public bool initialized = false;
	CinemachineTargetGroup cameraTargetGoup;

    // Start is called before the first frame update
    void Start()
	{
		fighmanager = FindObjectOfType<FightManager>();

        if (initializeAtStart)
        {
            Initialize();

        }
	}

	public void Initialize()
	{
		cameraTargetGoup = this.GetComponent<Cinemachine.CinemachineTargetGroup>();
		targets = cameraTargetGoup.m_Targets;

		if (targets.Length == 2)
			twoPlayers = true;

        initialized = true;
        

    }

	// Update is called once per frame
	void Update()
    {
        if (initialized)
        {
			//adjust camera rotation
			if (twoPlayers && rotateCamToPlayers)
			{
				//get position of players

				Vector3 player1pos = targets[0].target.transform.position;
				Vector3 player2pos = targets[1].target.transform.position;

				//calc direction from players' mid point to where the camera should be

				Vector3 lineBetweenPlayers = player1pos - player2pos;
				Vector3 direction2camera = Vector3.Cross(lineBetweenPlayers, Vector3.up).normalized;

				//translate into angle
				Quaternion finalAngle = Quaternion.FromToRotation(Vector3.forward, direction2camera);
				this.transform.rotation = finalAngle;
			}

			//activate or deactivate camera positional follow
			if ( cameraTargetGoup.enabled != rotateCamToPlayers)
			{
				cameraTargetGoup.enabled = rotateCamToPlayers;
			}
		}
    }
}
