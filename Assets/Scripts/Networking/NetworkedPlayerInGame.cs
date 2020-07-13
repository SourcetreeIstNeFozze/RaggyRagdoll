using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles the network player logic in in game, while the player is in the lobby
//e.g. is the player ready to join? what's  the players name?
public class NetworkedPlayerInGame : NetworkBehaviour
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

	public override void OnStartClient()
	{
		DontDestroyOnLoad(gameObject);
		NetworkManagerHand.gamePlayers.Add(this);
	}

	public override void OnStopClient()
	{
		NetworkManagerHand.gamePlayers.Remove(this);
		base.OnStopClient();
	}
}


