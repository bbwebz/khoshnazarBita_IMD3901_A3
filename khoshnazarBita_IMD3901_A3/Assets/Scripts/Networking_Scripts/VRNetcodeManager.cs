using UnityEngine;
using Unity.Netcode;

public class VRNetcodeManager : NetworkBehaviour
{
    public Camera VrCamera;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            VrCamera.enabled = false;
        }
    }
}
