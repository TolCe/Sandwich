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

    private void CreateGrid(LevelContainer levelContainer, LevelContainer randomLevelContainer, int index, bool saveRandom)
    {
        _tiles = new Tile[levelContainer.Grids[index].GridWidth, levelContainer.Grids[index].GridHeight];

        if (!levelContainer.Grids[index].randomizeLevels)
        {
            for (int i = 0; i < levelContainer.Grids[index].GridWidth; i++)
            {
                for (int j = 0; j < levelContainer.Grids[index].GridHeight; j++)
                {
                    Tile tile = Instantiate(TilePrefab, TilePrefabParent).GetComponent<Tile>();
                    tile.transform.position = new Vector3((i * 2) - levelContainer.Grids[index].GridWidth + 1, tile.transform.position.y, ((levelContainer.Grids[index].GridHeight - j - 1) * 2) - levelContainer.Grids[index].GridHeight + 1);
                    tile.SetIndex(j, i);

                    if (levelContainer.Grids[index].Grid[i * levelContainer.Grids[index].GridHeight + j].TileType == TileTypes.Ingredient)
                    {
                        tile.SetTileProperties(levelContainer.Grids[index].Grid[i * levelContainer.Grids[index].GridHeight + j].IngredientType, IngredientPrefabs, IngredientPrefabParent);
                    }

                    _tiles[j, i] = tile;
                }
            }
        }
        else
        {
            GridVO gridVO = new GridVO();

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

            int randomFirstBreadRow = Random.Range(0, 4);
            int randomFirstBreadColumn = Random.Range(0, 4);
            _tiles[randomFirstBreadRow, randomFirstBreadColumn].SetTileProperties(IngredientTypes.Bread, IngredientPrefabs, IngredientPrefabParent);

            int[] indexToCheck = { randomFirstBreadRow, randomFirstBreadColumn };
            List<Tile> possibleSecondBreadTiles = FillAvailableIndexes(indexToCheck);
            int randomSecondBreadIndex = Random.Range(0, possibleSecondBreadTiles.Count);
            possibleSecondBreadTiles[randomSecondBreadIndex].SetTileProperties(IngredientTypes.Bread, IngredientPrefabs, IngredientPrefabParent);

            List<Tile> ingredientTiles = new List<Tile>();
            List<Tile> possibleIngredientTiles = new List<Tile>();
            for (int i = 0; i < levelContainer.Grids[index].IngredientAmounts; i++)
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

            if (saveRandom)
            {
                gridVO.Grid = new List<TileVO>();

                for (int i = 0; i < levelContainer.Grids[index].GridWidth; i++)
                {
                    for (int j = 0; j < levelContainer.Grids[index].GridHeight; j++)
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

                randomLevelContainer.Grids.Add(gridVO);
            }
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
