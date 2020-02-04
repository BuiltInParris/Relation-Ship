using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas canvas;

    public void PlayGame ()
    {
        SceneManager.LoadScene("Scenes/DynamicallyGeneratedLevel");

    }

    public void QuitGame()
    {
      Debug.Log("Quit");
      Application.Quit();
    }

    public void SetPlayerCount (int pcIndex)
    {
        GameSettings.playerCount = pcIndex + 2;
    }
}
