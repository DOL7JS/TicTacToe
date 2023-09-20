using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : NetworkBehaviour
{
    public int row;
    public int column;

    [SyncVar(hook = nameof(OnToggleStateChanged))]
    public int value;

    public Box[,] gameboard;

    public void OnBoxValueChanged()
    {
        CmdToggleChanged(value);
    }

    [Command(requiresAuthority = false)]
    public void CmdToggleChanged(int newState)
    {
        value = newState;
    }

    private void OnToggleStateChanged(int oldState, int newState)
    {
        if (newState == -1)
        {
            gameObject.transform.GetComponent<Image>().sprite = Images.BoxClear;
            return;
        }
        value = newState;
        gameObject.transform.GetComponent<Image>().sprite = value == 1 ? Images.BoxCross : Images.BoxCircle;
    }

}
