using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject TilePrefab;
    public Transform TilePrefabParent;
    public GameObject[] IngredientPrefabs;
    public Transform IngredientPrefabParent;
    private Tile[,] _tiles;

    private void Start()
    {
        GameEvents.Instance.OnIngredientPlacedEvent += CheckIfAllIngredientsInOneTile;
        GameEvents.Instance.OnCheckLevelContainerEvent += CreateGrid;
    }

    private void CreateGrid(GridVO vo)
    {
        _tiles = new Tile[vo.GridWidth, vo.GridHeight];

        if (!vo.randomizeLevels)
        {
            for (int i = 0; i < vo.GridWidth; i++)
            {
                for (int j = 0; j < vo.GridHeight; j++)
                {
                    Tile tile = Instantiate(TilePrefab, TilePrefabParent).GetComponent<Tile>();
                    tile.transform.position = new Vector3((i * 2) - vo.GridWidth + 1, tile.transform.position.y, ((vo.GridHeight - j - 1) * 2) - vo.GridHeight + 1);
                    tile.SetIndex(j, i);

                    if (vo.Grid[i * vo.GridHeight + j].TileType == TileTypes.Ingredient)
                    {
                        tile.SetTileProperties(vo.Grid[i * vo.GridHeight + j].IngredientType, IngredientPrefabs, IngredientPrefabParent);
                    }

                    _tiles[j, i] = tile;
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Tile tile = Instantiate(TilePrefab, TilePrefabParent).GetComponent<Tile>();
                    tile.transform.position = new Vector3((i * 2) - 3, tile.transform.position.y, ((3 - j) * 2) - 3);
                    tile.SetIndex(j, i);
                    _tiles[j, i] = tile;
                }
            }

            int randomFirstBreadRow = Random.Range(0, vo.GridHeight);
            int randomFirstBreadColumn = Random.Range(0, vo.GridWidth);
            int[] indexToCheck = { randomFirstBreadRow, randomFirstBreadColumn };
            List<Tile> possibleSecondBreadTiles = FillAvailableIndexes(indexToCheck);

            _tiles[randomFirstBreadRow, randomFirstBreadColumn].SetTileProperties(IngredientTypes.Bread, IngredientPrefabs, IngredientPrefabParent);
            int randomSecondBreadIndex = Random.Range(0, possibleSecondBreadTiles.Count);
            possibleSecondBreadTiles[randomSecondBreadIndex].SetTileProperties(IngredientTypes.Bread, IngredientPrefabs, IngredientPrefabParent);
            possibleSecondBreadTiles.RemoveAt(randomSecondBreadIndex);

            List<Tile> ingredientTiles = new List<Tile>();

            for (int i = 0; i < vo.IngredientAmounts; i++)
            {
                int[] ingredientIndex = new int[2];

                if (i == 0)
                {
                    ingredientIndex[0] = randomFirstBreadRow;
                    ingredientIndex[1] = randomFirstBreadColumn;
                }
                else
                {
                    ingredientIndex[0] = ingredientTiles[i - 1].Index[0];
                    ingredientIndex[1] = ingredientTiles[i - 1].Index[1];
                }

                List<Tile> possibleIngredientTiles = FillAvailableIndexes(ingredientIndex);
                possibleIngredientTiles[Random.Range(0, possibleIngredientTiles.Count)].SetTileProperties((IngredientTypes)(Random.Range(1, System.Enum.GetValues(typeof(IngredientTypes)).Length)), IngredientPrefabs, IngredientPrefabParent);
                ingredientTiles.Add(possibleIngredientTiles[Random.Range(0, possibleIngredientTiles.Count)]);
            }
        }

        GameEvents.Instance.GridCreated(_tiles);
    }

    private List<Tile> FillAvailableIndexes(int[] indexToCheck)
    {
        List<Tile> availableIndexes = new List<Tile>();

        if (indexToCheck[0] > 0)
        {
            if (_tiles[indexToCheck[0] - 1, indexToCheck[1]].OccupiedIngredients.Count == 0)
            {
                availableIndexes.Add(_tiles[indexToCheck[0] - 1, indexToCheck[1]]);
            }
        }
        if (indexToCheck[0] < _tiles.GetLength(0) - 1)
        {
            if (_tiles[indexToCheck[0] + 1, indexToCheck[1]].OccupiedIngredients.Count == 0)
            {
                availableIndexes.Add(_tiles[indexToCheck[0] + 1, indexToCheck[1]]);
            }
        }
        if (indexToCheck[1] > 0)
        {
            if (_tiles[indexToCheck[0], indexToCheck[1] - 1].OccupiedIngredients.Count == 0)
            {
                availableIndexes.Add(_tiles[indexToCheck[0], indexToCheck[1] - 1]);
            }
        }
        if (indexToCheck[1] < _tiles.GetLength(1) - 1)
        {
            if (_tiles[indexToCheck[0], indexToCheck[1] + 1].OccupiedIngredients.Count == 0)
            {
                availableIndexes.Add(_tiles[indexToCheck[0], indexToCheck[1] + 1]);
            }
        }

        return availableIndexes;
    }

    private void CheckIfAllIngredientsInOneTile()
    {
        List<Tile> occupiedTileCount = new List<Tile>();

        for (int i = 0; i < _tiles.GetLength(1); i++)
        {
            for (int j = 0; j < _tiles.GetLength(0); j++)
            {
                if (_tiles[j, i].TileType != TileTypes.Empty)
                {
                    occupiedTileCount.Add(_tiles[j, i]);
                }
            }
        }

        if (occupiedTileCount.Count == 1)
        {
            if (occupiedTileCount[0].OccupiedIngredients[0].IngredientType == IngredientTypes.Bread && occupiedTileCount[0].OccupiedIngredients[occupiedTileCount[0].OccupiedIngredients.Count - 1].IngredientType == IngredientTypes.Bread)
            {
                GameEvents.Instance.LevelCompleted(true, 1);
            }
            else
            {
                GameEvents.Instance.LevelCompleted(false, 1);
            }
        }
    }
}
