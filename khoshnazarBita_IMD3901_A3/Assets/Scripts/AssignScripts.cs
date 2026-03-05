//using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;


public class AssignScripts : MonoBehaviour
{
    public static AssignScripts assigner;
    
    public GameObject playerPCPrefab;
    public GameObject playerVRPrefab;

    public GameObject AudioManager;


    private void Awake()
    {
        if (assigner == null)
        {
            assigner = this;
        }
    }

    private void Update()
    {
        if (playerPCPrefab != null)
        {
            //give PlayerInteractionNet field on the player prefabs access to the audio manager
            playerPCPrefab.GetComponent<PlayerInteractionNet>().audioManager_access = AudioManager.GetComponent<AudioManager>();
            playerVRPrefab.GetComponent<VRControllerInputNet>().audioManager_access = AudioManager.GetComponent<AudioManager>();
        }
    }

}
