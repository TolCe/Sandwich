using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region References
    [HideInInspector] public GameStateManager gameStateManager;
    #endregion

    private void Awake()
    {
        SetReferences();
    }
    private void Start()
    {
        GameEvents.Instance.OnLevelSuccessEvent += LevelSucceded;
        GameEvents.Instance.OnLevelFailedEvent += LevelFailed;
        UIEvents.Instance.AssignStartUI(StartGame);
    }

    public void SetReferences()
    {
        //Attach references here
        gameStateManager = FindObjectOfType<GameStateManager>();
        //
    }

    public void StartGame()
    {
        gameStateManager.gameState = GameState.Playing;
        GameEvents.Instance.GameStart();
    }

    public void LevelSucceded()
    {
        if (gameStateManager.gameState != GameState.Completed && gameStateManager.gameState != GameState.Failed)
        {
            gameStateManager.gameState = GameState.Completed;
        }
    }
    public void LevelFailed()
    {
        if (gameStateManager.gameState != GameState.Completed && gameStateManager.gameState != GameState.Failed)
        {
            gameStateManager.gameState = GameState.Failed;
        }
    }
}
