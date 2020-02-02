using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takeoff : MonoBehaviour
{
    private float yVel = 500;
    Dictionary<int, int> velocities;
    private int players = 0;
    //1050 is 0 players
    //1100 is 1 player
    //1200 is 2 players
    //1250 is 3 players
    //1300 is 4 players

    // Start is called before the first frame update
    void Start()
    {
        velocities = new Dictionary<int, int>();
        velocities.Add(0, 1050);
        velocities.Add(1, 1100);
        velocities.Add(2, 1200);
        velocities.Add(3, 1250);
        velocities.Add(4, 1300);

        int totalScore = 0;

        List<PlayerState> states = new List<PlayerState>();

        foreach(PlayerState playerState in GameState.playerStates){
            totalScore += playerState.score;
        }

        int playersLeft = 0;

        if(totalScore > 50){
            playersLeft = 1;
        } else if (totalScore > 100){
            playersLeft = 2;
        } else if (totalScore > 150){
            playersLeft = 3;
        } else if(totalScore > 200){
            playersLeft = 4;
        }

        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(velocities[playersLeft], yVel));
    }
}
