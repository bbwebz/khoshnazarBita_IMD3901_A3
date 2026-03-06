using Unity.Netcode;
using UnityEngine;

public class AudioManager : NetworkBehaviour
{
    [Header("---- Audio Source ----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---- Audio Clip ----")]
    //public AudioClip background;
    public AudioClip Tile1;
    public AudioClip Tile2;
    public AudioClip Tile3;
    public AudioClip Tile4;
    public AudioClip Tile5;
    public AudioClip Tile6;
    public AudioClip Tile7;
    public AudioClip Tile8;
    public AudioClip Tile9;


    //store strings of which tile each player pressed (for comparison later on)
    string player1Sound; 
    string player2Sound;
    int firstPlayer = -1; //player who presses first

    public PointManager pointManager_access;
    public ChooseGameMode chooseGameMode_access;

    /*
    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    */

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }


    [ServerRpc(RequireOwnership = false)]
    public void playTileSoundServerRpc(string tileName, int whoPressed)
    {
        //if whoPressed = 0 it was the host
        //if whoPressed = 1 it was the client

        if (firstPlayer == -1)
        {
            firstPlayer = whoPressed;
        }

        if (whoPressed == 0)
        {
            player1Sound = tileName;
            Debug.Log("host played " + tileName + " sound");
        }
        
        if (whoPressed == 1)
        {
            player2Sound = tileName;
            Debug.Log("client played " + tileName + " sound");
        }

        if(player1Sound != null && player2Sound != null) //only if both players have played a sound
        {
            if (player1Sound == player2Sound && chooseGameMode_access.isCollab == true) //if they played the same sound and they are in collaborative mode
            {
                Debug.Log("players played the SAME sound");
                pointManager_access.addTeamPointServerRpc();
            }
            else if (player1Sound != player2Sound && chooseGameMode_access.isComp == true) //if they didnt play the same sound and they are in competitive mode
            {
                Debug.Log("players played a DIFFERENT sound");

                //means one player got it wrong
                //add a point for the player who pressed a tile first
                //Debug.Log("whoPressed first was: " + whoPressed);
                Debug.Log("firstPlayer was: " + firstPlayer);

                if (firstPlayer == 0)
                {
                    pointManager_access.addP1PointServerRpc();
                }
                else if (firstPlayer == 1)
                {
                    pointManager_access.addP2PointServerRpc();
                }
            }

            //reset for next round
            player1Sound = null;
            player2Sound = null;
            firstPlayer = -1;
        }
       

        //play sound according to which tile was pressed (each tile plays a unique note)
        switch (tileName)
        {
            case "Tile1":
                PlaySFX(Tile1);
                break;
            case "Tile2":
                PlaySFX(Tile2);
                break;
            case "Tile3":
                PlaySFX(Tile3);
                break;
            case "Tile4":
                PlaySFX(Tile4);
                break;
            case "Tile5":
                PlaySFX(Tile5);
                break;
            case "Tile6":
                PlaySFX(Tile6);
                break;
            case "Tile7":
                PlaySFX(Tile7);
                break;
            case "Tile8":
                PlaySFX(Tile8);
                break;
            case "Tile9":
                PlaySFX(Tile9);
                break;
        }
    }




}
