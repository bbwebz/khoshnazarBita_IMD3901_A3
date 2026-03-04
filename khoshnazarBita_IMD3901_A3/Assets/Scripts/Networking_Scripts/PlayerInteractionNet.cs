using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class PlayerInteractionNet : NetworkBehaviour
{
    public float interactRange = 5f;
    public Camera playerCamera;

    public Crosshair crosshair_access;
    public PickupController pickupController_access;
    public AudioManager audioManager_access;
    public XRRayInteractor rightHandRay;

    void Update()
    {
        //check to see if its the HOST
        if (!IsOwner)
        {
            return;
        }

        //--------------------------------------------------------------------------------------------
        //VR input with right hand ray
        //var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        //UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);

        /*if (rightHandDevices.Count == 1)
        {
            UnityEngine.XR.InputDevice device = rightHandDevices[0];

            bool triggerValue;
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                Debug.Log("Trigger button is pressed.");
            }
        }*/
        //----------------------------------------------------------------------------


        //creating a ray that shoots out of the camera to detect objects in front of us
        //we want the ray to be in front of us
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit; //hit box for the ray cast

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                //checking if the ray hits something with a collider that is interactable
                crosshair_access.setInteractServerRpc(true);

                //PC INPUT with crosshair ray
                if (Keyboard.current.iKey.wasPressedThisFrame)
                {
                    if (IsHost)
                    {
                        Debug.Log("HOST pressed I");
                    }
                    else if (IsClient)
                    {
                        Debug.Log("CLIENT pressed I");
                        sendHoldRequestToServerRpc();
                    }
                }
                
                if (Keyboard.current.pKey.wasPressedThisFrame)
                {
                    Debug.Log("tile pressed was: " + hit.collider.gameObject.name);
                    string tileName = hit.collider.gameObject.name;
                    
                    //animate only the tile that was pressed/looked at
                    TileAnimate tile = hit.collider.GetComponent<TileAnimate>();

                    if (tile != null)
                    {
                        if (IsHost)
                        {
                            //if host, directly animate the tile
                            tile.AnimateTile();
                            //play the sound and keep track of which tile player1 played
                            audioManager_access.playTileSound(tileName, 0);
                        }
                        
                        if (IsClient)
                        {
                            //if client, request for ownership, animate the tile and synch
                            tile.PressTileServerRpc(tile.NetworkObjectId);
                            //play the sound and keep track of which tile player2 played
                            audioManager_access.playTileSound(tileName, 1);
                        }
                    }

                }
                

                //Debug.Log("interact was set to true");
                return;
            }
        }
        crosshair_access.setInteractServerRpc(false); //set it back to false if we look away from the object
    }

    //when server, debug appeared on HOST console
    //when owner, debug appeared on CLIENT console
    //when ClientsAndHost, debug appeared on BOTH consoles
    [Rpc(SendTo.Server)]
    public void sendHoldRequestToServerRpc()
    {
        Debug.Log("request to SERVER sent");
    }
}
