using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NetworkDiscoveryTicTac : MonoBehaviour
{


    public NetworkDiscovery networkDiscovery;
    public GameManager gameManager;
    [SerializeField]
    Text textConnect;
    [SerializeField]
    Text textPlayer;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    const string disconnectText = "Disconnect";
    const string connectText = "Connect";
    const string startGameText = "Start game";
    private bool isConnected = false;

    private void Awake()
    {
        networkDiscovery.StartDiscovery();
    }

    private void Update()
    {
        if (!isConnected)
        {
            if (discoveredServers.Count == 0 && !textConnect.text.Equals(startGameText))
            {
                textConnect.text = startGameText;
            }
            else if (discoveredServers.Count > 0 && !textConnect.text.Equals(connectText))
            {
                textConnect.text = connectText;
            }
        }
    }

    public void OnClickConnect()
    {
        if (isConnected)
        {
            gameManager.OnDisconnected();
            discoveredServers.Clear();
            if (NetworkServer.active)
            {
                NetworkManager.singleton.StopHost();
            }
            else
            {
                NetworkManager.singleton.StopClient();
            }
            networkDiscovery.StartDiscovery();
            textConnect.text = startGameText;
            gameManager.textTurn.gameObject.SetActive(false);
            isConnected = false;
            return;
        }

        networkDiscovery.StopDiscovery();

        if (discoveredServers.Count == 0)
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
            textPlayer.text = "Player O";
            gameManager.textTurn.text = "Turn: Player O";
        }
        else
        {
            NetworkManager.singleton.StartClient(discoveredServers.Values.First().uri);
            textPlayer.text = "Player X";
            gameManager.textTurn.text = "Turn: Player X";
        }

        gameManager.InitGameBoard();
        textConnect.text = disconnectText;
        isConnected = true;
        gameManager.textTurn.gameObject.SetActive(true);
        gameManager.SetButtonInteractibility(true);
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;
    }
}
