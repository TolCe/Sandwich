using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Data Container")]
    public LevelContainer levelContainer;

    [HideInInspector] public int BuildLevelIndex;
    [HideInInspector] public int OverallLevelIndex;
    [SerializeField] bool isRandomAfterFinished;

    private void Start()
    {
        BuildLevelIndex = PlayerPrefs.GetInt("BUILDLEVELINDEX", 0);
        OverallLevelIndex = PlayerPrefs.GetInt("OVERALLLEVELINDEX", 0);
        SetLevelStuff();
    }

    public void NextLevel()
    {
        if (++BuildLevelIndex >= levelContainer.levels.Count)
        {
            if (isRandomAfterFinished)
            {
                int randLevel = Random.Range(0, levelContainer.levels.Count);
                BuildLevelIndex = randLevel;
            }
            else
            {
                BuildLevelIndex = 0;
            }
        }

        PlayerPrefs.SetInt("BUILDLEVELINDEX", BuildLevelIndex);
        PlayerPrefs.SetInt("OVERALLLEVELINDEX", ++OverallLevelIndex);
        LoadLevel();
    }

    public void LoadLevel()
    {
        LoadGameScene(BuildLevelIndex);
    }

    void LoadGameScene(int levelIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelIndex);
    }

    private void SetLevelStuff()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        //Instantiate(levelContainer.levels[BuildLevelIndex].levelPrefab);
        //UIManager.Instance.ResetUI();
        //UIManager.Instance.SetListeners();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Invoke("SetLevelStuff", 0.01f);
    }
}
