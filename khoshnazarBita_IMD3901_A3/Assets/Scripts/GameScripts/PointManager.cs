using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public GameObject ScoreBoards;
    public TextMeshProUGUI teamText1;


    public int player1Points = 0;
    public int player2Points = 0;

    public int teamPoints = 0;

    public void addP1point()
    {
        player1Points += 1;
        Debug.Log("added point for P1");
        
        //update text on scoreboard 1
        
        
        //update text on scoreboard 2


    }

    public void addP2point()
    {
        player2Points += 1;
        Debug.Log("added point for P2");


    }

    public void addTeamPoint()
    {
        teamPoints += 1;
        Debug.Log("added point for team points");

        teamText1.GetComponent<TextMeshProUGUI>().text = "" + teamPoints;

    }


}
