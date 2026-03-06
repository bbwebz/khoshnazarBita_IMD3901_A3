using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PointManager : NetworkBehaviour
{
    public GameObject ScoreBoards;

    //texts that need to be updated for both canvases
    public TextMeshProUGUI teamText1_cv1;
    public TextMeshProUGUI teamText2_cv2;

    //public NetworkVariable<TMPro.TextMeshProUGUI> TEAMTEXT_cv1;
    //public NetworkVariable<TMPro.TextMeshProUGUI> TEAMTEXT_cv2;

    public TextMeshProUGUI p1Text_cv1;
    public TextMeshProUGUI p2Text_cv1;
    public TextMeshProUGUI p1Text_cv2;
    public TextMeshProUGUI p2Text_cv2;

    public int player1Points = 0;
    public int player2Points = 0;

    public NetworkVariable<int> teamPoints;


    public override void OnNetworkSpawn()
    {
        //set initial value
        teamPoints.Value = 0;
        teamPoints.OnValueChanged += OnTeamPointsChanged; 

        //set initial value when object spawns
        UpdateTeamText(teamPoints.Value);
    }

    private void OnTeamPointsChanged(int oldValue, int newValue)
    {
        //if the team points was changed by the host or the client(from within addTeamPointServerRpc)
        UpdateTeamText(newValue);
    }

    private void UpdateTeamText(int value)
    {
        teamText1_cv1.text = value.ToString();
        teamText2_cv2.text = value.ToString();
    }
    
    public void addP1point()
    {
        player1Points += 1;
        Debug.Log("added point for P1");

        //update text on scoreboard 1
        p1Text_cv1.GetComponent<TextMeshProUGUI>().text = " " + player1Points;
        p2Text_cv1.GetComponent<TextMeshProUGUI>().text = " " + player2Points;

        //update text on scoreboard 2
        p1Text_cv2.GetComponent<TextMeshProUGUI>().text = " " + player1Points;
        p2Text_cv2.GetComponent<TextMeshProUGUI>().text = " " + player2Points;
    }

    public void addP2point()
    {
        player2Points += 1;
        Debug.Log("added point for P2");

        //update text on scoreboard 1
        p1Text_cv1.GetComponent<TextMeshProUGUI>().text = " " + player1Points;
        p2Text_cv1.GetComponent<TextMeshProUGUI>().text = " " + player2Points;

        //update text on scoreboard 2
        p1Text_cv2.GetComponent<TextMeshProUGUI>().text = " " + player1Points;
        p2Text_cv2.GetComponent<TextMeshProUGUI>().text = " " + player2Points;
    }

    [ServerRpc(RequireOwnership = false)] //host and client are able to ask the server to update the teampoints
    public void addTeamPointServerRpc()
    {
        //increasae the value of team points on both host and client since its a network variable
        teamPoints.Value += 1;
        Debug.Log("added point for team points");
    }



}
