using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCell : MonoBehaviour
{
    Vector2Int coordinates;
    Vector2 size;
    Color cellColor;
    bool isEmpty;

    public void Instantiate(int x, int y, Vector2 cellSize)
    {
        size = cellSize;
        coordinates = new Vector2Int(x,y);
        transform.position = new Vector3(size.x * coordinates.x, 0, size.y * coordinates.y);
        cellColor = Color.black;
        isEmpty = true;
    }

    public Color GetColor()
    {
        return cellColor;
    }
    public void SetColor(Color newColor)
    {
        cellColor = newColor;
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }

    public void Fill()
    {
        isEmpty = false;
    }
}
