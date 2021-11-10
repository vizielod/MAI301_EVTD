using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulator;
using System.Linq;

public class Grid : IMapLayout
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

    }

    public Vector3 GetWorldPosition(int i, int j)
    {
        return new Vector3(i, 0, j) * cellSize;
    }

    public TileType TypeAt(int x, int y)
    {
        return tileTypeArray[x, y];
    }

    public (int x, int y) GetSpawnPoint()
    {
        return (1, 1);
    }

    public IEnumerator<TileType> GetEnumerator()
    {
        for (int i = 0; i < tileTypeArray.GetLength(0); i++)
        {
            for (int j = 0; j < tileTypeArray.GetLength(1); j++)
            {
                yield return tileTypeArray[i, j];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return tileTypeArray.GetEnumerator();
    }
}
