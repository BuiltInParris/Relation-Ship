using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takeoff : MonoBehaviour
{
    private float yVel = 0;
    private int players = 0;

    // Start is called before the first frame update
    void Start()
    {
        int totalScore = 0;

        List<GameState.PlayerState> states = new List<GameState.PlayerState>();

        foreach(GameState.PlayerState playerState in GameState.playerStates){
            totalScore += playerState.score;
        }

        int startVelocity = 1050;

        int playersLeft = (int) (totalScore / 50);
        GameState.numSurvivors = playersLeft;

        foreach (GameObject trainCar in GameObject.FindGameObjectsWithTag("Train"))
        {
            trainCar.GetComponent<Rigidbody2D>().AddForce(new Vector2(startVelocity + playersLeft * 65, yVel));
        }
    }
}
