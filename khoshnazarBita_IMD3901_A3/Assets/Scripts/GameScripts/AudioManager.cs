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
    //public AudioClip Tile2;
    //public AudioClip Tile3;

    //store strings of which tile each player pressed (for comparison later on)
    string player1Sound; 
    string player2Sound;

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

        if (whoPressed == 0)
        {
            player1Sound = tileName;
        }
        else if (whoPressed == 1)
        {
            player2Sound = tileName;
        }

        switch (tileName)
        {

            case "Tile1":
                PlaySFX(Tile1);
                break;
            
        }
    }




}
