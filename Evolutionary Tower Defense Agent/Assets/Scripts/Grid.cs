using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulator;
using System;

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

    public (int x, int y) Translate(Direction direction)
    {
        return direction switch
        {
            Direction.North => (-1, 0),
            Direction.South => (1, 0),
            Direction.East => (0, 1),
            Direction.West => (0, -1),
            _ => (0, 0)
        };
    }

    public Direction Translate(int x, int y)
    {
        (int x, int y) rel = (Math.Sign(x), Math.Sign(y));
        return rel switch
        {
            (-1, 0) => Direction.North,
            (1, 0) => Direction.South,
            (0, 1) => Direction.East,
            (0, -1) => Direction.West,
            (-1, -1) => Direction.North | Direction.West,
            (-1, 1) => Direction.North | Direction.East,
            (1, -1) => Direction.South | Direction.West,
            (1, 1) => Direction.South | Direction.East,
            _ => 0
        };
    }

    public bool InBounds(int x, int y)
    {
        return x >= 0 && x < tileTypeArray.GetLength(0) &&
            y >= 0 && y < tileTypeArray.GetLength(1);
    }
}