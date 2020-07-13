using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : NetworkBehaviour
{
    public static FightManager instance;

    [Header("Spawning players")]
    public GameObject playerPrefab;
    public float initialPlayerOffSet;
    private int playerIndex = 0;
    public List<GameObject> players;
    public bool localMultiplayer;

	//SPAWING PLAYERS AND SCENE SET UP

	public override void OnStartServer() // this is the equivalent of the Start() for networked objects
    {
        base.OnStartServer();
        instance = this;
        NetworkManagerHand.OnServerReadied += SpawnPlayerAvatar;
    }

	private void OnDestroy() //not sure if I need this
	{
        NetworkManagerHand.OnServerReadied -= SpawnPlayerAvatar;
    }


	//public void AddPlayer(PlayerInstance player) 
 //   {
 //       players.Add(player);

 //       if (players.Count == 2) 
 //       {
 //           OnBothPlayersJoined();
 //       }
 //   }


    public void SpawnPlayerAvatar(NetworkConnection conn)
    {
        
        // is this left player or right player?
        int side;
        if (playerIndex == 0) 
            side = 1;
        else
            side = -1;

        //spawn player
        GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(playerPrefab.transform.position.x, playerPrefab.transform.position.y, initialPlayerOffSet * side), Quaternion.Euler(0, -180 * side, 0) );
       
        ////assign references
        //players[playerIndex].inputController = newPlayer.GetComponent<PlayerInputController>();
        //players[playerIndex].feedback = newPlayer.GetComponent <PlayerFeedback>();
        //players[playerIndex].activeAvatar = newPlayer.GetComponentInChildren<HandReferences>();

        //spawn player on server
        NetworkServer.Spawn(newPlayer, conn); // the second argument determines who has the authority over the object
        players.Add(newPlayer);
       
        //load customisation??????
        LoadPlayerCustomisation();

        playerIndex ++;
    }


    private void LoadPlayerCustomisation()
	{
        Debug.Log("Here some customisation should be applied");
	}

    private void OnBothPlayersJoined() 
    {
        //SpawnPlayerAvatar(1);
        //SpawnPlayerAvatar(-1);

        //SetCamera();
    }

	//private void PrepCamera()
	//{
 //       Cinemachine.CinemachineTargetGroup targetGroup = FindObjectOfType<Cinemachine.CinemachineTargetGroup>();
	//	for (int i = 0; i < players.Count; i++)
	//	{
 //           targetGroup.m_Targets[i].target = players[i].activeAvatar.transform;
 //       }

 //       AverageRotation averageRotation = FindObjectOfType<AverageRotation>();
 //       averageRotation.Initialize();
 //   }
}
