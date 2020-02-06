using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    const string aliveString = "Alive";
    const string deadString = "Deceased";
    const string playerString = "Player";

    const string namePrefix = "Name";
    const string scorePrefix = "Score";
    const string statusPrefix = "Status";

    struct PlayerScoreDisplay
    {
        public TextMeshProUGUI nameDisplay;
        public TextMeshProUGUI scoreDisplay;
        public TextMeshProUGUI statusDisplay;
    }

    PlayerScoreDisplay[] scoreDisplays;

    void PopulateDisplays()
    {
        scoreDisplays = new PlayerScoreDisplay[4];

        foreach (Transform t in transform)
        {
            string idString = t.name.Substring(t.name.Length - 1);

            int id = 0;

            try
            {
                id = Convert.ToInt32(idString) - 1;
            } catch (FormatException)
            {
                continue;
            }

            if (t.name.StartsWith(namePrefix))
            {
                t.gameObject.SetActive(false);
                scoreDisplays[id].nameDisplay = t.GetComponent<TextMeshProUGUI>();
            }
            else if (t.name.StartsWith(scorePrefix))
            {
                t.gameObject.SetActive(false);
                scoreDisplays[id].scoreDisplay = t.GetComponent<TextMeshProUGUI>();
            }
            else if (t.name.StartsWith(statusPrefix))
            {
                t.gameObject.SetActive(false);
                scoreDisplays[id].statusDisplay = t.GetComponent<TextMeshProUGUI>();
            }
        }
    }
    void Start()
    {
        PopulateDisplays();

        int i = 0;
        foreach (GameState.PlayerState playerState in GameState.playerStates)
        {
            PlayerScoreDisplay scoreDisplay = scoreDisplays[i];

            scoreDisplay.nameDisplay.gameObject.SetActive(true);
            scoreDisplay.scoreDisplay.gameObject.SetActive(true);
            scoreDisplay.statusDisplay.gameObject.SetActive(true);

            scoreDisplay.nameDisplay.text = playerString + " " + (playerState.playerId + 1);
            scoreDisplay.scoreDisplay.text = "" + playerState.score;

            i++;

            if (i <= GameState.numSurvivors)
            {
                scoreDisplay.statusDisplay.text = aliveString;
            }
            else
            {
                scoreDisplay.statusDisplay.text = deadString;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scenes/DynamicallyGeneratedLevel");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
