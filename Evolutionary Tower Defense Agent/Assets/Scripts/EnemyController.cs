using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree.Agents;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Image healthBar;
    public float startHealthPoints;
    public float currentHealthPoints;
    public float newHealthPoints;
    //public SimpleEnemyAgent simpleEnemyAgent;
    public IEnemyAgent enemyAgent;
    public GameManager gameManager;
    public Enemy enemy;
    //public int enemyAgentIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (enemyAgent == null)
            return;

        startHealthPoints = enemyAgent.Health;
        currentHealthPoints = enemyAgent.Health;
        newHealthPoints = enemyAgent.Health;
        enemy = new Enemy(startHealthPoints);
    }

    // Update is called once per frame
    void Update()
    {
        //healthBar.fillAmount = simpleEnemyAgent.health / startHealthPoints;
    }

    public void UpdateHealthBar()
    {
        if (currentHealthPoints != newHealthPoints)
        {
            healthBar.fillAmount = enemyAgent.Health / startHealthPoints;
            currentHealthPoints = newHealthPoints;
        }
    }

    public void CheckIfGoalIsreached((int i, int j) goalPosition)
    {
        if (this.transform.position.x == goalPosition.i && this.transform.position.z == goalPosition.j)
        {
            Debug.Log("Reached Goal");
            PlayerStats.Lives--;
            //transform.gameObject.SetActive(false); //Here would be better to Set the Agent IsActive to false on the backend.
        }
    }

    private void OnMouseDown()
    {
        gameManager.treeVisualizer.Visualize(enemyAgent.GetTree());
    }
}
