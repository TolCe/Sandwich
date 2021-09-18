using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileTypes TileType;

    private int[] _index;
    private GameObject[] _ingredientPrefabs;
    public List<Ingredient> OccupiedIngredients;

    private void Awake()
    {
        OccupiedIngredients = new List<Ingredient>();
    }

    public void SetIndex(int row, int column)
    {
        _index = new int[2];
        _index[0] = row;
        _index[1] = column;
    }

    public void SetTileProperties(IngredientTypes ingredientType, GameObject[] ingredientPrefabs, Transform ingredientPrefabParent)
    {
        _ingredientPrefabs = ingredientPrefabs;
        GameObject ingredient = Instantiate(_ingredientPrefabs[(int)ingredientType], ingredientPrefabParent);
        Ingredient instantiatedingredient = ingredient.GetComponent<Ingredient>();
        OccupiedIngredients.Add(instantiatedingredient);
        ingredient.transform.position = transform.position + OccupiedIngredients.Count * 0.1f * Vector3.up;
        instantiatedingredient.CurrentIndexOnGrid = _index;
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
