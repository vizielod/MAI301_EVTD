using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulator;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject Wall;
    public GameObject Ground;
    public GameObject Spawn;
    public GameObject Goal;

    public GameObject enemy;
    private Grid grid;

    public int[,] gridArray = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 3, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        };

    public TileType[,] tileTypeArray;
    IStateSequence sim;

    void SetTileTypeArray()
    {
        tileTypeArray = new TileType[gridArray.GetLength(0), gridArray.GetLength(1)];
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                tileTypeArray[i, j] = (TileType)gridArray[i, j];
            }
        }
    }
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        SetTileTypeArray();
        grid = new Grid(tileTypeArray.GetLength(0), tileTypeArray.GetLength(1), 5, tileTypeArray); // int rowsOrHeight = ary.GetLength(0); int colsOrWidth = ary.GetLength(1);
        InitializeGridTiles();
        sim = new SimulatorFactory().CreateSimulator(grid);
        enemy.transform.position = new Vector3(5, 1, 5);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StepForward();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            StepBackward();
        }
    }

    void StepForward()
    {
        //enemy.GetComponent<EnemyMovementController>().PerformStepForward();
        sim.StepForward();
        IState state = sim.GetCurrentStep();
        IAgent agent = state.Agents.First();
        (int x, int y) = state.PositionOf(agent);
        enemy.transform.position = new Vector3(x * 5, 1, y * 5);
    }

    void StepBackward()
    {
        //enemy.GetComponent<EnemyMovementController>().PerformStepBackward();
        sim.StepBackward();
        IState state = sim.GetCurrentStep();
        IAgent agent = state.Agents.First();
        (int x, int y) = state.PositionOf(agent);
        enemy.transform.position = new Vector3(x * 5, 1, y * 5);
    }

    void InitializeGridTiles()
    {
        if (grid == null)
            return;

        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                Debug.Log(grid.TypeAt(i, j));
                InstantiateGridTile(grid.TypeAt(i, j), i, j);
            }
        }
    }

    void InstantiateGridTile(TileType tileType, int i, int j)
    {
        GameObject tile;
        Transform parent;

        switch (tileType)
        {
            case TileType.Ground:
                tile = Ground;
                parent = transform.Find("Ground");
                break;
            case TileType.Spawn:
                tile = Spawn;
                parent = transform;
                break;
            case TileType.Goal:
                tile = Goal;
                parent = transform;
                break;
            case TileType.Wall:
                tile = Wall;
                parent = transform.Find("Tiles");
                break;
            default:
                tile = Wall;
                parent = transform.Find("Tiles");
                break;
        }

        Transform newTile = (Instantiate(tile, new Vector3(i * grid.cellSize, 0, j * grid.cellSize), Quaternion.identity) as GameObject).transform;

        newTile.SetParent(parent);
    }

    void OnDrawGizmos()
    {
        if(grid == null)
        {
            return;
        }
        for (int i = 0; i < grid.tileTypeArray.GetLength(0); i++)
        {
            for (int j = 0; j < grid.tileTypeArray.GetLength(1); j++)
            {
                var debug_line_horizontal_start = new Vector3(grid.GetWorldPosition(i, j).x - grid.cellSize / 2, grid.GetWorldPosition(i, j).y, grid.GetWorldPosition(i, j).z - grid.cellSize / 2);
                var debug_line_horizontal_end = new Vector3(grid.GetWorldPosition(i, j + 1).x - grid.cellSize / 2, grid.GetWorldPosition(i, j + 1).y, grid.GetWorldPosition(i, j + 1).z - grid.cellSize / 2);
                var debug_line_vertical_start = new Vector3(grid.GetWorldPosition(i, j).x - grid.cellSize / 2, grid.GetWorldPosition(i, j).y, grid.GetWorldPosition(i, j).z - grid.cellSize / 2);
                var debug_line_vertical_end = new Vector3(grid.GetWorldPosition(i + 1, j).x - grid.cellSize / 2, grid.GetWorldPosition(i + 1, j).y, grid.GetWorldPosition(i + 1, j).z - grid.cellSize / 2);
                Debug.DrawLine(debug_line_horizontal_start, debug_line_horizontal_end, Color.white, 100f);
                Debug.DrawLine(debug_line_vertical_start, debug_line_vertical_end, Color.white, 100f);
            }
        }
        var debug_last_line_horizontal_start = new Vector3(grid.GetWorldPosition(0, grid.Height).x - grid.cellSize / 2, grid.GetWorldPosition(0, grid.Height).y, grid.GetWorldPosition(0, grid.Height).z - grid.cellSize / 2);
        var debug_last_line_horizontal_end = new Vector3(grid.GetWorldPosition(grid.Width, grid.Height).x - grid.cellSize / 2, grid.GetWorldPosition(grid.Width, grid.Height).y, grid.GetWorldPosition(grid.Width, grid.Height).z - grid.cellSize / 2);
        var debug_last_line_vertical_start = new Vector3(grid.GetWorldPosition(grid.Width, 0).x - grid.cellSize / 2, grid.GetWorldPosition(grid.Width, 0).y, grid.GetWorldPosition(grid.Width, 0).z - grid.cellSize / 2);
        var debug_last_line_vertical_end = new Vector3(grid.GetWorldPosition(grid.Width, grid.Height).x - grid.cellSize / 2, grid.GetWorldPosition(grid.Width, grid.Height).y, grid.GetWorldPosition(grid.Width, grid.Height).z - grid.cellSize / 2);
        Debug.DrawLine(debug_last_line_horizontal_start, debug_last_line_horizontal_end, Color.white, 100f);
        Debug.DrawLine(debug_last_line_vertical_start, debug_last_line_vertical_end, Color.white, 100f);
    }
}
