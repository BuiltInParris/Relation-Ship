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
    public GameObject enginePrefab;
    public GameObject playerPrefab;
    public GameObject train;
    public GameObject engine;
    List<GameObject> playersObjects;
    List<Player> players;
    public int numberOfPlayers = 2;
    public int totalPoints = 0;
    public int maxPoints;
    public Dictionary<int, Color> playerColors;


    // Start is called before the first frame update
    void Start()
    {
        numberOfPlayers = GameSettings.playerCount;

        playerColors = new Dictionary<int, Color>();
        playerColors.Add(0, new Color(25f, 0f, 0f, 255f));
        playerColors.Add(1, new Color(0f, 25f, 0f, 255f));
        playerColors.Add(2, new Color(0f, 0f, 25f, 255f));
        playerColors.Add(3, new Color(0f, 25f, 255f, 255f));

        float xLoc = Constants.DISTANCE_BETWEEN_CARS * (numberOfPlayers + Constants.ADDITONAL_CARS)/2 + 0.5f;
        engine = Instantiate(enginePrefab, new Vector3(xLoc, 0, 0), Quaternion.identity);

        maxPoints = numberOfPlayers * (int)gameTotalTime * 5;
        train = Instantiate(trainPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        train.GetComponent<Train>().numberOfCars = numberOfPlayers + Constants.ADDITONAL_CARS;

        playersObjects = new List<GameObject>();
        players = new List<Player>();

        GameState.playerStates = new List<GameState.PlayerState>();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            xLoc = Constants.DISTANCE_BETWEEN_CARS * (i - numberOfPlayers/2);
            GameObject player = Instantiate(playerPrefab, new Vector3(xLoc, 0, 0), Quaternion.identity);

            player.GetComponent<SpriteRenderer>().color = playerColors[i];

            if(Gamepad.all.Count >= numberOfPlayers)
            {
                player.GetComponent<PlayerInput>().currentActionMap.devices = new InputDevice[] { Gamepad.all[i] };
            }
            playersObjects.Add(player);
            Player playerScript = player.GetComponent<Player>();
            playerScript.setCurrentPosition(new Vector2(xLoc, 0));

            // Create a new player state and add it to the player
            GameState.PlayerState playerState = new GameState.PlayerState();
            playerState.playerId = i;
            playerState.color = playerColors[i];
            GameState.playerStates.Add(playerState);
            playerScript.playerState = GameState.playerStates[GameState.playerStates.Count - 1];
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

        GameState.playerStates.Sort((x, y) => y.score.CompareTo(x.score));

        // if (totalPoints >= maxPoints)
        {
            SceneManager.LoadScene("Scenes/WinScene");
        }

        // SceneManager.LoadScene("Scenes/End Screen");
        // This is where we switch scenes to the end scene
        // Train crash (or not?)
        // Victory/loss screen
    }

    void aggregatePointTotal(){
        totalPoints = 0;
        foreach(GameState.PlayerState playerState in GameState.playerStates){
            totalPoints += playerState.score;
        }
    }
}
