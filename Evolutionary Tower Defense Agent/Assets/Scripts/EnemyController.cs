using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float hitPoints = 50f;

    public float speed = 10f;
    public float stepSize = 5f;

    private Transform previousTarget;
    private Transform nextTarget;
    private int waypointIndex = 0;

    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = new Enemy(hitPoints);
    }

    // Update is called once per frame
    void Update()
    {
    }
}