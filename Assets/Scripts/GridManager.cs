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

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Tile tile = Instantiate(TilePrefab, TilePrefabParent).GetComponent<Tile>();
                tile.transform.position = new Vector3((i * 2) - vo.GridWidth + 1, tile.transform.position.y, ((vo.GridHeight - j - 1) * 2) - vo.GridHeight + 1);
                tile.TileType = vo.Grid[i * vo.GridHeight + j].TileType;
                tile.SetIndex(j, i);

                if (tile.TileType == TileTypes.Ingredient)
                {
                    tile.SetTileProperties(vo.Grid[i * vo.GridHeight + j].IngredientType, IngredientPrefabs, IngredientPrefabParent);
                }

                _tiles[j, i] = tile;
            }
        }

        GameEvents.Instance.GridCreated(_tiles);
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
