using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GridVO
{
    [EnumToggleButtons] public TileTypes SelectedTileType;

    [OnValueChanged("CreateGrid")]
    public int GridWidth = 4;
    [OnValueChanged("CreateGrid")]
    public int GridHeight = 4;
    public bool randomizeLevels;
    [ShowIf("randomizeLevels")]
    public int IngredientAmounts = 1;

    [SerializeField] [HideInInspector] public List<TileVO> Grid;

#if UNITY_EDITOR
    [ShowInInspector]
    [TableMatrix(SquareCells = true, DrawElementMethod = "DrawElement")]
    private TileVO[,] _editorGrid;
    [SerializeField] private TileVO _insertationTileData;

    private TileVO DrawElement(Rect rect, TileVO tile)
    {
        switch (tile.TileType)
        {
            case TileTypes.Empty:
                EditorGUI.DrawRect(rect.Padding(1), Color.white);
                break;
            case TileTypes.Ingredient:
                EditorGUI.DrawRect(rect.Padding(1), Color.blue);
                break;
            default:
                break;
        }

        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            if (Event.current.button == 0)
            {
                tile.TileType = SelectedTileType;
            }
            else if (Event.current.button == 1)
            {
                if (tile.TileType == TileTypes.Ingredient)
                {
                    tile.IngredientType = (IngredientTypes)(0);
                }
                else
                {
                    tile.TileType = TileTypes.Empty;
                }
            }
            else if (Event.current.button == 2)
            {
                if (tile.TileType == TileTypes.Ingredient)
                {
                    if ((int)tile.IngredientType == System.Enum.GetValues(typeof(IngredientTypes)).Length - 1)
                    {
                        tile.IngredientType = (IngredientTypes)(0);
                    }
                    else
                    {
                        tile.IngredientType = (IngredientTypes)((int)tile.IngredientType + 1);
                    }
                }
                else
                {
                    Debug.Log(Grid.IndexOf(tile));
                }
            }

            Serialize();
            _insertationTileData = tile;
            GUI.changed = true;
            Event.current.Use();
        }

        if (tile.TileType == TileTypes.Ingredient)
        {
            var labelStyle = EditorStyles.label;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 16;
            var newRect = rect.SetSize(rect.size * 0.4f);
            newRect.center = rect.center;
            EditorGUI.DrawRect(newRect, Color.black);
            EditorGUI.LabelField(rect, tile.IngredientType + "");
        }

        return tile;
    }

    private void CreateGrid()
    {
        Grid = new List<TileVO>();
        _editorGrid = new TileVO[GridWidth, GridHeight];

        for (int i = 0; i < GridHeight * GridWidth; i++)
        {
            Grid.Add(new TileVO
            {
                TileType = TileTypes.Empty,
            });
        }

        Deserialize();
    }

    [OnInspectorInit]
    private void OnInit()
    {
        _editorGrid = new TileVO[GridWidth, GridHeight];
        Deserialize();
    }

    private void Serialize()
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                Grid[i * GridHeight + j] = _editorGrid[i, j];
            }
        }
    }
    private void Deserialize()
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                _editorGrid[i, j] = Grid[i * GridHeight + j];
            }
        }
    }
#endif
}
