using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TileAnimate : NetworkBehaviour
{
    public float pressDistance = 0.3f;
    public float pressSpeed = 2f;
    Vector3 startPos;
    bool isPressed = false;
    bool isAnimating = false;

    private NetworkObject netObj_tile; //the tile network object

    void Start()
    {
        startPos = transform.localPosition;
        netObj_tile = GetComponent<NetworkObject>();
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame) //if p was pressed
        {
            Debug.Log("p was presssed to press tile");

            if (IsOwner)
            {
                AnimateTile();
                
                //animate the tile
                Vector3 target = isPressed ? startPos - Vector3.up * pressDistance : startPos;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, pressSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.localPosition, target) < 0.001f)
                {
                    if (isPressed)
                    {
                        isPressed = false;
                    }
                    else
                    {
                        isAnimating = false;
                    }
                }
            }

            if (IsClient) 
            {
                AnimateTile();
                PressTileServerRpc(netObj_tile.NetworkObjectId);
            }

        }
    }

    public void AnimateTile()
    {
        isPressed = true;
        isAnimating = true;
        Debug.Log("tile pressed");
    }

    [ServerRpc(RequireOwnership = false)]
    public void PressTileServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if (netObj_tile.TryGetComponent<NetworkObject>(out netObj_tile))
        {
            //transfer ownership so the client can interact with it (playerId here is SenderClientId)
            netObj_tile.ChangeOwnership(rpcParams.Receive.SenderClientId); //give client ownership access

            //animate the tile
            Vector3 target = isPressed ? startPos - Vector3.up * pressDistance : startPos;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, pressSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, target) < 0.001f)
            {
                if (isPressed)
                {
                    isPressed = false;
                }
                else
                {
                    isAnimating = false;
                }
            }
        }
    }

}
