using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 5f;
    public Camera playerCamera;

    public Crosshair crosshair_access;
    public PickupController pickupController_access;
    //public TileAnimate tileAnimate_access;
   
    void Update()
    {
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

                if (Keyboard.current.pKey.wasPressedThisFrame)
                {
                    Debug.Log("tile pressed was: " + hit.collider.gameObject.name);
                    //tileAnimate_access.Press();

                    //trigger the press function only for the tile that was hit with the ray
                    TileAnimate tile = hit.collider.GetComponent<TileAnimate>();
                    if (tile != null)
                    {
                        //tile.Press();
                    }
                }

                //Debug.Log("interact was set to true");
                return;
            }
        }
        crosshair_access.setInteractServerRpc(false); //set it back to false if we look away from the object
    }
}
