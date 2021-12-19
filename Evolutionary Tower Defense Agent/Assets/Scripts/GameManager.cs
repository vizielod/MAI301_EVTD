using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulator;
using System.Linq;
using BehaviorTree;
using BehaviorTree.Agents;
using Evolution;
using System.Threading.Tasks;
using UnityEngine.UI;

public enum GridType { 
    useGridWithTurretsSetup = 0, 
    useGridWithoutTurretsSetup = 1, 
    useGridMultiLineWithoutTurretsSetup = 2,
    useGridComplexWithoutTurretsSetup = 3,
    useSplitlaneWithoutTurretsSetup = 4,
    useSimpleWithoutTurretsSetup = 5
}
public class GameManager : MonoBehaviour
{
    [Header("Game State & UI")]
    public UIManager uiManager;
    public PlayerStats playerStats;
    public bool gameOver = false;
    public Graph graph;
    public TreeVisualizer treeVisualizer;
    public loadingtext loading;

    [Header("Prefabs")]
    public GameObject Wall;
    public GameObject Ground;
    public GameObject Spawn;
    public GameObject Goal;
    public GameObject Turret;
    public GameObject enemyPrefab;

    [Header("Variables")]
    public int tileSize = 5;
    public int numberOfEnemies = 5;
    public int numberOfGenerations = 50;
    public int numberOfTurrets = 0;
    [Range(0f, 1f)] public float mutationRate = 0.5f;
    [Range(0f, 1f)] public float eliteRate = 0.02f;
    [Range(0f, 1f)] public float roulettRate = 0.5f;
    public bool useZinger = true;
    [Range(1, 10)] public int treeGeneratorRandomizationIterations = 2;
    public bool CrossBreedCompositeNodes = true;
    [Range(0f, 1f)] public float CompositeNodeBias = 0.5f;
    [Range(0f, 1f)] public float ActionsNodeBias = 0.5f;
    //public GameObject enemy;

    [Header("Grid Setup")]
    public Vector3 spawnPosition = new Vector3(5, 2.75f, 5);
    public GridType gridType;
    public Grid grid;
    public int maxTurretCount = 10;
    public int turretCount = 0;

    private List<GameObject> enemyGameObjects;
    private GameObject[] turrets;

    public Dictionary<IAgent, GameObject> agentGODictionary;
    

    private string enemyTag = "Enemy";
    private string turretTag = "Turret";
    private List<IAgent> turretAgents;
    private List<IAgent> enemyAgents;
    private Evolutionary evolutionary;

    private bool agentsInitialized = false;
    private bool runSimulation = false;
    private bool lastGenerationReached = false;

    public int[,] defaultGridArray = new int[,]
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

