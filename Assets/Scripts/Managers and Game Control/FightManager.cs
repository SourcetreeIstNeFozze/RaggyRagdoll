using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : NetworkBehaviour
{
    public static FightManager instance;
    // Active Players
    public List<PlayerInstance> players;
    public bool localMultiplayer;

    public GameObject playerPrefab;
    public float initialPlayerOffSet;

	private void Awake()
	{
        instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(PlayerInstance player) 
    {
        players.Add(player);

        if (players.Count == 2) 
        {
            OnBothPlayersJoined();
        }
    }



    [Command]
    public void CmdSpawnPlayerAvatar(int side)
    {
        GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(playerPrefab.transform.position.x, playerPrefab.transform.position.y, initialPlayerOffSet * side), Quaternion.Euler(0, -180 * side, 0) );

        LoadPlayerCustomisation();

        newPlayer.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);

        NetworkServer.Spawn(newPlayer);
    }


    private void LoadPlayerCustomisation()
	{
        Debug.Log("here some customisation should be applied");
	}

    private void OnBothPlayersJoined() 
    {
        CmdSpawnPlayerAvatar(1);
        CmdSpawnPlayerAvatar(-1);
    }
}
