using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color hoverColor;

    private Renderer rend;
    private Color originalColor;
    private GameManager gameManager;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        gameManager = transform.GetComponentInParent<GameManager>();

    }

    private void OnMouseDown()
    {
        if(gameManager.turretCount >= 10)
        {
            Debug.LogWarning("YOU CANNOT PLACE MORE TURRETS! PRESS START!");
            return;
        }
        if (gameManager.useGridWithoutTurretsSetup)
        {
            Debug.Log(transform.position);
            (int i, int j) gridPosition = ((int)(transform.position.x / gameManager.tileSize), (int)(transform.position.z / gameManager.tileSize));
            Debug.Log("Grid Position: " + gridPosition.i + " " + gridPosition.j);
            gameManager.grid.tileTypeArray[gridPosition.i, gridPosition.j] = Simulator.TileType.Turret;
            gameManager.tileTypeArray[gridPosition.i, gridPosition.j] = Simulator.TileType.Turret;
            gameManager.InstantiateGridTile(Simulator.TileType.Turret, gridPosition.i, gridPosition.j);
            GameObject turret = gameManager.InstantiateTurret(gridPosition.i, gridPosition.j);
            gameManager.gridWithoutTurretsArray[gridPosition.i, gridPosition.j] = 4;
            gameManager.InitializeTurretAgent(gridPosition.i, gridPosition.j, turret);
            gameManager.turretCount++;
        }
        else if (gameManager.useGridWithTurretsSetup)
        {
            Debug.LogError("Not a valid action! You cannot place additional turrets on a predefined Grid!");
            return;
        }
        else
        {
            Debug.LogError("Not a valid action!");
            return;
        }
    }
    void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = originalColor;
    }

}
