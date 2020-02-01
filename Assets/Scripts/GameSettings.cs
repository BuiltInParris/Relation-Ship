using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int playerCount = 2;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
