using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public double time;
    public double gameTotalTime;
    public GameObject trainPrefab;
    GameObject train;
    public GameObject playerPrefab;
    List<GameObject> playersObjects;
    List<Player> players;
    public int numberOfPlayers = 2;
    public int totalPoints = 0;
    public int maxPoints;

    // Start is called before the first frame update
    void Start()
    {
        maxPoints = numberOfPlayers * (int)gameTotalTime * 5;
        train = Instantiate(trainPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        train.GetComponent<Train>().numberOfCars = numberOfPlayers + Constants.ADDITONAL_CARS;

        playersObjects = new List<GameObject>();
        players = new List<Player>();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            int xLoc = Constants.DISTANCE_BETWEEN_CARS * (i - numberOfPlayers/2);
            GameObject player = Instantiate(playerPrefab, new Vector3(xLoc, 0, 0), Quaternion.identity);
            if(Gamepad.all.Count >= numberOfPlayers)
            {
                player.GetComponent<PlayerInput>().currentActionMap.devices = new InputDevice[] { Gamepad.all[i] };
            }
            playersObjects.Add(player);
            Player playerScript = player.GetComponent<Player>();
            playerScript.setCurrentPosition(new Vector2(xLoc, 0));
            players.Add(playerScript);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time = time + 0.02;
        aggregatePointTotal();
        if(time > gameTotalTime){
            endGame();
        }
    }

    void endGame(){

        players.Sort((x, y) =>  x.points.CompareTo(y.points));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
