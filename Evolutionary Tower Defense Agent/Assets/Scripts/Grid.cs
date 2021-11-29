using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulator;
using System.Linq;

public class Grid : IMapLayout
{

    public int Width { get; }
    public int Height { get; }

    public float tileSize;

    public TileType[,] tileTypeArray;

    public (int x, int y) Spawn { get; set; }

    public (int x, int y) Goal { get; set; }

    public Grid(int height, int width, int tileSize, TileType[,] tileTypeArray)
    {
        Height = height;
        Width = width;
        this.tileSize = tileSize;

        this.tileTypeArray = tileTypeArray;
        SavePointsOfInterests();
    }

    private void SavePointsOfInterests()
    {
        for (int i = 0; i < tileTypeArray.GetLength(0); i++)
        {
            for (int j = 0; j < tileTypeArray.GetLength(1); j++)
            {
                if (tileTypeArray[i, j] == TileType.Spawn)
                    Spawn = (i, j);
                else if (tileTypeArray[i, j] == TileType.Goal)
                    Goal = (i, j);
            }
        }
    }

    public Vector3 GetWorldPosition(int i, int j)
    {
        return new Vector3(i, 0, j) * tileSize;
    }

    public TileType TypeAt(int x, int y)
    {
        return tileTypeArray[x, y];
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
