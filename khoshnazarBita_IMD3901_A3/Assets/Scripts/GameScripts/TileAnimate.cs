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
        //animate the tile up and down once its been pressed
        if (isAnimating)
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

    [ServerRpc(RequireOwnership = false)] //client requests to server to press the tile
    public void PressTileServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if (netObj_tile.TryGetComponent<NetworkObject>(out netObj_tile))
        {
            //transfer ownership so the client can interact with it (playerId here is SenderClientId)
            netObj_tile.ChangeOwnership(rpcParams.Receive.SenderClientId);

            //animate the tile now that the client has ownership over it
            AnimateTileClientRpc();
        }
    }

    [ClientRpc] //run the tile animation on the client
    private void AnimateTileClientRpc()
    {
        if (IsOwner)
        {
            AnimateTile();
            //Debug.Log("CLIENT RPC ANIMATE CALLED");
        }
    }

}
