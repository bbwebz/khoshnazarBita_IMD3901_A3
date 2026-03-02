using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : NetworkBehaviour
{
    public Image crosshairIMG;
    public Color normalColor = Color.white;
    public Color interactColor = Color.blue;

    [ServerRpc(RequireOwnership = false)]
    public void setInteractServerRpc(bool canInteract)
    {
        crosshairIMG.color = canInteract ? interactColor : normalColor;
    }
}
