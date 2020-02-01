using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    private Game game;

    public Slider timerSlider;
    public Slider savedSlider;


    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObject = GameObject.Find("GameController");
        game = gameObject.GetComponent<Game>();
    }

   
    void FixedUpdate()
    {
        // time elapsed
        float timerProgress = (float)game.time / (float)game.gameTotalTime;
        float savedProgress = (float)game.totalPoints / (float)game.maxPoints;

        timerSlider.value = timerProgress;

        // % of train saved
        savedSlider.value = savedProgress;

    }
}
