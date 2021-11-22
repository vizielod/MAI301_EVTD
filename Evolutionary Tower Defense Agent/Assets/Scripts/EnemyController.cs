using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Image healthBar;
    public float startHealthPoints;
    private float currentHealthPoints;
    public SimpleEnemyAgent simpleEnemyAgent;
    public Enemy enemy;
    //public int enemyAgentIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (simpleEnemyAgent == null)
            return;

        startHealthPoints = simpleEnemyAgent.health;
        enemy = new Enemy(startHealthPoints);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = simpleEnemyAgent.health / startHealthPoints;
    }
}
