using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionDelay : MonoBehaviour
{
    public string nextScene;
    public float delayTime;
    private float elapsedTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= delayTime)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
