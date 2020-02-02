using System.Collections;
using System.Collections.Generic;

public class GameState
{
    public class PlayerState
    {
        public int playerId;
        public int score;
    }

    public static List<PlayerState> playerStates = new List<PlayerState>();
}
