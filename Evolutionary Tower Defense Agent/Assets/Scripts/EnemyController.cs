using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Image healthBar;
    public float startHealthPoints;
    public float currentHealthPoints;
    public float newHealthPoints;
    public SimpleEnemyAgent simpleEnemyAgent;
    public Enemy enemy;
    //public int enemyAgentIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (simpleEnemyAgent == null)
            return;

        startHealthPoints = simpleEnemyAgent.health;
        currentHealthPoints = simpleEnemyAgent.health;
        newHealthPoints = simpleEnemyAgent.health;
        enemy = new Enemy(startHealthPoints);
    }

    // Update is called once per frame
    void Update()
    {
        //healthBar.fillAmount = simpleEnemyAgent.health / startHealthPoints;
    }

    public void UpdateHealthBar()
    {
        if(currentHealthPoints != newHealthPoints)
        {
            healthBar.fillAmount = simpleEnemyAgent.health / startHealthPoints;
            currentHealthPoints = newHealthPoints;
        }
    }
}
