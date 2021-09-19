using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Data Container")]
    public LevelContainer levelContainer;

    [HideInInspector] public int ContainerLevelIndex;
    public bool LoadIndicatedLevel;
    [ShowIf("LoadIndicatedLevel")]
    public int IndicatedLevelIndex = 0;
    [SerializeField] private bool _isRandomAfterFinished;

    private void Start()
    {
        UIEvents.Instance.AssignUILevelButtons(NextLevel, LoadLevel);

        if (LoadIndicatedLevel)
        {
            ContainerLevelIndex = IndicatedLevelIndex;
        }
        else
        {
            ContainerLevelIndex = PlayerPrefs.GetInt("CONTAINERLEVELINDEX", 0);
        }

        GameEvents.Instance.CreateGrid(levelContainer.Grids[ContainerLevelIndex]);
    }

    public void NextLevel()
    {
        if (!levelContainer.Grids[ContainerLevelIndex].randomizeLevels)
        {
            if (++ContainerLevelIndex >= levelContainer.Grids.Count)
            {
                if (_isRandomAfterFinished)
                {
                    int randLevel = Random.Range(0, levelContainer.Grids.Count);
                    ContainerLevelIndex = randLevel;
                }
                else
                {
                    ContainerLevelIndex = 0;
                }
            }

            PlayerPrefs.SetInt("BUILDLEVELINDEX", ContainerLevelIndex);
        }

        LoadLevel();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(0);
    }

    //private void SaveLevel()
    //{

    //    string json = JsonUtility.ToJson(myObject);
    //}
    //private void LoadLevelFromJSON()
    //{
    //    myObject = JsonUtility.FromJson<MyClass>(json);
    //}
}
