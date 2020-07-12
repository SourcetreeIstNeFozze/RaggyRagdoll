using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles the network player logic in in game, while the player is in the lobby
//e.g. is the player ready to join? what's  the players name?
public class NetworkedPlayerInGame : NetworkBehaviour
{


    //void Start()
    //{
    //    if (isLocalPlayer && isServer)
    //    {
    //        // run is this is the player controlled by this computer
    //        Debug.Log("Local player joined");
    //    }
    //    else
    //    {
    //        Debug.Log("Remote player joined");
    //        // run if this is a player controled by a different computer
    //    }

    //    FightManager.instance.AddPlayer(GetComponent<PlayerInstance>());

    //    // run on all player regardles who controlls them

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (hasAuthority)
    //    {
    //        Debug.Log("cats", gameObject);
    //        // run is this is the player controlled by this computer
    //    }
    //    else
    //    {
    //        // run if this is a player controled by a different computer
    //    }

    //    // run on all player regardles who controlls them
    //}
}

