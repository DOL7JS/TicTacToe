using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnToggleStateChanged))]
    private bool isToggleOn = false;


    public void OnToggleValueChanged()
    {
        bool isOn = GetComponent<Toggle>().isOn;
        CmdToggleChanged(isOn);
    }

    [Command(requiresAuthority = false)]
    public void CmdToggleChanged(bool newState)
    {
        isToggleOn = newState;
    }

    private void OnToggleStateChanged(bool oldState, bool newState)
    {
        Toggle toggle = GetComponent<Toggle>();
        toggle.isOn = newState;
    }
}
