using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    private Ingredient _selectedIngredient;
    private Tile[,] _tiles;

    private void Awake()
    {
        GameEvents.Instance.OnGridCreatedEvent += GetGrid;
        GameEvents.Instance.OnTouchDownEvent += OnIngredientSelected;
        GameEvents.Instance.OnIngredientSelectedEvent += RotateIngredient;
    }

    private void GetGrid(Tile[,] tile)
    {
        _tiles = tile;
    }

    private void OnIngredientSelected(Ingredient selectedIngredient)
    {
        _selectedIngredient = selectedIngredient;
    }

    public void RotateIngredient(Vector3 direction)
    {
        Ingredient[] ingredientsToRotate = new Ingredient[_selectedIngredient.AttachedTile.OccupiedIngredients.Count];
        for (int i = 0; i < ingredientsToRotate.Length; i++)
        {
            ingredientsToRotate[i] = _selectedIngredient.AttachedTile.OccupiedIngredients[_selectedIngredient.AttachedTile.OccupiedIngredients.Count - 1 - i];
        }

        foreach (Ingredient ingredient in ingredientsToRotate)
        {
            ingredient.RotateIngredient(direction, _tiles);
        }
    }
}
