using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupController : NetworkBehaviour
{
    [SerializeField] Transform holdArea; //will be parented to this

    //the object that is picked up
    private GameObject heldObj;
    public GameObject HeldObject => heldObj;
    private Rigidbody heldObjRB;

    //physics
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private void Update()
    {
        //PICKING UP-----------------------------
        if (Keyboard.current.iKey.wasPressedThisFrame) //if i was pressed to pick up
        {
            Debug.Log("i was presssed to pickup object");

            if (heldObj == null) //if an object is NOT already being held
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    //pick up the object
                    pickupObject(hit.transform.gameObject);
                }
            }
        }

        //DROPPING-----------------------------
        if (Keyboard.current.tabKey.wasPressedThisFrame && heldObj != null) //if tab was pressed to drop
        {
            Debug.Log("tab was presssed to drop object");
            dropObject();
        }

        //MOVING-----------------------------
        if (heldObj != null) //if an object is currently being held
        {
            //move the object around
            moveObject();
        }
    }

    /*----------------FUNCTIONS---------------*/
    void pickupObject(GameObject pickObj)
    {
        //Debug.Log("pickup() called from server");
        /* if (pickObj.GetComponent<Rigidbody>())
         {
             heldObjRB = pickObj.GetComponent<Rigidbody>();
             //make the object float
             heldObjRB.useGravity = false;
             heldObjRB.linearDamping = 10;
             heldObjRB.constraints = RigidbodyConstraints.FreezeRotation; //avoids spinning

             heldObjRB.transform.parent = holdArea; //parent it to the hold area

             heldObj = pickObj;
         }*/

        if (!IsOwner) return; // only the player controlling this can request pickup

        NetworkObject netObj = pickObj.GetComponent<NetworkObject>();
        if (netObj == null) return; // only NetworkObjects

        PickupObjectServerRpc(netObj.NetworkObjectId, OwnerClientId); // ask server to pick it up

    }

    void dropObject()
    {
        if (!IsOwner) return;
        if (heldObj == null) return;

        NetworkObject netObj = heldObj.GetComponent<NetworkObject>();
        if (netObj != null)
            DropObjectServerRpc(netObj.NetworkObjectId);

        // Clear local reference immediately so moveObject() stops running
        heldObj = null;
        heldObjRB = null;
    }

    void moveObject()
    {
        if (heldObj == null) return; // stops dragging after drop

        // optional: physics movement
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }


    [ServerRpc(RequireOwnership = false)]
    void PickupObjectServerRpc(ulong objectId, ulong playerClientId)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];

        // Transfer ownership so the client can interact with it
        netObj.ChangeOwnership(playerClientId);

        // Reparent to player's hold area
        netObj.transform.SetParent(NetworkManager.Singleton.ConnectedClients[playerClientId].PlayerObject.transform);

        // Optional: reset local position
        //netObj.transform.localPosition = Vector3.zero;

        // Optionally store reference on server if needed
    }

    [ServerRpc(RequireOwnership = false)]
    void DropObjectServerRpc(ulong objectId)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];

        // Unparent on server
        netObj.transform.SetParent(null);

        // Optional: reset rotation/position or velocity
        Rigidbody rb = netObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
        }
    }



}
