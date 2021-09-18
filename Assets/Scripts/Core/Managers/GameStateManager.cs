using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameState gameState;

    private void Start()
    {
        //gameState = GameState.Start;
    }
}

public enum GameState
{
    Start,
    Playing,
    Idle,
    Completed,
    Failed,
}
