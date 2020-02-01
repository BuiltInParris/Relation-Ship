using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    double time;
    double gameTotalTime = 60;
    Train train;
    List<Player> players;
    int numberOfPlayers = 2;
    int totalPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        train = new Train();
        train.numberOfCars = numberOfPlayers;
        players = new List<Player>();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player player = new Player();
            players.Add(player);
        }
    }

    // Update is called once per frame
    void fixedUpdate()
    {
        time = time + 0.02;
        aggregatePointTotal();
        if(time > gameTotalTime){
            endGame();
        }
    }

    void endGame(){
        // This is where we switch scenes to the end scene
        // Train crash (or not?)
        // Victory/loss screen
    }

    void aggregatePointTotal(){
        totalPoints = 0;
        foreach(Player player in players){
            totalPoints = totalPoints + player.points;
        }
    }
}
