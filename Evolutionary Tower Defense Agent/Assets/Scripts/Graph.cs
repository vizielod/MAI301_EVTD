using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private RectTransform graphContainer;
    public int maxDatapointsShown = 10;
    private List<float> valueList;

    /*private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
    }*/
    private void Start()
    {
        valueList = new List<float>();
        Debug.Log(this.transform.GetComponent<RectTransform>().rect.width);
        Debug.Log(this.transform.GetComponent<RectTransform>().rect.height);
        graphContainer.sizeDelta = new Vector2(this.transform.GetComponent<RectTransform>().rect.width,
            this.transform.GetComponent<RectTransform>().rect.height);
        //graphContainer.position = new Vector3(this.transform.GetComponent<RectTransform>().rect.width / 2, this.transform.GetComponent<RectTransform>().rect.height / 2, 0);
        graphContainer.position = new Vector3(0, 0, 0);
    }

    /*private void Update()
    {
        //TODO: Instead of Update, just make a last adjustment when the Start Button is clicked
        graphContainer.sizeDelta = new Vector2(this.transform.GetComponent<RectTransform>().rect.width,
            this.transform.GetComponent<RectTransform>().rect.height);
        graphContainer.position = new Vector3(this.transform.GetComponent<RectTransform>().rect.width / 2, this.transform.GetComponent<RectTransform>().rect.height / 2, 0);
    }*/

    public void addValue(float value)
    {
        valueList.Add(value);
        ShowGraph(valueList);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, float scoreValue)
    {
        GameObject circleGO = new GameObject("circle", typeof(Image));
        circleGO.transform.SetParent(graphContainer, false);
        circleGO.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = circleGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(8, 8);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return circleGO;
    }

    private GameObject CreateCircleWithText(Vector2 anchoredPosition, float scoreValue)
    {
        GameObject circleGO = new GameObject("circle", typeof(Image));
        circleGO.transform.SetParent(graphContainer, false);
        circleGO.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = circleGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(8, 8);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        CreateText(circleGO, scoreValue);
        return circleGO;
    }

    private void CreateText(GameObject circleGO, float scoreValue)
    {
        GameObject scoreTextGO = new GameObject("scoreText", typeof(Text));
        scoreTextGO.transform.SetParent(circleGO.transform, false);

        Text textComponent = scoreTextGO.GetComponent<Text>();
        textComponent.text = scoreValue.ToString();

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        textComponent.font = ArialFont;
        textComponent.material = ArialFont.material;

        textComponent.alignment = TextAnchor.MiddleCenter;

        RectTransform rectTransform = scoreTextGO.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 20, 0);
        rectTransform.sizeDelta = new Vector2(46, 20);

    }

    public void ShowFinalGraph()
    {
        int a = (int)(valueList.Count / maxDatapointsShown);
        List<float> tempList = new List<float>();
        tempList.Add(valueList[0]);

        float sum = 0;
        for (int i = 1; i < valueList.Count-1; i++)
        {
            if (i % a != 0)
            {
                sum += valueList[i];
            }
            if(i % a == 0)
            {
                sum += valueList[i];
                int avg = (int)(sum / a);
                tempList.Add(/*valueList[i]*/avg);
                sum = 0;
            }
        }
        tempList.Add(valueList[valueList.Count - 1]);

        for (int i = 0; i < tempList.Count; i++)
        {
            Debug.Log(tempList[i]);
        }

        ShowGraph(tempList, true);
    }

    private void ShowGraph(List<float> valueList, bool showGraphWithScore = false)
    {
        Clean();

        if (valueList.Count == 0)
            return;

        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float yMargin = 5;
        //float yMaximum = valueList.Max();
        float yMaximum = 20000f;
        float xSize = graphWidth / valueList.Count; //size distance between each point on X axis
        float xMargin = xSize / 2;

        GameObject lastCircleGO = null;
        for (int i = 0; i < valueList.Count; i++)
        {

            float xPosition = i * xSize + xMargin;
            float yPosition = (valueList[i] / yMaximum) * (graphHeight - 2 * yMargin) + yMargin;
            GameObject circleGO;
            if (!showGraphWithScore)
            {
                circleGO = CreateCircle(new Vector2(xPosition, yPosition), valueList[i]);
            }
            else
            {
                circleGO = CreateCircleWithText(new Vector2(xPosition, yPosition), valueList[i]);
            }
            if(lastCircleGO != null)
            {
                /*CreateDotConnection(lastCircleGO.GetComponent<RectTransform>().anchoredPosition,
                    circleGO.GetComponent<RectTransform>().anchoredPosition);*/
                CreateDotConnection(lastCircleGO.GetComponent<RectTransform>().position, 
                    circleGO.GetComponent<RectTransform>().position);
            }
            lastCircleGO = circleGO;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.rotation = Quaternion.Euler(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

        //rectTransform.rotation = 
        //rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
    private void Clean()
    {
        foreach (Transform child in graphContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
