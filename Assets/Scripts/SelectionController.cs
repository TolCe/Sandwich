using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public Ingredient SelectedIngredient;
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
        SelectedIngredient = selectedIngredient;
    }

    public void RotateIngredient(Vector3 direction)
    {
        Ingredient[] ingredientsToRotate = new Ingredient[SelectedIngredient.AttachedTile.OccupiedIngredients.Count];
        for (int i = 0; i < ingredientsToRotate.Length; i++)
        {
            ingredientsToRotate[i] = SelectedIngredient.AttachedTile.OccupiedIngredients[SelectedIngredient.AttachedTile.OccupiedIngredients.Count - 1 - i];
        }

        foreach (Ingredient ingredient in ingredientsToRotate)
        {
            ingredient.RotateIngredient(direction, _tiles);
        }
    }
}
