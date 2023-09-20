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

    //[SyncVar(hook = nameof(OnHostStop))]
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
            gameManager.OnClickButtonReset();
            discoveredServers.Clear();
            NetworkManager.singleton.StopHost();
            networkDiscovery.StartDiscovery();
            textConnect.text = connectText;
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
            Debug.Log("Making HOST");
        }
        else
        {
            NetworkManager.singleton.StartClient(discoveredServers.Values.First().uri);
            Debug.Log("Making CLIENT");
            textPlayer.text = "Player X";

        }
        textConnect.text = disconnectText;
        isConnected = true;
        gameManager.textTurn.gameObject.SetActive(true);
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;
    }
}
