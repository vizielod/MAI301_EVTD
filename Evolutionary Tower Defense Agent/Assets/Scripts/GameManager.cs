using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject enemy;
    private void Awake()
    {
        //enemies = new GameObject[]
    }
    // Start is called before the first frame update
    void Start()
    {
        Grid grid = new Grid(5, 5, 5);
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
        enemy.GetComponent<EnemyMovementController>().PerformStepForward();
    }

    void StepBackward()
    {
        enemy.GetComponent<EnemyMovementController>().PerformStepBackward();
    }
}
