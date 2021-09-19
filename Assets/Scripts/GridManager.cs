using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject TilePrefab;
    public Transform TilePrefabParent;
    public Ingredient[] IngredientPrefabs;
    public Transform IngredientPrefabParent;
    public Ingredient[] ValuedPrefabs;
    public Transform ValuedPrefabParent;
    private Tile[,] _tiles;

    private void Start()
    {
        GameEvents.Instance.OnIngredientPlacedEvent += MergeValuedOnes;
        GameEvents.Instance.OnIngredientPlacedEvent += CheckIfAllIngredientsInOneTile;
        GameEvents.Instance.OnCheckLevelContainerEvent += CreateGrid;
    }

    private void CreateGrid(LevelContainer levelContainer, LevelContainer randomLevelContainerToSaveIn, int index, bool saveRandom)
    {
        _tiles = new Tile[levelContainer.Grids[index].GridWidth, levelContainer.Grids[index].GridHeight];

        if (!levelContainer.Grids[index].MakeLevelValued)
        {
            if (!levelContainer.Grids[index].RandomizeLevels)
            {
                OpenGridFromLevel(levelContainer, index);
            }
            else
            {
                GenerateRandomizedGridAndSaveInContainer(levelContainer, randomLevelContainerToSaveIn, index, saveRandom);
            }
        }
        else
        {
            GenerateRandomizedValuedGridAndSaveInContainer(levelContainer, randomLevelContainerToSaveIn, index, saveRandom);
        }

        GameEvents.Instance.GridCreated(_tiles);
    }

    private void OpenGridFromLevel(LevelContainer levelContainer, int index)
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
                    tile.GenerateIngredientOnTile(IngredientPrefabs[(int)(levelContainer.Grids[index].Grid[i * levelContainer.Grids[index].GridHeight + j].IngredientType)], IngredientPrefabParent);
                }

                _tiles[j, i] = tile;
            }
        }
    }

    private void GenerateRandomizedGridAndSaveInContainer(LevelContainer levelContainer, LevelContainer randomLevelContainerToSaveIn, int index, bool saveRandom)
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
        _tiles[randomFirstBreadRow, randomFirstBreadColumn].GenerateIngredientOnTile(IngredientPrefabs[0], IngredientPrefabParent);

        int[] indexToCheck = { randomFirstBreadRow, randomFirstBreadColumn };
        List<Tile> possibleSecondBreadTiles = CheckAvailableIndexes(indexToCheck);
        int randomSecondBreadIndex = Random.Range(0, possibleSecondBreadTiles.Count);
        possibleSecondBreadTiles[randomSecondBreadIndex].GenerateIngredientOnTile(IngredientPrefabs[0], IngredientPrefabParent);

        List<Tile> ingredientTiles = new List<Tile>();
        List<Tile> possibleIngredientTiles = new List<Tile>();
        for (int i = 0; i < levelContainer.Grids[index].IngredientAmounts; i++)
        {
            int[] ingredientIndex = new int[2];

            if (i == 0)
            {
                ingredientIndex = indexToCheck;
                possibleIngredientTiles = CheckAvailableIndexes(ingredientIndex);
            }
            else
            {
                for (int j = 1; j <= ingredientTiles.Count; j++)
                {
                    ingredientIndex = ingredientTiles[i - j].Index;
                    possibleIngredientTiles = CheckAvailableIndexes(ingredientIndex);

                    if (possibleIngredientTiles.Count > 0)
                    {
                        break;
                    }
                }
            }

            ingredientTiles.Add(possibleIngredientTiles[Random.Range(0, possibleIngredientTiles.Count)]);
            ingredientTiles[i].GenerateIngredientOnTile(IngredientPrefabs[(int)(IngredientTypes)(Random.Range(1, System.Enum.GetValues(typeof(IngredientTypes)).Length))], IngredientPrefabParent);
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

            randomLevelContainerToSaveIn.Grids.Add(gridVO);
        }
    }

    private void GenerateRandomizedValuedGridAndSaveInContainer(LevelContainer levelContainer, LevelContainer randomLevelContainerToSaveIn, int index, bool saveRandom)
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

        int randomFirstRow = Random.Range(0, 4);
        int randomFirstColumn = Random.Range(0, 4);
        _tiles[randomFirstRow, randomFirstColumn].GenerateIngredientOnTile(ValuedPrefabs[0], ValuedPrefabParent);

        int[] indexToCheck = { randomFirstRow, randomFirstColumn };

        List<Tile> valuedTiles = new List<Tile>();
        List<Tile> possibleValuedTiles = new List<Tile>();
        for (int i = 0; i < levelContainer.Grids[index].ValuedAmounts; i++)
        {
            int[] valuedIndex = new int[2];

            if (i == 0)
            {
                valuedIndex = indexToCheck;
                possibleValuedTiles = CheckAvailableIndexes(valuedIndex);
            }
            else
            {
                for (int j = 1; j <= valuedTiles.Count; j++)
                {
                    valuedIndex = valuedTiles[i - j].Index;
                    possibleValuedTiles = CheckAvailableIndexes(valuedIndex);

                    if (possibleValuedTiles.Count > 0)
                    {
                        break;
                    }
                }
            }

            valuedTiles.Add(possibleValuedTiles[Random.Range(0, possibleValuedTiles.Count)]);
            valuedTiles[i].GenerateIngredientOnTile(ValuedPrefabs[i], ValuedPrefabParent);
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
                        tileVO.Value = _tiles[j, i].OccupiedIngredients[0].Value;
                    }

                    gridVO.Grid.Add(tileVO);
                }
            }

            randomLevelContainerToSaveIn.Grids.Add(gridVO);
        }
    }

    private List<Tile> CheckAvailableIndexes(int[] indexToCheck)
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

    private void MergeValuedOnes()
    {
        for (int i = 0; i < _tiles.GetLength(1); i++)
        {
            for (int j = 0; j < _tiles.GetLength(0); j++)
            {
                if (_tiles[j, i].OccupiedIngredients.Count > 1)
                {
                    if (_tiles[j, i].OccupiedIngredients[0].Value > 0)
                    {
                        int value = _tiles[j, i].OccupiedIngredients[0].Value;
                        for (int k = 0; k < _tiles[j, i].OccupiedIngredients.Count; k++)
                        {
                            Destroy(_tiles[j, i].OccupiedIngredients[k].gameObject);
                        }

                        _tiles[j, i].OccupiedIngredients = new List<Ingredient>();
                        _tiles[j, i].GenerateIngredientOnTile(ValuedPrefabs[(int)Mathf.Log(value * 2, 2) - 1], ValuedPrefabParent);
                    }
                }
            }
        }
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