    public int[,] gridWithTurretsArray = new int[,]
        {
            { 1, 1, 1, 4, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1},
            { 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 4, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 4, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 0, 0, 0, 4, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 4, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 4, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 4, 0, 0, 0, 0, 0, 3, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        };

    public int[,] gridWithoutTurretsArray = new int[,]
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

    public int[,] gridMultiLineWithoutTurretsArray = new int[,]
{
            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 1, 1},
            { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
            { 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 3, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1},
};

    public int[,] gridComplexWithoutTurretsArray = new int[,]
{
            { 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1},
            { 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1},
            { 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1},
            { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
            { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
            { 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
            { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
            { 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 3, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1},
};

    public int[,] gridSplitlaneWithoutTurretsArray = new int[,]
{
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 2, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 3, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
};

public int[,] gridSimpleWithoutTurretsArray = new int[,]
{
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 2, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 3, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
};

    public TileType[,] tileTypeArray;
    IStateSequence sim;

    public int stepCount = 0;
    public float stepTimer = 0f;
    public float maxStepTime = 0.05f;


    void SetTileTypeArray(int[,] gridArray)
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
        turretAgents = new List<IAgent>();
        enemyAgents = new List<IAgent>();
        PlayerStats.remainingTurretcount = maxTurretCount;
    }
    // Start is called before the first frame update
    void Start()
    {
        agentGODictionary = new Dictionary<IAgent, GameObject>();

        /*if(useGridWithTurretsSetup && useGridWithoutTurretsSetup && useGridMultiLineWithoutTurretsSetup ||
            useGridWithTurretsSetup && useGridWithoutTurretsSetup ||
            useGridWithTurretsSetup && useGridMultiLineWithoutTurretsSetup ||
            useGridWithoutTurretsSetup && useGridMultiLineWithoutTurretsSetup)
        {
            Debug.LogError("Make sure only one of useGridWithTurretsSetup and useGridWithoutTurretsSetup is selected!");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
            return;
        }*/
        if (/*useGridWithTurretsSetup*/gridType == GridType.useGridWithTurretsSetup)
        {
            uiManager.HintText.SetActive(false);
            uiManager.RemainingTurrets.SetActive(false);
            uiManager.RemainingTurretCount.SetActive(false);
            SetTileTypeArray(gridWithTurretsArray);
        }
        else if (/*useGridWithoutTurretsSetup*/gridType == GridType.useGridWithoutTurretsSetup)
        {
            SetTileTypeArray(gridWithoutTurretsArray);
        }
        else if (/*useGridMultiLineWithoutTurretsSetup*/gridType == GridType.useGridMultiLineWithoutTurretsSetup)
        {
            SetTileTypeArray(gridMultiLineWithoutTurretsArray);
        }
        else if (gridType == GridType.useGridComplexWithoutTurretsSetup)
        {
            SetTileTypeArray(gridComplexWithoutTurretsArray);
        }
        else if (gridType == GridType.useSplitlaneWithoutTurretsSetup)
        {
            SetTileTypeArray(gridSplitlaneWithoutTurretsArray);
        }
        else if (gridType == GridType.useSimpleWithoutTurretsSetup)
        {
            SetTileTypeArray(gridSimpleWithoutTurretsArray);
        }
        else
        {
            Debug.LogError("Make sure either useGridWithTurretsSetup, useGridWithoutTurretsSetup or useGridMultiLineWithoutTurretsSetup is selected!");
            return;
        }

        grid = new Grid(tileTypeArray.GetLength(0), tileTypeArray.GetLength(1), tileSize, tileTypeArray); // int rowsOrHeight = ary.GetLength(0); int colsOrWidth = ary.GetLength(1);
        InitializeGridTiles();

        /*turrets = GameObject.FindGameObjectsWithTag(turretTag);
        InitializeAgents();*/
    }

    public void Restart()
    {
        RemoveGameObjects();

        turretAgents = new List<IAgent>();
        enemyAgents = new List<IAgent>();
        PlayerStats.remainingTurretcount = maxTurretCount;
        gridWithoutTurretsArray = defaultGridArray;
        agentGODictionary = new Dictionary<IAgent, GameObject>();
        sim = null;
        grid = null;
        tileTypeArray = null;

        turretCount = 0;
        numberOfTurrets = 0;

        if (gridType == GridType.useGridWithTurretsSetup)
        {
            uiManager.HintText.SetActive(false);
            uiManager.RemainingTurrets.SetActive(false);
            uiManager.RemainingTurretCount.SetActive(false);
            SetTileTypeArray(gridWithTurretsArray);
        }
        else if (gridType == GridType.useGridWithoutTurretsSetup)
        {
            SetTileTypeArray(gridWithoutTurretsArray);
        }
        else if (gridType == GridType.useGridMultiLineWithoutTurretsSetup)
        {
            SetTileTypeArray(gridMultiLineWithoutTurretsArray);
        }

        grid = new Grid(tileTypeArray.GetLength(0), tileTypeArray.GetLength(1), tileSize, tileTypeArray); // int rowsOrHeight = ary.GetLength(0); int colsOrWidth = ary.GetLength(1);
        InitializeGridTiles();

        uiManager.Restart();
        playerStats.Restart();
        PlayerStats.remainingTurretcount = maxTurretCount;

        gameOver = false;
    }
    public async void StartGame()
    {
        if (gridType == GridType.useGridWithTurretsSetup)
        {
           
            turrets = GameObject.FindGameObjectsWithTag(turretTag);
            await InitializeAgents();
            return;
        }
        else if(turretCount < maxTurretCount)
        {
            Debug.LogWarning("YOU NEED TO PLACE ALL 10 TURRETS BEFORE YOU START!");
            return;
        }
        else
        {
            //turrets = GameObject.FindGameObjectsWithTag(turretTag);
            await InitializeAgents();
            //WriteGridOnDebug();
        }
        uiManager.StartGamePressed();

        //Instantiate(transform.gameObject, new Vector3(10, 0, 10), Quaternion.identity);

    }

    void WriteGridOnDebug()
    {
        for (int i = 0; i < gridWithoutTurretsArray.GetLength(0); i++)
        {
            string line = "";
            for (int j = 0; j < gridWithoutTurretsArray.GetLength(1); j++)
            {
                line = System.String.Concat(line, " ", gridWithoutTurretsArray[i, j]);
            }
            Debug.Log(line);
        }

        for (int i = 0; i < grid.Height; i++)
        {
            for (int j = 0; j < grid.Width; j++)
            {
                Debug.Log(i + " " + j + " " + grid.tileTypeArray[i, j]);
            }
        }
    }
    async Task InitializeAgents()
    {
        //Vector3 spawnPosition = new Vector3(5, 2.75f, 5);

        if (gridType == GridType.useGridWithTurretsSetup)
        {
            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    if (grid.TypeAt(i, j) == TileType.Turret)
                    {
                        IAgent turretAgent = new TurretAgent((i, j));
                        turretAgents.Add(turretAgent);

                        agentGODictionary.Add(turretAgent, turrets[numberOfTurrets].gameObject);

                        turrets[numberOfTurrets].gameObject.GetComponent<TurretController>().turretAgent = (TurretAgent)turretAgent;

                        numberOfTurrets++;
                    }
                }
            }
        }

        //sim = new SimulatorFactory().CreateSimulator(grid, enemyAgents, turretAgents); // Parse enemies and tower agents

        /*foreach (var sim in new Evolutionary(numberOfEnemies).RunEvolution(grid, turretAgents))
        {
            while (!sim.IsGameOver)
            {
                sim.StepForward();
            }
        }*/

        EvolutionConfiguration config = new EvolutionConfiguration()
        {
            NumberOfGenerations = numberOfGenerations,
            MutationRate = mutationRate,
            PopulationSize = numberOfEnemies,
            EliteRate = eliteRate,
            RoulettRate = roulettRate,
            UseZinger = useZinger,
            TreeGeneratorIterations = treeGeneratorRandomizationIterations,
            CrossComposites = CrossBreedCompositeNodes,
            ConditionalVsActionNodes = ActionsNodeBias,
            LeafVsCompositeNodes = CompositeNodeBias
        };

        TowerDefenceConfiguration towerConfig = new TowerDefenceConfiguration
        {
            Map = grid,
            MaxRounds = 200,
            PlayLifes = playerStats.startLives
        };
        evolutionary = new Evolutionary(config, new TowerDefenceSimulatorFactory(towerConfig, turretAgents));
        var _ = evolutionary.RunEvolutionAsync((result) => 
        {
            Debug.Log($"Score: {result.Score}");
            graph.addValue(result.Score);

            if(evolutionary.CurrentGeneration == numberOfGenerations)
            {
                lastGenerationReached = true;
                graph.LastValueAdded();
            }
        });

    }

    private void InstantiateEnemyAgents(IStateSequence sim)
    {
        enemyAgents = sim.AllEnemyAgents.ToList();

        for (int i = 0; i < enemyAgents.Count; i++)
        {
            GameObject newEnemyGO = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity) as GameObject;
            newEnemyGO.transform.SetParent(transform.Find("Enemies"));

            newEnemyGO.GetComponent<EnemyController>().enemyAgent = (IEnemyAgent)enemyAgents[i];
            newEnemyGO.GetComponent<EnemyController>().enabled = true;
            newEnemyGO.GetComponent<EnemyController>().gameManager = this;

            agentGODictionary.Add(enemyAgents[i], newEnemyGO);
        }
    }
    private IEnumerator AutoSimulateCoroutine(IStateSequence sim, int currentGeneration)
    {
        RemoveEnemyObjects();
        InstantiateEnemyAgents(sim);

        //loading.numberOfSteps = sim.NumberOfRounds;
        Debug.Log("NumberOfRounds: " + sim.NumberOfRounds);
        loading.InitializeLoadingAnimation(sim.NumberOfRounds);

        runSimulation = true;
        while (!sim.IsGameOver)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer > maxStepTime)
            {
                StepForward();
                loading.AnimateLoading();
                stepTimer = 0f;
                Debug.Log("Game is not over yet");
            }
            yield return null;
        }
        if (currentGeneration != numberOfGenerations)
        {
            RemoveEnemyObjects();
            runSimulation = false;
        }
        Debug.Log("Game over");
        yield return true;
    }

    public void AutoSimulate(IStateSequence sim)
    {
        InstantiateEnemyAgents(sim);
        agentsInitialized = true;
        runSimulation = true;
    }

    public void InitializeTurretAgent(int i, int j, GameObject turretGO)
    {
        IAgent turretAgent = new TurretAgent((i, j));
        turretAgents.Add(turretAgent);
        agentGODictionary.Add(turretAgent, turretGO);
        turretGO.GetComponent<TurretController>().turretAgent = (TurretAgent)turretAgent;
        numberOfTurrets++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StepForward();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                StepBackward();
            }
            if (PlayerStats.Lives <= 0)
            {
                EndGame();
            }
            if(evolutionary != null)
            {
                if (evolutionary.NewestSimulation != null && !runSimulation)
                {
                    sim = evolutionary.NewestSimulation;
                    uiManager.CurrentGenerationCount.transform.GetComponent<Text>().text = evolutionary.CurrentGeneration.ToString();
                    StartCoroutine(AutoSimulateCoroutine(sim, evolutionary.CurrentGeneration));
                }
            }
        }

    }

    void EndGame()
    {
        Debug.Log("Game Over");
        uiManager.GameOverState();
        gameOver = true;
    }

    public void StepForward(/*int offset_x, int offset_z*/)
    {

        sim.StepForward();
        int numberOfAgents = numberOfEnemies + numberOfTurrets;
        IState state = sim.GetCurrentStep();
        //List<IAgent> agents = state.Agents.ToList();
        List<IAgent> agents = sim.AllAgents.ToList();
        foreach(IAgent agent in agents)
        {
            if (agent.IsActive)
            {
                if (agent is IEnemyAgent && state.Agents.Contains(agent))
                {
                    (int x, int y) = state.PositionOf(agent);
                    agentGODictionary[agent].transform.position = new Vector3(x * 5, 2.75f, y * 5);
                    //agentGODictionary[agent].transform.position = new Vector3(offset_x + x * 5, 2.75f, offset_z + y * 5);

                    var enemyAgent = (IEnemyAgent)agent;
                    var enemyController = agentGODictionary[agent].GetComponent<EnemyController>();
                    enemyController.newHealthPoints = enemyAgent.Health;
                    enemyController.UpdateHealthBar();
                    //enemyController.CheckIfGoalIsreached((grid.Goal.x * tileSize, grid.Goal.y * tileSize));
                    /*var enemyAgent = (SimpleEnemyAgent)agent;
                    Debug.Log("Agent: " + agent + " Health: " + enemyAgent.health);*/
                    
                }
                if (agent is TurretAgent)
                {

                    var turretAgent = (TurretAgent)agent;
                    //Debug.Log(targetGO);
                    agentGODictionary[agent].GetComponent<TurretController>().state = state;
                    Maybe<IAgent> maybeTarget = state.GetTargetOf(turretAgent);
                    maybeTarget.Apply(target =>
                    {
                        Debug.Log(target);
                        if (state.EngagedTargetOf(agent))
                        {
                            agentGODictionary[agent].GetComponent<TurretController>().LookTowardsTarget(target);
                        }
                        else
                        {
                            agentGODictionary[agent].GetComponent<TurretController>().DisableLaser();
                        }

                    });
                    maybeTarget.IfEmpty(agentGODictionary[agent].GetComponent<TurretController>().DisableLaser);

                }
            }
            agentGODictionary[agent].SetActive(state.IsActive(agent));
            PlayerStats.Lives = playerStats.startLives - state.ScoredPoints;
        }
    }

    public (int i, int j) GetGoalPosition()
    {
        return grid.Goal;
    }
    void StepBackward()
    {
        sim.StepBackward();
        //StepEnemiesBackward();
        /*IState state = sim.GetCurrentStep();
        IAgent agent = state.Agents.First();
        (int x, int y) = state.PositionOf(agent);
        enemy.transform.position = new Vector3(x * 5, 3, y * 5);*/

        foreach (var turret in turrets)
        {
            turret.GetComponent<TurretController>().UndoScanForTargetRotation();
            turret.GetComponent<TurretController>().HealTarget();
        }
    }

    void InitializeGridTiles()
    {
        if (grid == null)
            return;

        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                //Debug.Log(grid.TypeAt(i, j));
                InstantiateGridTile(grid.TypeAt(i, j), i, j);
            }
        }
    }

    public void InstantiateGridTile(TileType tileType, int i, int j)
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
            case TileType.Turret:
                tile = Turret;
                parent = transform.Find("Turrets");
                break;
            default:
                tile = Wall;
                parent = transform.Find("Tiles");
                break;
        }

        Transform newTile = (Instantiate(tile, new Vector3(i * grid.tileSize, 0, j * grid.tileSize), Quaternion.identity) as GameObject).transform;

        newTile.SetParent(parent);
    }

    public GameObject InstantiateTurret(int i, int j)
    {
        GameObject newTurret = Instantiate(Turret, new Vector3(i * grid.tileSize, 0, j * grid.tileSize), Quaternion.identity) as GameObject;
        Transform newTurretTile = newTurret.transform;

        newTurretTile.SetParent(transform.Find("Turrets"));

        return newTurret;
    }

    public void RemoveEnemyObjects()
    {
        Transform enemies = transform.Find("Enemies");
        for (int i = 0; i < enemies.childCount; i++)
        {
            Destroy(enemies.GetChild(i).gameObject);
        }
    }
    public void RemoveGameObjects()
    {
        Transform turrets = transform.Find("Turrets");
        for (int i = 0; i < turrets.childCount; i++)
        {
            Destroy(turrets.GetChild(i).gameObject);
        }

        Transform tiles = transform.Find("Tiles");
        for (int i = 0; i < tiles.childCount; i++)
        {
            Destroy(tiles.GetChild(i).gameObject);
        }

        Transform ground = transform.Find("Ground");
        for (int i = 0; i < ground.childCount; i++)
        {
            Destroy(ground.GetChild(i).gameObject);
        }

        Transform enemies = transform.Find("Enemies");
        for (int i = 0; i < enemies.childCount; i++)
        {
            Destroy(enemies.GetChild(i).gameObject);
        }

        Destroy(transform.Find("START(Clone)").gameObject);
        Destroy(transform.Find("END(Clone)").gameObject);
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
                var debug_line_horizontal_start = new Vector3(grid.GetWorldPosition(i, j).x - grid.tileSize / 2, grid.GetWorldPosition(i, j).y, grid.GetWorldPosition(i, j).z - grid.tileSize / 2);
                var debug_line_horizontal_end = new Vector3(grid.GetWorldPosition(i, j + 1).x - grid.tileSize / 2, grid.GetWorldPosition(i, j + 1).y, grid.GetWorldPosition(i, j + 1).z - grid.tileSize / 2);
                var debug_line_vertical_start = new Vector3(grid.GetWorldPosition(i, j).x - grid.tileSize / 2, grid.GetWorldPosition(i, j).y, grid.GetWorldPosition(i, j).z - grid.tileSize / 2);
                var debug_line_vertical_end = new Vector3(grid.GetWorldPosition(i + 1, j).x - grid.tileSize / 2, grid.GetWorldPosition(i + 1, j).y, grid.GetWorldPosition(i + 1, j).z - grid.tileSize / 2);
                Debug.DrawLine(debug_line_horizontal_start, debug_line_horizontal_end, Color.white, 100f);
                Debug.DrawLine(debug_line_vertical_start, debug_line_vertical_end, Color.white, 100f);
            }
        }
        var debug_last_line_horizontal_start = new Vector3(grid.GetWorldPosition(0, grid.Height).x - grid.tileSize / 2, grid.GetWorldPosition(0, grid.Height).y, grid.GetWorldPosition(0, grid.Height).z - grid.tileSize / 2);
        var debug_last_line_horizontal_end = new Vector3(grid.GetWorldPosition(grid.Width, grid.Height).x - grid.tileSize / 2, grid.GetWorldPosition(grid.Width, grid.Height).y, grid.GetWorldPosition(grid.Width, grid.Height).z - grid.tileSize / 2);
        var debug_last_line_vertical_start = new Vector3(grid.GetWorldPosition(grid.Width, 0).x - grid.tileSize / 2, grid.GetWorldPosition(grid.Width, 0).y, grid.GetWorldPosition(grid.Width, 0).z - grid.tileSize / 2);
        var debug_last_line_vertical_end = new Vector3(grid.GetWorldPosition(grid.Width, grid.Height).x - grid.tileSize / 2, grid.GetWorldPosition(grid.Width, grid.Height).y, grid.GetWorldPosition(grid.Width, grid.Height).z - grid.tileSize / 2);
        Debug.DrawLine(debug_last_line_horizontal_start, debug_last_line_horizontal_end, Color.white, 100f);
        Debug.DrawLine(debug_last_line_vertical_start, debug_last_line_vertical_end, Color.white, 100f);
    }
}
