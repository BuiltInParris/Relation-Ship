using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public Canvas canvas;
    public VideoPlayer videoPlayer;

    public void PlayGame ()
    {
        // play cutscene
        canvas.gameObject.SetActive(false);
        videoPlayer.gameObject.SetActive(true);

        // load game
        videoPlayer.loopPointReached += EndReached;

    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
