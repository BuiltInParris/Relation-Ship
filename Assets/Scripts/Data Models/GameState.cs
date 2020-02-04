using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public class PlayerState
    {
        public int playerId;
        public int score;
        public Color color;
    }

    public static List<PlayerState> playerStates = new List<PlayerState>();

    public static int numSurvivors = 0;
}
