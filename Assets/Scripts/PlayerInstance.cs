using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : NetworkBehaviour
{

     // WHAt references do I need here?
    public PlayerInputController inputController;
    public PlayerScore score;
    public PlayerFeedback feedback;
    public PlayerCustomization customization;
    // Start is called before the first frame update


	void Start()
    {
        if (isLocalPlayer && isServer)
        {
            // run is this is the player controlled by this computer
            FightManager.instance.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);

            Debug.Log("Local player joined");
        }
        else 
        {
            Debug.Log("Remote player joined");
            // run if this is a player controled by a different computer
        }

        FightManager.instance.AddPlayer(this);


        // run on all player regardles who controlls them

    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            // run is this is the player controlled by this computer
        }
        else
        {
            // run if this is a player controled by a different computer
        }

        // run on all player regardles who controlls them
    }

}
