using Mirror;
using Mirror.Examples.Basic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManagerNetworked : NetworkBehaviour
{
    public static FightManagerNetworked instance;
    public bool localMultiplayer;

    [Header("Spawning players")]
    public GameObject playerPrefab;
    public float initialPlayerOffSet;
    private int playerIndex = 0;
    public List<PlayerInstance> players;
    
    //SPAWING PLAYERS AND SCENE SET UP
    public bool debugs = true;
	public override void OnStartServer() // this is the equivalent of the Start() for networked objects
    {
        base.OnStartServer();
        instance = this;
        NetworkManagerHand.OnServerReadied += AddPlayer;
    }

	private void OnDestroy() //not sure if I need this
	{
        NetworkManagerHand.OnServerReadied -= AddPlayer;
    }


	public void AddPlayer(NetworkConnection conn)
	{
        players.Add(SpawnPlayerAvatar(conn));

		if (players.Count == 2)
		{
			OnBothPlayersJoined();
		}
	}


	public PlayerInstance SpawnPlayerAvatar(NetworkConnection conn)
    {
        if (debugs) Debug.Log("Spawning Avatar");

        // is this left player or right player?
        int side;
        if (playerIndex == 0) 
            side = 1;
        else
            side = -1;

        //spawn player
        GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(playerPrefab.transform.position.x, playerPrefab.transform.position.y, initialPlayerOffSet * side), Quaternion.Euler(0, -180 * side, 0) );
        PlayerInstance newPlayerReference = newPlayer.GetComponent<PlayerInstance>();

        //assign references
        newPlayerReference.inputController = newPlayer.GetComponent<PlayerInputController>();
        newPlayerReference.feedback = newPlayer.GetComponent<PlayerFeedback>();
        newPlayerReference.activeAvatar = newPlayer.GetComponentInChildren<HandReferences>();
        
        //load customisation??????
        LoadPlayerCustomisation();

        //spawn player on server
        if (conn != null)
        {
            NetworkServer.Spawn(newPlayer, conn); // the second argument determines who has the authority over the object
        }

        playerIndex ++;

        return newPlayerReference;
    }


    private void LoadPlayerCustomisation()
	{
        if(debugs) Debug.Log("Here some customisation should be applied");
	}

    private void OnBothPlayersJoined() 
    {
        if (debugs) Debug.Log("BothPlayersHaveJoined");

        //SetUp Players
        players[0].activeAvatar.GetReferences();
        players[1].activeAvatar.GetReferences();

       
        SetCrossReferencess(players[0], players[1]);
        SetCrossReferencess(players[1], players[0]);

        players[0].inputController.Initialize();
        players[1].inputController.Initialize();


        //Set up camaera
        PrepCamera();
    }

	private void PrepCamera()
	{
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
