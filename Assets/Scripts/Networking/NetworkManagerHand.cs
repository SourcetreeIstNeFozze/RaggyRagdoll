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
	[Scene] private string lastScene;

	[Header("Prefabs")]
	public NetworkedPlayerInGame NETPlayerInGame_prefab;
	public NetworkPlayerLobby NETPlayerLobby_prefab;
	public GameObject fightManager_prefab;


	public static System.Action OnClientConnected;
	public static System.Action OnClientDisconnected;
	public static System.Action<NetworkConnection> OnServerReadied; // Called ON SERVER when a CLIENT has readied, ot when the server is readied!!!

	public bool debugs; 
	public override void OnClientConnect(NetworkConnection conn)
	{
		if (debugs) Debug.Log("Connecting Client...");
		base.OnClientConnect(conn);

		OnClientConnected?.Invoke();
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		if (debugs)  Debug.Log("Disconnecting Client");
		base.OnClientConnect(conn);

		OnClientDisconnected?.Invoke();
	}


	public override void OnServerConnect(NetworkConnection conn)
	{
		if (debugs)  Debug.Log("Connecting Server...");
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
		if (SceneManager.GetActiveScene().path == mainMenuScene)
		{
			if (debugs) Debug.Log("Adding player in MainMenu ...");
			NetworkPlayerLobby networkPlayerLobby = Instantiate(NETPlayerLobby_prefab);
			NetworkServer.AddPlayerForConnection(conn, networkPlayerLobby.gameObject);
		}
	}

	public override void OnStopServer()
	{
		//lobbyPlayers.Clear();
		base.OnStopServer();

	}

	
	public void StartGame()
	{
		if (SceneManager.GetActiveScene().path == mainMenuScene)
		{
			if (debugs)  Debug.Log("Starting Level...");
			// TO DO?:
			//- check if all the players are ready

			lastScene = SceneManager.GetActiveScene().name;
			ServerChangeScene(gameScene);
		}
		
	}

	public override void ServerChangeScene(string newSceneName)
	{
		if (debugs)  Debug.Log("Changing Scene on Server....");

		lastScene = SceneManager.GetActiveScene().path;

		base.ServerChangeScene(newSceneName);
	}
	public override void OnServerSceneChanged(string sceneName)
	{
		if (debugs)  Debug.Log("Changed Scene on Server....");

		base.OnServerSceneChanged(sceneName);
				
		//ON GAME SCENE LOADED
		if (SceneManager.GetActiveScene().path == gameScene)
		{
			//menu to game
			if (lastScene == mainMenuScene)
			{
				if (debugs) Debug.Log("Reinstantiating players..");

				//replace lobby players with the game players
				for (int i = 0; i < lobbyPlayers.Count; i++)	
				{
					if (debugs) Debug.Log("Reinstantiating playr:" + i);
					NetworkConnection conn = lobbyPlayers[i].connectionToClient;
					NetworkedPlayerInGame playerInstance = Instantiate(NETPlayerInGame_prefab);

					NetworkServer.Destroy(conn.identity.gameObject); // actually destroys the lobbyPlayer object
					NetworkServer.ReplacePlayerForConnection(conn, playerInstance.gameObject); // bind the connection of the destroyed object with the newly instantiated object
				}

				//lobbyPlayers.Clear();
			}
			//game to game 
			else
			{
				//TO BE IMPELMENTED
			}

			//if (debugs) Debug.Log("Spawning Fight Manager playrs..");
			////Spawn fight manager
			//GameObject fightManager = GameObject.Instantiate(fightManager_prefab);
			//NetworkServer.Spawn(fightManager); // if no second argument is given than the server has authority over this object

		}
	}

	public override void OnServerReady(NetworkConnection conn)
	{
		//if (debugs) Debug.Log( conn.identity.gameObject.name + " is ready", conn.identity.gameObject); 
		base.OnServerReady(conn);
		OnServerReadied?.Invoke(conn);
	}

}
