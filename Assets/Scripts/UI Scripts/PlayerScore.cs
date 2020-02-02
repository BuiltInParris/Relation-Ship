using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public int playerId;

    private TextMeshProUGUI textMesh;
    private GameState.PlayerState playerState;

    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isInGame = false;

        foreach (GameState.PlayerState player in GameState.playerStates)
        {
            if (player.playerId == playerId)
            {
                isInGame = true;
                playerState = player;
                textMesh.text = "Player " + (playerId + 1) + ": " + playerState.score;
                textMesh.color = playerState.color;

                break;
            }
        }

        if (!isInGame)
        {
            gameObject.SetActive(false);
        }
    }
}
