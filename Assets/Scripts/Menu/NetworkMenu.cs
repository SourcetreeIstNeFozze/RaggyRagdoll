using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkMenu : MonoBehaviour
{
    public NetworkManagerHand networkManager;

    [Header("UI Elements")]
    public TMP_InputField IPInpuField;
    public TextMeshProUGUI hostIP;
    public Button joinLobbyButton;


    
	private void Awake()
	{
	}
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public void OnEnable()
	{
        NetworkManagerHand.OnClientConnected += HandlePlayerConnected;
        NetworkManagerHand.OnClientDisconnected += HandlePlayerDisconnected;
    }

	public void OnDisable()
    {
        NetworkManagerHand.OnClientConnected -= HandlePlayerConnected;
        NetworkManagerHand.OnClientDisconnected -= HandlePlayerDisconnected;

    }

	public void HostLobby() 
    {
        networkManager.networkAddress = IPManager.GetIP(ADDRESSFAM.IPv4);
        hostIP.text = networkManager.networkAddress;
        Debug.Log(networkManager.networkAddress);
        networkManager.StartHost();
       
    }

    public void JoinLobby() 
    {
        //try connecting to the given IP
        string IPAddress = IPInpuField.text;
        networkManager.networkAddress = IPAddress;
        networkManager.StartClient();

        //deactivate button to prevent spamming;
        joinLobbyButton.interactable = false;
    }

    public void HandlePlayerConnected() 
    { 

    }

    public void HandlePlayerDisconnected() 
    {
        //reactivate button in case the player fails to connect
        joinLobbyButton.interactable = true;

    }
}
