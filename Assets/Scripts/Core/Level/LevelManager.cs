using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Data Container")]
    public LevelContainer levelContainer;

    [HideInInspector] public int BuildLevelIndex;
    [HideInInspector] public int OverallLevelIndex;
    [SerializeField] private bool _isRandomAfterFinished;

    private void Start()
    {
        UIEvents.Instance.AssignUILevelButtons(NextLevel, LoadLevel);
        BuildLevelIndex = PlayerPrefs.GetInt("BUILDLEVELINDEX", 0);
        OverallLevelIndex = PlayerPrefs.GetInt("OVERALLLEVELINDEX", 0);
        GameEvents.Instance.CreateGrid(levelContainer.Grids[BuildLevelIndex]);
    }

    public void NextLevel()
    {
        if (++BuildLevelIndex >= levelContainer.Grids.Count)
        {
            if (_isRandomAfterFinished)
            {
                int randLevel = Random.Range(0, levelContainer.Grids.Count);
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
        SceneManager.LoadScene(0);
    }
}
