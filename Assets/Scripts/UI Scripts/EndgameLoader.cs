using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndgameLoader : MonoBehaviour
{
    private Game game;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObject = GameObject.Find("GameController");
        game = gameObject.GetComponent<Game>();
    }

   
    void FixedUpdate()
    {
        float progress = (float)game.time / (float)game.gameTotalTime;

        slider.value = progress;
        //Debug.Log("progress: " + progress);
    }
}
