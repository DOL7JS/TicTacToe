using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTicTac : MonoBehaviour
{
    public EnumPlayerType playerType;

    public static List<PlayerTicTac> ActivePlayers = new List<PlayerTicTac>();
    protected virtual void Awake()
    {
        ActivePlayers.Add(this);
        Debug.Log("Player added");
        if (ActivePlayers.Count > 1)
        {
            ActivePlayers[0].playerType = EnumPlayerType.CIRCLE;
            ActivePlayers[1].playerType = EnumPlayerType.CROSS;
        }
    }
    protected virtual void OnDestroy()
    {
        ActivePlayers.Remove(this);
    }
}
