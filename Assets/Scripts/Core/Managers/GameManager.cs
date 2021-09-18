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
        GameEvents.Instance.OnLevelCompletedEvent += LevelCompleted;
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

    public void LevelCompleted(bool success, float delayForPanel)
    {
        if (gameStateManager.gameState != GameState.Completed && gameStateManager.gameState != GameState.Failed)
        {
            if (success)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        }
    }
}
