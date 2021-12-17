using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameObject GameOverBackground;
    public GameObject StartButton;
    public GameObject Lives;
    public GameObject LivesCount;
    public GameObject GameOver;
    public GameObject RestartButton;
    public GameObject HintText;
    public GameObject RemainingTurrets;
    public GameObject RemainingTurretCount;
    public GameObject CurrentGenerationText;
    public GameObject CurrentGenerationCount;

    public Text hintText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOverState()
    {
        GameOverBackground.SetActive(true);
        GameOver.SetActive(true);
        RestartButton.SetActive(true);
        StartButton.SetActive(false);
        Lives.SetActive(false);
        LivesCount.SetActive(false);
        HintText.SetActive(false);
        RemainingTurrets.SetActive(false);
        RemainingTurretCount.SetActive(false);
        CurrentGenerationText.SetActive(false);
        CurrentGenerationCount.SetActive(false);
    }

    public void Restart()
    {
        GameOverBackground.SetActive(false);
        GameOver.SetActive(false);
        RestartButton.SetActive(false);
        StartButton.SetActive(true);
        Lives.SetActive(true);
        LivesCount.SetActive(true);
        HintText.SetActive(true);
        RemainingTurrets.SetActive(true);
        RemainingTurretCount.SetActive(true);
        CurrentGenerationText.SetActive(true);
        CurrentGenerationCount.SetActive(true);

        hintText.text = "Place Turrets Before you start!";
    }

    public void TurretCountReachedZero()
    {
        hintText.text = "Press Start Button!";
    }

    public void StartGamePressed()
    {
        StartButton.SetActive(false);
        HintText.SetActive(false);
        RemainingTurrets.SetActive(false);
        RemainingTurretCount.SetActive(false);
    }
}
