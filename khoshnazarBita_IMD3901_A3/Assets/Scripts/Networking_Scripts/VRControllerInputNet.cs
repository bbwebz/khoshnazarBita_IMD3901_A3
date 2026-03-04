using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRControllerInputNet : NetworkBehaviour
{
    //use a network variable boolean so that host and client can synch the states of the variable
    public NetworkVariable<bool> isTriggerPressed = new NetworkVariable<bool>(false);

    //access the right hand ray interactors of both host and client 
    private XRRayInteractor rightHandRay;

    void Start()
    {
        if (!IsOwner) return; // Only get the local player's hand

        //access the right hand rays
        rightHandRay = GetComponentInChildren<XRRayInteractor>();
    }

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

        if (triggerPressed)
        {
            //check to see if the ray is colliding with something (like in pc)
            if (rightHandRay.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                // Check if the object has the tag "Interactable"
                if (hit.collider.CompareTag("Interactable"))
                {
                    Debug.Log($"Trigger pressed! Ray hit interactable: {hit.collider.name}");
                    

                    //AUDIO



                }
            }
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