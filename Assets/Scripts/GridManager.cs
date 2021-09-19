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
    public RandomizedLevelContainer RandomizedLevelContainer;

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
            GridVO gridVO = new GridVO();

            for (int i = 0; i < vo.GridWidth; i++)
            {
                for (int j = 0; j < vo.GridHeight; j++)
                {
                    Tile tile = Instantiate(TilePrefab, TilePrefabParent).GetComponent<Tile>();
                    tile.transform.position = new Vector3((i * 2) - 3, tile.transform.position.y, ((3 - j) * 2) - 3);
                    tile.SetIndex(j, i);
                    _tiles[j, i] = tile;
                }
            }

            int randomFirstBreadRow = Random.Range(0, vo.GridHeight);
            int randomFirstBreadColumn = Random.Range(0, vo.GridWidth);
            _tiles[randomFirstBreadRow, randomFirstBreadColumn].SetTileProperties(IngredientTypes.Bread, IngredientPrefabs, IngredientPrefabParent);

            int[] indexToCheck = { randomFirstBreadRow, randomFirstBreadColumn };
            List<Tile> possibleSecondBreadTiles = FillAvailableIndexes(indexToCheck);
            int randomSecondBreadIndex = Random.Range(0, possibleSecondBreadTiles.Count);
            possibleSecondBreadTiles[randomSecondBreadIndex].SetTileProperties(IngredientTypes.Bread, IngredientPrefabs, IngredientPrefabParent);

            List<Tile> ingredientTiles = new List<Tile>();
            List<Tile> possibleIngredientTiles = new List<Tile>();
            for (int i = 0; i < vo.IngredientAmounts; i++)
            {
                int[] ingredientIndex = new int[2];

                if (i == 0)
                {
                    ingredientIndex = indexToCheck;
                    possibleIngredientTiles = FillAvailableIndexes(ingredientIndex);
                }
                else
                {
                    for (int j = 1; j <= ingredientTiles.Count; j++)
                    {
                        ingredientIndex = ingredientTiles[i - j].Index;
                        possibleIngredientTiles = FillAvailableIndexes(ingredientIndex);

                        if (possibleIngredientTiles.Count > 0)
                        {
                            break;
                        }
                    }
                }

                ingredientTiles.Add(possibleIngredientTiles[Random.Range(0, possibleIngredientTiles.Count)]);
                ingredientTiles[i].SetTileProperties((IngredientTypes)(Random.Range(1, System.Enum.GetValues(typeof(IngredientTypes)).Length)), IngredientPrefabs, IngredientPrefabParent);
            }

            gridVO.Grid = new List<TileVO>();

            for (int i = 0; i < vo.GridWidth; i++)
            {
                for (int j = 0; j < vo.GridHeight; j++)
                {
                    TileVO tileVO = new TileVO();
                    tileVO.TileType = _tiles[j, i].TileType;

                    if (_tiles[j, i].OccupiedIngredients.Count > 0)
                    {
                        tileVO.IngredientType = _tiles[j, i].OccupiedIngredients[0].IngredientType;
                    }

                    gridVO.Grid.Add(tileVO);
                }
            }

            RandomizedLevelContainer.Grids.Add(gridVO);
        }

        GameEvents.Instance.GridCreated(_tiles);
    }

    private List<Tile> FillAvailableIndexes(int[] indexToCheck)
    {
        List<Tile> availableIndexes = new List<Tile>();

        if (indexToCheck[0] > 0)
        {
            if (_tiles[indexToCheck[0] - 1, indexToCheck[1]].TileType == TileTypes.Empty)
            {
                availableIndexes.Add(_tiles[indexToCheck[0] - 1, indexToCheck[1]]);
            }
        }
        if (indexToCheck[0] < _tiles.GetLength(0) - 1)
        {
            if (_tiles[indexToCheck[0] + 1, indexToCheck[1]].TileType == TileTypes.Empty)
            {
                availableIndexes.Add(_tiles[indexToCheck[0] + 1, indexToCheck[1]]);
            }
        }
        if (indexToCheck[1] > 0)
        {
            if (_tiles[indexToCheck[0], indexToCheck[1] - 1].TileType == TileTypes.Empty)
            {
                availableIndexes.Add(_tiles[indexToCheck[0], indexToCheck[1] - 1]);
            }
        }
        if (indexToCheck[1] < _tiles.GetLength(1) - 1)
        {
            if (_tiles[indexToCheck[0], indexToCheck[1] + 1].TileType == TileTypes.Empty)
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
                GameEvents.Instance.LevelSucceded();
            }
            else
            {
                GameEvents.Instance.LevelFailed();
            }
        }
    }
}
