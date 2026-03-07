using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Management;

public class ChooseGameMode : MonoBehaviour
{
    bool isPCmode;
    bool isVRmode;
    public bool isComp = false;
    public bool isCollab = false;

    //initialize references to VR and PC objects that will need to be toggled on or off
    public GameObject VRplayer;
    public GameObject XRInteractionSimulator;
    public GameObject NetworkManagerObject;
    public GameObject PCplayer;

    //initialize references to canvases that will need to be toggled on or off
    public Canvas choose_game_mode;
    public Canvas choose_comp_collab;
    public Canvas networking_canvas;
    public Canvas collaborative_canvas1;
    public Canvas competitive_canvas1;
    public Canvas collaborative_canvas2;
    public Canvas competitive_canvas2;


    public void setVRmode()
    {
        isVRmode = true;
        Debug.Log("chose vr mode");

        //turn off all the PC related stuff and turn on anything VR related
        XRInteractionSimulator.SetActive(true);

        //switch the prefab in the networkmanagerobject to be set to the VR player prefab
        NetworkManager.Singleton.NetworkConfig.PlayerPrefab = VRplayer;

        //turn on the next canvas of choosing collaborative or competitive game mode
        choose_comp_collab.gameObject.SetActive(true);
        //turn off current canvas
        choose_game_mode.gameObject.SetActive(false);

    }

    public void setPCmode()
    {
        isPCmode = true;
        Debug.Log("chose pc mode");

        //turn off all the VR related stuff and turn on anything PC related
        XRInteractionSimulator.SetActive(false);

        //switch the prefab in the networkmanagerobject to be set to the PC player prefab
        NetworkManager.Singleton.NetworkConfig.PlayerPrefab = PCplayer;

        //turn on the next canvas of choosing collaborative or competitive game mode
        choose_comp_collab.gameObject.SetActive(true);
        //turn off current canvas
        choose_game_mode.gameObject.SetActive(false);
    }

    public void setCollabMode()
    {
        isCollab = true;

        Debug.Log("chose a collaborative game");

        //turn ON collaborative canvas in the scene
        collaborative_canvas1.gameObject.SetActive(true);
        collaborative_canvas2.gameObject.SetActive(true);
        //turn off competitive canvas in the scene
        competitive_canvas1.gameObject.SetActive(false);
        competitive_canvas2.gameObject.SetActive(false);

        //turn ON the next canvas of starting host or client
        networking_canvas.gameObject.SetActive(true);
        //turn off current canvas
        choose_comp_collab.gameObject.SetActive(false);
    }

    public void setCompMode()
    {
        isComp = true;

        Debug.Log("chose a competitive game");

        //turn OFF collaborative canvas in the scene
        collaborative_canvas1.gameObject.SetActive(false);
        collaborative_canvas2.gameObject.SetActive(false);
        //turn ON competitive canvas in the scene
        competitive_canvas1.gameObject.SetActive(true);
        competitive_canvas2.gameObject.SetActive(true);

        //turn on the next canvas of starting host or client
        networking_canvas.gameObject.SetActive(true);
        //turn off current canvas
        choose_comp_collab.gameObject.SetActive(false);
    }


}
