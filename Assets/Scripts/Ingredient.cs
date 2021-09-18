using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public IngredientContainer IngredientContainer;
    public IngredientTypes IngredientType;
    public int[] CurrentIndexOnGrid;
    public Tile AttachedTile;

    private void Start()
    {
        IngredientType = IngredientContainer.Ingredient.IngredientType;
    }

    public void RotateIngredient(Vector3 direction, Tile[,] tiles)
    {
        Vector3 rotateDirection = Vector3.zero;
        Vector3 rotatePoint = Vector3.zero;
        int rowDirection = 0, columnDirection = 0;

        if (direction == Vector3.up)
        {
            rotatePoint = tiles[CurrentIndexOnGrid[0], CurrentIndexOnGrid[1]].transform.position + Vector3.forward;
            rotateDirection = Vector3.left;
            rowDirection = -1;
        }
        else if (direction == Vector3.down)
        {
            rotatePoint = tiles[CurrentIndexOnGrid[0], CurrentIndexOnGrid[1]].transform.position - Vector3.forward;
            rotateDirection = Vector3.right;
            rowDirection = 1;
        }
        else if (direction == Vector3.right)
        {
            rotatePoint = tiles[CurrentIndexOnGrid[0], CurrentIndexOnGrid[1]].transform.position + Vector3.right;
            rotateDirection = Vector3.forward;
            columnDirection = 1;
        }
        else if (direction == Vector3.left)
        {
            rotatePoint = tiles[CurrentIndexOnGrid[0], CurrentIndexOnGrid[1]].transform.position - Vector3.right;
            rotateDirection = Vector3.back;
            columnDirection = -1;
        }

        rotatePoint.y = transform.position.y;

        if (CurrentIndexOnGrid[0] + rowDirection < 0 || CurrentIndexOnGrid[0] + rowDirection > tiles.GetLength(0) - 1 || CurrentIndexOnGrid[1] + columnDirection < 0 || CurrentIndexOnGrid[1] + columnDirection > tiles.GetLength(1) - 1)
        {
            return;
        }
        if (tiles[CurrentIndexOnGrid[0] + rowDirection, CurrentIndexOnGrid[1] + columnDirection] == null || tiles[CurrentIndexOnGrid[0] + rowDirection, CurrentIndexOnGrid[1] + columnDirection].TileType == TileTypes.Empty)
        {
            return;
        }

        StartCoroutine(RotateTo(rotatePoint, rotateDirection, 175f, 0, tiles, rowDirection, columnDirection));
    }

    private IEnumerator RotateTo(Vector3 rotatePoint, Vector3 direction, float rotateAmount, float waitAmount, Tile[,] tiles, int rowDirection, int columnDirection)
    {
        yield return new WaitForSecondsRealtime(waitAmount);

        float timer = 0;
        while (timer < 0.4f)
        {
            transform.RotateAround(rotatePoint, direction, -rotateAmount / 0.4f * Time.fixedDeltaTime);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        AttachedTile.RemoveFromTile(this);
        CurrentIndexOnGrid[0] += rowDirection;
        CurrentIndexOnGrid[1] += columnDirection;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        tiles[CurrentIndexOnGrid[0], CurrentIndexOnGrid[1]].AddToTile(this);
        transform.position = AttachedTile.transform.position + AttachedTile.OccupiedIngredients.Count * 0.1f * Vector3.up;
        GameEvents.Instance.IngredientPlaced();
    }
}

public enum IngredientTypes
{
    Bread,
    Bacon,
    Cheese,
    Egg,
    Ham,
    Onion,
    Salad,
    Salami,
    Tomato
}
