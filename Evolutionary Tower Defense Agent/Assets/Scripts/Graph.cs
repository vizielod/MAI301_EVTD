using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private RectTransform graphContainer;
    private List<float> valueList;

    private void Start()
    {
        valueList = new List<float>();
    }

    public void addValue(float value)
    {
        valueList.Add(value);
        ShowGraph(valueList);
    }

    private void createCircle(Vector2 anchoredPosition)
    {
        GameObject go = new GameObject("circle", typeof(Image));
        go.transform.SetParent(graphContainer, false);
        go.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(4, 4);
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
        float yMaximum = valueList.Max();
        float xSize = graphWidth / valueList.Count;
        float xMargin = xSize / 2;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = i * xSize + xMargin;
            float yPosition = (valueList[i] / yMaximum) * (graphHeight - 2 * yMargin) + yMargin;
            createCircle(new Vector2(xPosition, yPosition));
        }
    }

    private void Clean()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
