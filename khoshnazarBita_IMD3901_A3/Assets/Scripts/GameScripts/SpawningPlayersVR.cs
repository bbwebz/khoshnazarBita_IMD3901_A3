using Unity.Netcode;
using UnityEngine;

public class SpawningPlayersVR : NetworkBehaviour
{
    public Transform pointP1;
    public Transform pointP2;

    public override void OnNetworkSpawn()
    {
        //spawning the players at their given spawn points
        if (NetworkManager.Singleton.LocalClientId == 0) //host
        {
            Debug.Log("placed host at spawn point");
            gameObject.transform.transform.position = pointP1.position;
        }
        else if (NetworkManager.Singleton.LocalClientId == 1) //client
        {
            Debug.Log("placed client at spawn point");
            gameObject.transform.transform.position = pointP2.position;
        }
    }

}
