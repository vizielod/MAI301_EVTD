using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingtext : MonoBehaviour {

    private RectTransform rectComponent;
    private Image imageComp;

    private float speed;
    private float numberOfSteps;
    public bool startLoadingAnimation;
    public Text text;
    public Text textNormal;
    private float fillAmount;


    // Use this for initialization
    void Start () {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        textNormal.text = "";
        text.text = 0 + "%";
        imageComp.fillAmount = 0.0f;
        fillAmount = 0;
    }
	
    public void InitializeLoadingAnimation(int numberOfSteps)
    {
        imageComp.fillAmount = 0.0f;
        textNormal.text = "Simulating...";
        this.numberOfSteps = numberOfSteps;
        speed = 1f / this.numberOfSteps;
        fillAmount = 0;
        startLoadingAnimation = true;
    }

    public void AnimateLoading()
    {
        int a = 0;
        fillAmount += speed;
        imageComp.fillAmount = fillAmount;
        a = (int)(imageComp.fillAmount * 100);
        text.text = a + "%";
    }

    public void Restart()
    {
        textNormal.text = "";
        text.text = 0 + "%";
        imageComp.fillAmount = 0.0f;
        fillAmount = 0;
    }
	// Update is called once per frame
	void Update () {
        /*if (startLoadingAnimation)
        {
            int a = 0;
            if (imageComp.fillAmount != 1f)
            {
                fillAmount += speed;
                imageComp.fillAmount = fillAmount;
                a = (int)(imageComp.fillAmount * 100);
            }
            text.text = a + "%";
        }
        else
        {
            imageComp.fillAmount = 0.0f;
            text.text = "0%";
        }*/
        /*int a = 0;
        if (imageComp.fillAmount != 1f)
        {
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
            a = (int)(imageComp.fillAmount * 100);
            if (a > 0 && a <= 33)
            {
                textNormal.text = "Loading...";
            }
            else if (a > 33 && a <= 67)
            {
                textNormal.text = "Downloading...";
            }
            else if (a > 67 && a <= 100)
            {
                textNormal.text = "Please wait...";
            }
            else {

            }
            text.text = a + "%";
        }
        else
        {
            imageComp.fillAmount = 0.0f;
            text.text = "0%";
        }*/
    }
}
