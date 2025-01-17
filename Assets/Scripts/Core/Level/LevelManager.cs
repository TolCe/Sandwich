﻿using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Data Container")]
    public LevelContainer LevelContainer;
    public LevelContainer RandomizedLevelContainer;

    [HideInInspector] public int ContainerLevelIndex;
    public bool LoadIndicatedLevel;
    [ShowIf("LoadIndicatedLevel")]
    public int IndicatedLevelIndex = 0;
    [ShowIf("LoadIndicatedLevel")]
    public bool LoadRandomized;

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

        if (!LoadRandomized)
        {
            if (LevelContainer[ContainerLevelIndex].RandomizeLevels)
            {
                GameEvents.Instance.CreateGrid(LevelContainer, RandomizedLevelContainer, ContainerLevelIndex, true);
            }
            else
            {
                GameEvents.Instance.CreateGrid(LevelContainer, RandomizedLevelContainer, ContainerLevelIndex, false);
            }
        }
        else
        {
            GameEvents.Instance.CreateGrid(RandomizedLevelContainer, RandomizedLevelContainer, ContainerLevelIndex, false);
        }
    }

    public void NextLevel()
    {
        if (++ContainerLevelIndex >= LevelContainer.Grids.Count)
        {
            ContainerLevelIndex = 0;
        }

        PlayerPrefs.SetInt("CONTAINERLEVELINDEX", ContainerLevelIndex);
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
