using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelSimManager : MonoBehaviour
{

    public GameObject levelGO;
    public int levelCount;
    public List<GameObject> levels;
    // Start is called before the first frame update
    void Start()
    {
        levels = new List<GameObject>();
        levels.Add(levelGO);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < levelCount; i++)
            {
                int offset_x = i / (levelCount / 2);
                int offset_z = i % (levelCount / 2);
                levels[i].GetComponent<GameManager>().StepForward(/*offset_x * 100, offset_z * 100*/);
            }
        }
    }

    public void MultiplyLevel()
    {
        for (int i = 1; i < levelCount; i++)
        {
            GameObject level = Instantiate(levelGO, new Vector3((i/(levelCount/2) * 100), 0, (i%(levelCount / 2)) *100), Quaternion.identity);
            levels.Add(level);
        }
    }

    public async void StartGame()
    {
        foreach(GameObject level in levels)
        {
            level.GetComponent<GameManager>().StartGame();
        }
    }
}
