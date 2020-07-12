using Mirror;
using Mirror.Websocket;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerHand : NetworkManager
{
	[Header("Player Tracking")]
	private int curPlayersCount;
	public List<NetworkPlayerLobby> lobbyPlayers = new List<NetworkPlayerLobby>();
	public List<NetworkedPlayerInGame> gamePlayers = new List<NetworkedPlayerInGame>();

	[Header("Scenes")]

	[Scene] public string mainMenuScene;
	[Scene] public string gameScene;
	[Scene] public string lastScene;

	[Header("Prefabs")]
	public NetworkedPlayerInGame NETPlayerInGame_prefab;
	public NetworkPlayerLobby NETPlayerLobby_prefab;
	public GameObject fightManager_prefab;


	public static System.Action OnClientConnected;
	public static System.Action OnClientDisconnected;

	public override void OnClientConnect(NetworkConnection conn)
	{
		Debug.Log("Connecting Client...");
		base.OnClientConnect(conn);

		OnClientConnected?.Invoke();
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		Debug.Log("Disconnecting Client");
		base.OnClientConnect(conn);

		OnClientDisconnected?.Invoke();
	}


	public override void OnServerConnect(NetworkConnection conn)
	{
		Debug.Log("Connecting Server...");
		//base.OnServerConnect(conn);

		//disconnect (== dont allow to jain) a player if max number of players has been reached
		if (curPlayersCount >= maxConnections) 
		{
			conn.Disconnect();
		}

		//if the current active scene ON THE SERVER is not the mainMenu, ie. the game has already started, disconnect
		if (SceneManager.GetActiveScene().path != mainMenuScene)
		{
			conn.Disconnect();
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		Debug.Log("Adding player...");
		if (SceneManager.GetActiveScene().path == mainMenuScene)
		{
			NetworkPlayerLobby networkPlayerLobby = Instantiate(NETPlayerLobby_prefab);
			NetworkServer.AddPlayerForConnection(conn, networkPlayerLobby.gameObject);
		}
	}

	public void StartGame()
	{
		if (SceneManager.GetActiveScene().path == mainMenuScene)
		{
			// TO DO?:
			//- check if all the players are ready

			lastScene = SceneManager.GetActiveScene().name;
			ServerChangeScene(gameScene);
		}
		
	}

	public override void OnServerSceneChanged(string sceneName)
	{
		base.OnServerSceneChanged(sceneName);

		//menu to game
		if (SceneManager.GetActiveScene().path == mainMenuScene)
		{
			//replace lobby players with the game players
			for (int i = 0; i < lobbyPlayers.Count; i++)
			{
				NetworkConnection conn = lobbyPlayers[i].connectionToClient;
				NetworkedPlayerInGame playerInstance = Instantiate(NETPlayerInGame_prefab);

				NetworkServer.Destroy(conn.identity.gameObject); // actually destroys the lobbyPlayer
				NetworkServer.ReplacePlayerForConnection(conn, playerInstance.gameObject);
			}
		}
	}

}
