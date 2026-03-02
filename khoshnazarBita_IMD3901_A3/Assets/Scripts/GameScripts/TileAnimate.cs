using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TileAnimate : NetworkBehaviour
{
    public float pressDistance = 0.3f;
    public float pressSpeed = 2f;

    Vector3 startPos;
    Vector3 targetPos;
    bool isPressed = false;
    bool isAnimating = false;

    private NetworkObject netObj_tile; //the tile network object

    void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos;
        netObj_tile = GetComponent<NetworkObject>(); //get the tiles network object component
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame) //if p was pressed
        {
            //isPressed = true;
            Debug.Log("p was presssed to press tile");

            if (IsOwner) //if the host pressed p just animate the tile, already has ownership
            {
                AnimateTile();
            }
           
            if (IsClient) //if the client pressed p, request the server for ownership, then animate the tile
            {
                PressTileServerRpc(netObj_tile.NetworkObjectId);
            }
        }

        //animate the tile up and down once its been pressed
        if (isAnimating && IsOwner)
        {
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

    public void AnimateTile()
    {
        isPressed = true;
        isAnimating = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void PressTileServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if (netObj_tile.TryGetComponent<NetworkObject>(out netObj_tile))
        {
            //transfer ownership so the client can interact with it (playerId here is SenderClientId)
            netObj_tile.ChangeOwnership(rpcParams.Receive.SenderClientId); //give client ownership access

            //animate the tile now that the client has ownership over it
            AnimateTile();
        }
    }

}
