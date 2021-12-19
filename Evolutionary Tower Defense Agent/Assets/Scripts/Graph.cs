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
    public int numberOfHorizontalLines = 8;
    private List<float> valueList;
    private bool newValueAdded = false;
    private bool lastValueAdded = false;
    private int showEveryXthGeneration;

    private List<int> genIndexList;

    private void Start()
    {
        genIndexList = new List<int>();
        valueList = new List<float>();
        Debug.Log(this.transform.GetComponent<RectTransform>().rect.width);
        Debug.Log(this.transform.GetComponent<RectTransform>().rect.height);
        graphContainer.sizeDelta = new Vector2(this.transform.GetComponent<RectTransform>().rect.width,
            this.transform.GetComponent<RectTransform>().rect.height);
        //graphContainer.position = new Vector3(this.transform.GetComponent<RectTransform>().rect.width / 2, this.transform.GetComponent<RectTransform>().rect.height / 2, 0);
        graphContainer.position = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        //TODO: Instead of Update, just make a last adjustment when the Start Button is clicked
        /*graphContainer.sizeDelta = new Vector2(this.transform.GetComponent<RectTransform>().rect.width,
            this.transform.GetComponent<RectTransform>().rect.height);
        graphContainer.position = new Vector3(this.transform.GetComponent<RectTransform>().rect.width / 2, this.transform.GetComponent<RectTransform>().rect.height / 2, 0);*/
        if (newValueAdded)
        {
            ShowGraph(valueList, false);
            newValueAdded = false;
        }
        if (lastValueAdded)
        {
            ShowFinalGraph();
            lastValueAdded = false;
        }
    }

    public void addValue(float value)
    {
        valueList.Add(value);
        newValueAdded = true;
        //ShowGraph(valueList);
    }

    public void LastValueAdded()
    {
        lastValueAdded = true;
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
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

    private void CreateTextForXAxis(GameObject verticalLineGO, int index)
    {
        GameObject scoreTextGO = new GameObject("scoreText", typeof(Text));
        scoreTextGO.transform.SetParent(verticalLineGO.transform, false);

        Text textComponent = scoreTextGO.GetComponent<Text>();
        if (showEveryXthGeneration > 1)
        {
            if (index == 0)
            {
                textComponent.text = "Gen. " + index.ToString();
            }
            else
            {
                textComponent.text = "Gen. " + ((index * showEveryXthGeneration) - (showEveryXthGeneration - 1)).ToString()
                    + "-" + (index * showEveryXthGeneration).ToString();
            }
        }
        else
        {
            textComponent.text = "Gen. " + index.ToString();
        }

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        textComponent.font = ArialFont;
        textComponent.material = ArialFont.material;
        textComponent.fontSize = 10;
        textComponent.alignment = TextAnchor.MiddleCenter;

        RectTransform rectTransform = scoreTextGO.GetComponent<RectTransform>();
        rectTransform.localRotation = Quaternion.Euler(0, 0, -90);
        rectTransform.localPosition = new Vector3(6, 0, 0);
        rectTransform.sizeDelta = new Vector2(75, 20);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    public void ShowFinalGraph()
    {
        
        showEveryXthGeneration = (int)(valueList.Count / maxDatapointsShown) > 0 ? 
            (int)(valueList.Count / maxDatapointsShown) : 
            0;
        List<float> tempList = new List<float>();

        if (showEveryXthGeneration > 0)
        {
            tempList.Add(valueList[0]);
            float sum = 0;
            for (int i = 1; i < valueList.Count - 1; i++)
            {
                if (i % showEveryXthGeneration != 0)
                {
                    sum += valueList[i];
                }
                if (i % showEveryXthGeneration == 0)
                {
                    sum += valueList[i];
                    int avg = (int)(sum / showEveryXthGeneration);
                    tempList.Add(/*valueList[i]*/avg);
                    sum = 0;
                }
            }
            tempList.Add(valueList[valueList.Count - 1]);
        }
        else
        {
            for (int i = 0; i < valueList.Count; i++)
            {
                tempList.Add(valueList.Count);
            }
        }

        for (int i = 0; i < tempList.Count; i++)
        {
            Debug.Log(tempList[i]);
        }
        ShowGraph(tempList, true);
        CreateHorizontalLines();
    }

    private void ShowGraph(List<float> valueList, bool showGraphWithScore = false)
    {
        CleanGraphVisuals();

        if (valueList.Count == 0)
            return;

        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float yMargin = 15+15;
        float yMaximum = valueList.Max()+15;
        //float yMaximum = 20000f;
        float xSize = (graphWidth-30) / valueList.Count; //size distance between each point on X axis
        float xMargin = xSize / 2;

        GameObject lastCircleGO = null;
        for (int i = 0; i < valueList.Count; i++)
        {

            float xPosition = (i * xSize) + xMargin + 15;
            float yPosition = (valueList[i] / yMaximum) * (graphHeight - 2 * yMargin) + yMargin;
            GameObject circleGO;
            if (!showGraphWithScore)
            {
                circleGO = CreateCircle(new Vector2(xPosition, yPosition));
            }
            else
            {
                circleGO = CreateCircleWithText(new Vector2(xPosition, yPosition), valueList[i]);
                CreateVerticalLines(new Vector2(circleGO.transform.position.x, circleGO.transform.position.y), i);
            }
            if(lastCircleGO != null)
            {
                CreateDotConnection(lastCircleGO.GetComponent<RectTransform>().position, 
                    circleGO.GetComponent<RectTransform>().position);
            }
            lastCircleGO = circleGO;
        }
    }

    private void CreateVerticalLines(Vector2 dotPosition, int index)
    {
        GameObject verticalLineGO = new GameObject("verticalLine", typeof(Image));
        verticalLineGO.transform.SetParent(graphContainer, false);
        verticalLineGO.GetComponent<Image>().color = new Color(1, 1, 1, .2f);
        RectTransform rectTransform = verticalLineGO.GetComponent<RectTransform>();
        float height = graphContainer.sizeDelta.y;
        Vector2 lineStartPoint = new Vector2(dotPosition.x, 15);
        Vector2 lineEndPoint = new Vector2(dotPosition.x, height-15);
        float distance = Vector2.Distance(lineStartPoint, lineEndPoint);
        Vector2 dir = (lineEndPoint - lineStartPoint).normalized;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 1f);
        rectTransform.anchoredPosition = lineStartPoint + dir * distance * .5f;
        rectTransform.rotation = Quaternion.Euler(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

        CreateTextForXAxis(verticalLineGO, index);
    }

    private void CreateHorizontalLines()
    {
        float width = graphContainer.sizeDelta.x;
        float height = graphContainer.sizeDelta.y;

        for (int i = 0; i < numberOfHorizontalLines; i++)
        {
            GameObject horizontalLineGO = new GameObject("horizontalLine", typeof(Image));
            horizontalLineGO.transform.SetParent(graphContainer, false);
            horizontalLineGO.GetComponent<Image>().color = new Color(1, 1, 1, .2f);
            RectTransform rectTransform = horizontalLineGO.GetComponent<RectTransform>();
            Vector2 lineStartPoint = new Vector2(15, 30 + i * (height - 30) / numberOfHorizontalLines);
            Vector2 lineEndPoint = new Vector2(width - 15, 30 + i * (height - 30) / numberOfHorizontalLines);
            float distance = Vector2.Distance(lineStartPoint, lineEndPoint);
            Vector2 dir = (lineEndPoint - lineStartPoint).normalized;
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 1f);
            rectTransform.anchoredPosition = lineStartPoint + dir * distance * .5f;
            rectTransform.rotation = Quaternion.Euler(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
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
    }
    private void CleanGraphVisuals()
    {
        foreach (Transform child in graphContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CleanGraphData()
    {
        valueList.Clear();
        genIndexList.Clear();
    }

    public void CleanGraph()
    {
        CleanGraphVisuals();
        CleanGraphData();
    }
}
