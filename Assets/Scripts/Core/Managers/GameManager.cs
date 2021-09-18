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
        UIEvents.Instance.AssignUI(StartGame);
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
            StartCoroutine(LevelEndActions(success, delayForPanel));
        }
    }
    public void LevelFailed(float delayForPanel)
    {
        if (gameStateManager.gameState != GameState.Completed && gameStateManager.gameState != GameState.Failed)
        {
        }
    }

    private IEnumerator LevelEndActions(bool success, float delayForEndPanels)
    {
        if (success)
        {
            Debug.Log("Success");
        }
        else
        {
            Debug.Log("Fail");
        }

        yield return null;
        //uimanager.instance.changeactivityofelement(uimanager.instance.levelelements, false);

        //if (islevelsucceded)
        //{
        //    gamestatemanager.gamestate = gamestate.completed;

        //    if (camera.main.transform.childcount > 0)
        //    {
        //        camera.main.transform.getchild(0).getcomponent<particlesystem>().play();
        //    }
        //}
        //else
        //{
        //    gamestatemanager.gamestate = gamestate.failed;
        //}

        //float _timer = 0;
        //while (_timer < delayforendpanels)
        //{
        //    _timer += time.fixeddeltatime;
        //    yield return new waitforfixedupdate();
        //}

        //if (islevelsucceded)
        //{
        //    uimanager.instance.levelcompletedpanel.setactive(true);
        //}
        //else
        //{
        //    uimanager.instance.levelfailedpanel.setactive(true);
        //}
    }
}
