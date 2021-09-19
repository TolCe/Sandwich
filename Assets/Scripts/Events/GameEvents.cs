using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public event Action<GridVO> OnCheckLevelContainerEvent;
    public void CreateGrid(GridVO vo)
    {
        if (OnCheckLevelContainerEvent != null)
        {
            OnCheckLevelContainerEvent(vo);
        }
    }

    public event Action OnGameStartEvent;
    public void GameStart()
    {
        if (OnGameStartEvent != null)
        {
            OnGameStartEvent();
        }
    }

    public event Action<Tile[,]> OnGridCreatedEvent;
    public void GridCreated(Tile[,] tiles)
    {
        if (OnGridCreatedEvent != null)
        {
            OnGridCreatedEvent(tiles);
        }
    }

    public event Action<Ingredient> OnTouchDownEvent;
    public void TouchDown(Ingredient selectedIngredient)
    {
        if (OnTouchDownEvent != null)
        {
            OnTouchDownEvent(selectedIngredient);
        }
    }

    public event Action<Vector3> OnIngredientSelectedEvent;
    public void IngredientSelected(Vector3 direction)
    {
        if (OnIngredientSelectedEvent != null)
        {
            OnIngredientSelectedEvent(direction);
        }
    }

    public event Action OnIngredientPlacedEvent;
    public void IngredientPlaced()
    {
        if (OnIngredientPlacedEvent != null)
        {
            OnIngredientPlacedEvent();
        }
    }

    public event Action OnLevelSuccessEvent;
    public void LevelSucceded()
    {
        if (OnLevelSuccessEvent != null)
        {
            OnLevelSuccessEvent();
        }
    }
    public event Action OnLevelFailedEvent;
    public void LevelFailed()
    {
        if (OnLevelFailedEvent != null)
        {
            OnLevelFailedEvent();
        }
    }
}
