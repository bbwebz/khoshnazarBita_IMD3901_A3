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

    public AudioManager audioManager_access;

    void Start()
    {
        if (!IsOwner) return; // Only get the local player's hand

        //access the right hand rays
        rightHandRay = GetComponentInChildren<XRRayInteractor>();
    }


    void Update()
    {
        if (!IsOwner) return;

        if (Keyboard.current.tKey.wasPressedThisFrame) //for mock HMD
        {
            //check to see if the ray is colliding with something (like in pc)
            if (rightHandRay.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                //check if the object has the tag "Interactable"
                if (hit.collider.CompareTag("Interactable"))
                {
                    Debug.Log("Trigger pressed! Ray hit: " + hit.collider.name);

                    string tileName = hit.collider.gameObject.name;
                    //animate only the tile that was pressed/looked at
                    TileAnimate tile = hit.collider.GetComponent<TileAnimate>();

                    if (tile != null)
                    {
                        if (IsServer)
                        {
                            //if host, directly animate the tile
                            tile.AnimateTile();
                            //play the sound and keep track of which tile player1 played
                            audioManager_access.playTileSoundServerRpc(tileName, 0);
                        }
                        else if (IsClient)
                        {
                            //if client, request for ownership, animate the tile and synch
                            tile.PressTileServerRpc(tile.NetworkObjectId);
                            //play the sound and keep track of which tile player2 played
                            audioManager_access.playTileSoundServerRpc(tileName, 1);
                        }
                    }
                }
            }
        }
    }


    /*
    [ServerRpc(RequireOwnership = false)]
    void TriggerPressedServerRpc()
    {
        isTriggerPressed.Value = true; //t has been pressed by either host or client
        Debug.Log("Trigger Pressed on Server by Client " + OwnerClientId);

        //reset the boolean on the server
        isTriggerPressed.Value = false;
    }
    */

}