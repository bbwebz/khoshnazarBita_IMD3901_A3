using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PointManager : NetworkBehaviour
{
    public GameObject ScoreBoards;

    //texts that need to be updated for both canvases
    public TextMeshProUGUI teamText1_cv1;
    public TextMeshProUGUI teamText2_cv2;

    public TextMeshProUGUI p1Text_cv1;
    public TextMeshProUGUI p2Text_cv1;
    public TextMeshProUGUI p1Text_cv2;
    public TextMeshProUGUI p2Text_cv2;

    //network variable for things that need to be synched between host and client
    public NetworkVariable<int> teamPoints;
    public NetworkVariable<int> player1Points;
    public NetworkVariable<int> player2Points;


    public override void OnNetworkSpawn()
    {
        //set initial value
        teamPoints.Value = 0;
        player1Points.Value = 0;
        player2Points.Value = 0;

        //upate the values when they are changed
        teamPoints.OnValueChanged += OnTeamPointsChanged;
        player1Points.OnValueChanged += OnP1PointsChanged;
        player2Points.OnValueChanged += OnP2PointsChanged;

        //set initial value of all the texts when the object spawns
        UpdateTeamText(teamPoints.Value);
    }

    //TEAM POINTS---------
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
    [ServerRpc(RequireOwnership = false)] //host and client are able to ask the server to update the teampoints
    public void addTeamPointServerRpc()
    {
        //increasae the value of team points on both host and client since its a network variable
        teamPoints.Value += 1;
        Debug.Log("added point for team points");
    }


    //P1 POINTS---------
    private void OnP1PointsChanged(int oldValue, int newValue)
    {
        UpdateP1Text(newValue);
    }
    private void UpdateP1Text(int value)
    {
        //update text on scoreboard 1
        p1Text_cv1.text = value.ToString();
        //update text on scoreboard 2
        p1Text_cv2.text = value.ToString();
    }
    [ServerRpc(RequireOwnership = false)]
    public void addP1PointServerRpc()
    {
        player1Points.Value += 1;
        Debug.Log("added point for P1");
    }

    //P2 POINTS---------
    private void OnP2PointsChanged(int oldValue, int newValue)
    {
        UpdateP2Text(newValue);
    }
    private void UpdateP2Text(int value)
    {
        //update text on scoreboard 1
        p2Text_cv1.text = value.ToString();
        //update text on scoreboard 2
        p2Text_cv2.text = value.ToString();
    }
    [ServerRpc(RequireOwnership = false)]
    public void addP2PointServerRpc()
    {
        player2Points.Value += 1;
        Debug.Log("added point for P2");
    }

}
