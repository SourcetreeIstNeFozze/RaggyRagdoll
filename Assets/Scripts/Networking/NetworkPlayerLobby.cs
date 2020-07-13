using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles the player logic, while the player is in the lobby
//e.g. is the player ready to join? what's  the players name?
public class NetworkPlayerLobby : NetworkBehaviour
{
	private NetworkManagerHand networkManagerHand;
	private NetworkManagerHand NetworkManagerHand
	{
		get
		{
			if (networkManagerHand == null)
				networkManagerHand = NetworkManager.singleton as NetworkManagerHand;

			return networkManagerHand;
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdStartGame();
		}
	}
	public override void OnStartAuthority()
	{
		base.OnStartAuthority();
	}

	public override void OnStartClient()
	{
		//base.OnStartClient();

		NetworkManagerHand.lobbyPlayers.Add(this);

		if (NetworkManagerHand.lobbyPlayers.Count == 2)
		{
			CmdStartGame();
		}
	}

	[Command]
	public void CmdStartGame() 
	{
		NetworkManagerHand.StartGame();
	}
}
