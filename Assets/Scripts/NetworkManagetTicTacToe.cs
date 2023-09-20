using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Pong
{
    // Custom NetworkManager that simply assigns the correct racket positions when
    // spawning players. The built in RoundRobin spawn method wouldn't work after
    // someone reconnects (both players would be on the same side).
    [AddComponentMenu("")]
    public class NetworkManagetTicTacToe : NetworkManager
    {
        public GameManager gameManager;
        public GameObject playground;
     
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
            gameManager.textTurn.gameObject.SetActive(false);
        }
        private void OnDisconnectedFromServer()
        {
            Debug.Log("DISCONNECTED");
        }
    }
}