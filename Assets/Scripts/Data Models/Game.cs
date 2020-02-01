﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    double time;
    double gameTotalTime = 60;
    public GameObject trainPrefab;
    GameObject train;
    public GameObject playerPrefab;
    List<GameObject> playersObjects;
    List<Player> players;
    public int numberOfPlayers = 0;
    int totalPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject settings = GameObject.Find("GameSettings");
        numberOfPlayers = settings.GetComponent<GameSettings>().playerCount;
        train = Instantiate(trainPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        train.GetComponent<Train>().numberOfCars = numberOfPlayers;

        playersObjects = new List<GameObject>();
        players = new List<Player>();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            int xLoc = Constants.DISTANCE_BETWEEN_CARS * (i - numberOfPlayers/2);
            GameObject player = Instantiate(playerPrefab, new Vector3(xLoc, 0, 0), Quaternion.identity);
            playersObjects.Add(player);
            players.Add(player.GetComponent<Player>());
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

        players.Sort((x, y) =>  x.points.CompareTo(y.points));
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
