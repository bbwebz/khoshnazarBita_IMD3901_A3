using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRControllerInputNet : NetworkBehaviour
{
    //use a network variable boolean so that host and client can synch the states of the variable
    public NetworkVariable<bool> isTriggerPressed = new NetworkVariable<bool>(false);

    void Update()
    {
        if (!IsOwner) return;

        bool triggerPressed = false;

        //for mock HMD
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            triggerPressed = true;
        }
        //send request to server to update the network variable (bool)
        if (triggerPressed && !isTriggerPressed.Value)
        {
            TriggerPressedServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void TriggerPressedServerRpc()
    {
        isTriggerPressed.Value = true; //t has been pressed by either host or client
        Debug.Log("Trigger Pressed on Server by Client " + OwnerClientId);

        //reset the boolean on the server
        isTriggerPressed.Value = false;
    }

}