using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public static int Lives;
    public int startLives = 20;

    public static int remainingTurretcount;

    public Text livesText;
    public Text remainingTurretCountText;
    // Start is called before the first frame update
    void Start()
    {
        Lives = startLives;
        livesText.text = Lives.ToString();
    }

    public void UpdateLivesText()
    {
        livesText.text = Lives.ToString();
    }

    public void Restart()
    {
        Lives = startLives;
        livesText.text = Lives.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        livesText.text = Lives.ToString();
        remainingTurretCountText.text = remainingTurretcount.ToString();
    }
}
