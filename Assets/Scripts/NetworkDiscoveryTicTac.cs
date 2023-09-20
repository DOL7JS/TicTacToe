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
    [SerializeField]
    Text textConnect;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

    private void Awake()
    {
        networkDiscovery.StartDiscovery();
    }

    private void Update()
    {
        if (discoveredServers.Count == 0 && !textConnect.text.Equals("Start game"))
        {
            textConnect.text = "Start game";
        }
        else if (discoveredServers.Count > 0 && !textConnect.text.Equals("Connect"))
        {
            textConnect.text = "Connect";
        }
    }

    public void OnClickConnect()
    {

        if (discoveredServers.Count == 0)
        {
            networkDiscovery.StopDiscovery();
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
            Debug.Log("Making HOST");
        }
        else
        {
            networkDiscovery.StopDiscovery();
            NetworkManager.singleton.StartClient(discoveredServers.Values.First().uri);
            Debug.Log("Making CLIENT");

        }
    }
    public void OnDiscoveredServer(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;
    }
}
