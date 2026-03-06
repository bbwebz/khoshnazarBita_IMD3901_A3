using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PointManager : MonoBehaviour
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

    public int teamPoints = 0;

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

    //[ServerRpc(RequireOwnership = false)] 
    public void addTeamPoint()
    {
        teamPoints += 1;
        Debug.Log("added point for team points");

        //update text on both collaborative canvases
        teamText1_cv1.GetComponent<TextMeshProUGUI>().text = " " + teamPoints;
        teamText2_cv2.GetComponent<TextMeshProUGUI>().text = " " + teamPoints;
        //TEAMTEXT_cv1.Value.text = teamPoints.ToString();
        //TEAMTEXT_cv2.Value.text = teamPoints.ToString();


       //updateTeamTextClientRpc();
    }

   /* [ClientRpc] //called by the server, runs on the client
    void updateTeamTextClientRpc()
    {
        teamText1_cv1.GetComponent<TextMeshProUGUI>().text = " " + teamPoints;
        teamText2_cv2.GetComponent<TextMeshProUGUI>().text = " " + teamPoints;
        //TEAMTEXT_cv1.Value.text = teamPoints.ToString();
        //TEAMTEXT_cv2.Value.text = teamPoints.ToString();
    }*/


}
