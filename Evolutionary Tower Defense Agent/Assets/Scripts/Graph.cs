using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private RectTransform graphContainer;
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
        graphContainer.position = new Vector3(this.transform.GetComponent<RectTransform>().rect.width / 2, this.transform.GetComponent<RectTransform>().rect.height / 2, 0);

    }

    private void Update()
    {
        //TODO: Instead of Update, just make a last adjustment when the Start Button is clicked
        graphContainer.sizeDelta = new Vector2(this.transform.GetComponent<RectTransform>().rect.width,
            this.transform.GetComponent<RectTransform>().rect.height);
        graphContainer.position = new Vector3(this.transform.GetComponent<RectTransform>().rect.width / 2, this.transform.GetComponent<RectTransform>().rect.height / 2, 0);
    }

    public void addValue(float value)
    {
        valueList.Add(value);
        ShowGraph(valueList);
    }

    private void CreateCircle(Vector2 anchoredPosition)
    {
        GameObject go = new GameObject("circle", typeof(Image));
        go.transform.SetParent(graphContainer, false);
        go.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(8, 8);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    private void ShowGraph(List<float> valueList)
    {
        Clean();

        if (valueList.Count == 0)
            return;

        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float yMargin = 5;
        //float yMaximum = valueList.Max();
        float yMaximum = 20000f;
        float xSize = graphWidth / valueList.Count;
        float xMargin = xSize / 2;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = i * xSize + xMargin;
            float yPosition = (valueList[i] / yMaximum) * (graphHeight - 2 * yMargin) + yMargin;
            CreateCircle(new Vector2(xPosition, yPosition));
        }
    }

    private void Clean()
    {
        foreach (Transform child in graphContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
