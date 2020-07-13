using Mirror;
using Mirror.Examples.Basic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;
    public bool localMultiplayer = true;

    [Header("Spawning players")]
    public GameObject playerPrefab;
    public float initialPlayerOffSet;
    private int playerIndex = 0;
    public List<PlayerInstance> players = new List<PlayerInstance>();
    public List<GameObject> playerAvatars = new List<GameObject>();
    public bool debugs;

	public void Awake()
	{
        //if local multiplayer, immediately spawn two players
        if (localMultiplayer) 
        {
            // if not enough avatars, at start, spawn some
            while (playerAvatars.Count < 2) 
            {
                playerAvatars.Add(SpawnPlayerAvatar());
            }

			// turn into player instances
			for (int i = 0; i < playerAvatars.Count; i++)
			{
                PlayerInstance playerInstance = GeneratePlayerInstancFromGameObject(playerAvatars[i]);
                players.Add(playerInstance);
            }

            OnBothPlayersJoined();
            
         }
	}


    public GameObject SpawnPlayerAvatar()
	{
		if (debugs) Debug.Log("Spawning Avatar...");

		// is this left player or right player?
		int side;
		if (playerIndex == 0)
			side = 1;
		else
			side = -1;

		//spawn player
		GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(playerPrefab.transform.position.x, playerPrefab.transform.position.y, initialPlayerOffSet * side), Quaternion.Euler(0, -90 * side, 0));
		
		playerIndex++;

		return newPlayer;
	}

	private static PlayerInstance GeneratePlayerInstancFromGameObject(GameObject newPlayer)
	{
		PlayerInstance newPlayerReference = newPlayer.GetComponent<PlayerInstance>();

		newPlayerReference.inputController = newPlayer.GetComponent<PlayerInputController>();
		newPlayerReference.feedback = newPlayer.GetComponent<PlayerFeedback>();
		newPlayerReference.activeAvatar = newPlayer.GetComponentInChildren<HandReferences>();

        newPlayerReference.activeAvatar.GetReferences();

		return newPlayerReference;
	}

	private void LoadPlayerCustomisation()
    {
        if (debugs) Debug.Log("Here some customisation should be applied");
    }

    private void OnBothPlayersJoined()
    {
        if (debugs) Debug.Log("Both Players Have Joined...");

        SetCrossReferencess(players[0], players[1]);
        SetCrossReferencess(players[1], players[0]);

        players[0].inputController.Initialize();
        players[1].inputController.Initialize();

        //Set up camaera
        PrepCamera();
    }

    private void PrepCamera()
    {
        Debug.Log("preparing camera");
        Cinemachine.CinemachineTargetGroup targetGroup = FindObjectOfType<Cinemachine.CinemachineTargetGroup>();
        for (int i = 0; i < players.Count; i++)
        {
            targetGroup.m_Targets[i].target = players[i].activeAvatar.transform;
        }

        AverageRotation averageRotation = FindObjectOfType<AverageRotation>();
        averageRotation.Initialize();
    }

    private void SetCrossReferencess(PlayerInstance player1, PlayerInstance player2)
    {
        player1.inputController.otherPlayer = player2;
        player1.activeAvatar.balance.lookAtTarget = player2.activeAvatar.gameObject;

    }


}
