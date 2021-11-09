using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum TileType
{
    Ground,
    Wall,
    Spawn,
    Goal
}*/

public class Grid
{

    public int Width { get; }
    public int Height { get; }

    public float cellSize;

    public TileType[,] tileTypeArray;

    public Grid(int height, int width, float cellSize, TileType[,] tileTypeArray)
    {
        this.Height = height;
        this.Width = width;
        this.cellSize = cellSize;

        this.tileTypeArray = tileTypeArray;
        /*
        tileTypeArray = new TileType[, ]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        };*/

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                //this.tileType = TileType.Ground;
                //tileTypeArray[i, j] = TileType.Ground;
                //Debug.Log("Center of cell: " + i + ", " + j + " is world position: " + GetWorldPosition(i, j) + " TileType: " + tileTypeArray[i, j]);

                var debug_line_horizontal_start = new Vector3(GetWorldPosition(i, j).x - cellSize / 2, GetWorldPosition(i, j).y, GetWorldPosition(i, j).z - cellSize / 2);
                var debug_line_horizontal_end = new Vector3(GetWorldPosition(i, j + 1).x - cellSize / 2, GetWorldPosition(i, j + 1).y, GetWorldPosition(i, j + 1).z - cellSize / 2);
                var debug_line_vertical_start = new Vector3(GetWorldPosition(i, j).x - cellSize / 2, GetWorldPosition(i, j).y, GetWorldPosition(i, j).z - cellSize / 2);
                var debug_line_vertical_end = new Vector3(GetWorldPosition(i + 1, j).x - cellSize / 2, GetWorldPosition(i + 1, j).y, GetWorldPosition(i + 1, j).z - cellSize / 2);
                Debug.DrawLine(debug_line_horizontal_start, debug_line_horizontal_end, Color.white, 100f);
                Debug.DrawLine(debug_line_vertical_start, debug_line_vertical_end, Color.white, 100f);
            }
        }
        var debug_last_line_horizontal_start = new Vector3(GetWorldPosition(0, height).x - cellSize / 2, GetWorldPosition(0, height).y, GetWorldPosition(0, height).z - cellSize / 2);
        var debug_last_line_horizontal_end = new Vector3(GetWorldPosition(width, height).x - cellSize / 2, GetWorldPosition(width, height).y, GetWorldPosition(width, height).z - cellSize / 2);
        var debug_last_line_vertical_start = new Vector3(GetWorldPosition(width, 0).x - cellSize / 2, GetWorldPosition(width, 0).y, GetWorldPosition(width, 0).z - cellSize / 2);
        var debug_last_line_vertical_end = new Vector3(GetWorldPosition(width, height).x - cellSize / 2, GetWorldPosition(width, height).y, GetWorldPosition(width, height).z - cellSize / 2);
        Debug.DrawLine(debug_last_line_horizontal_start, debug_last_line_horizontal_end, Color.white, 100f);
        Debug.DrawLine(debug_last_line_vertical_start, debug_last_line_vertical_end, Color.white, 100f);

    }

    public Vector3 GetWorldPosition(int i, int j)
    {
        return new Vector3(i, 0, j) * cellSize;
    }

    public TileType TypeAt(int i, int j)
    {
        return tileTypeArray[i, j];
    }

    /*void OnDrawGizmos()
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                //Debug.Log("Center of cell: " + i + ", " + j + " is world position: " + GetWorldPosition(i, j));
                var debug_line_horizontal_start = new Vector3(GetWorldPosition(i, j).x - cellSize / 2, GetWorldPosition(i, j).y, GetWorldPosition(i, j).z - cellSize / 2);
                var debug_line_horizontal_end = new Vector3(GetWorldPosition(i, j + 1).x - cellSize / 2, GetWorldPosition(i, j + 1).y, GetWorldPosition(i, j + 1).z - cellSize / 2);
                var debug_line_vertical_start = new Vector3(GetWorldPosition(i, j).x - cellSize / 2, GetWorldPosition(i, j).y, GetWorldPosition(i, j).z - cellSize / 2);
                var debug_line_vertical_end = new Vector3(GetWorldPosition(i + 1, j).x - cellSize / 2, GetWorldPosition(i + 1, j).y, GetWorldPosition(i + 1, j).z - cellSize / 2);
                Debug.DrawLine(debug_line_horizontal_start, debug_line_horizontal_end, Color.white, 100f);
                Debug.DrawLine(debug_line_vertical_start, debug_line_vertical_end, Color.white, 100f);
            }
        }
        var debug_last_line_horizontal_start = new Vector3(GetWorldPosition(0, height).x - cellSize / 2, GetWorldPosition(0, height).y, GetWorldPosition(0, height).z - cellSize / 2);
        var debug_last_line_horizontal_end = new Vector3(GetWorldPosition(width, height).x - cellSize / 2, GetWorldPosition(width, height).y, GetWorldPosition(width, height).z - cellSize / 2);
        var debug_last_line_vertical_start = new Vector3(GetWorldPosition(width, 0).x - cellSize / 2, GetWorldPosition(width, 0).y, GetWorldPosition(width, 0).z - cellSize / 2);
        var debug_last_line_vertical_end = new Vector3(GetWorldPosition(width, height).x - cellSize / 2, GetWorldPosition(width, height).y, GetWorldPosition(width, height).z - cellSize / 2);
        Debug.DrawLine(debug_last_line_horizontal_start, debug_last_line_horizontal_end, Color.white, 100f);
        Debug.DrawLine(debug_last_line_vertical_start, debug_last_line_vertical_end, Color.white, 100f);
    }*/
}
