using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class NetworkHandAnimations : NetworkBehaviour
{
    public InputActionProperty triggerValue;
    public InputActionProperty gripValue;
    public Animator handAnimator;

    void Update()
    {
        if (IsOwner)
        {
            //read the grip and trigger values
            float trigger = triggerValue.action.ReadValue<float>();
            float grip = gripValue.action.ReadValue<float>();

            //change the trigger value in the animator
            handAnimator.SetFloat("Trigger", trigger);
            handAnimator.SetFloat("Grip", grip);

            //update host, then within that call function to update client
            SubmitHandAnimationServerRpc(trigger, grip);
        }
    }

    [ServerRpc] //run code on server to update the host's hands
    void SubmitHandAnimationServerRpc(float trigger, float grip)
    {
        //sned the values to the client
        UpdateHandAnimationClientRpc(trigger, grip);
    }

    [ClientRpc] //run code on client to update the client's hands
    void UpdateHandAnimationClientRpc(float trigger, float grip)
    {
        if (IsOwner) return; //owner already updated locally

        handAnimator.SetFloat("Trigger", trigger);
        handAnimator.SetFloat("Grip", grip);
    }
}
