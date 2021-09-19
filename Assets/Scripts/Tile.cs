using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileTypes TileType;

    [HideInInspector] public int[] Index;
    public List<Ingredient> OccupiedIngredients;

    private void Awake()
    {
        OccupiedIngredients = new List<Ingredient>();
    }

    public void SetIndex(int row, int column)
    {
        Index = new int[2];
        Index[0] = row;
        Index[1] = column;
    }

    public void GenerateIngredientOnTile(Ingredient ingredientPrefab, Transform ingredientPrefabParent)
    {
        TileType = TileTypes.Ingredient;
        GameObject ingredient = Instantiate(ingredientPrefab.gameObject, ingredientPrefabParent);
        Ingredient instantiatedingredient = ingredient.GetComponent<Ingredient>();
        OccupiedIngredients.Add(instantiatedingredient);
        ingredient.transform.position = transform.position + OccupiedIngredients.Count * 0.1f * Vector3.up;
        instantiatedingredient.CurrentIndexOnGrid = Index;
        instantiatedingredient.AttachedTile = this;
    }

    public void AddToTile(Ingredient ingredient)
    {
        OccupiedIngredients.Add(ingredient);
        ingredient.AttachedTile = this;
    }
    public void RemoveFromTile(Ingredient ingredient)
    {
        OccupiedIngredients.Remove(ingredient);

        if (OccupiedIngredients.Count == 0)
        {
            TileType = TileTypes.Empty;
        }
    }
}

public enum TileTypes
{
    Empty,
    Ingredient,
}
