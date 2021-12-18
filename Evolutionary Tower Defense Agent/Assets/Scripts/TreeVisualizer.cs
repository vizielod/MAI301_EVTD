using BehaviorTree;
using System.Linq;
using UnityEngine;

public class TreeVisualizer : MonoBehaviour
{
    public GameObject nodePrefab;
    public RectTransform parent;

    private Vector2 size = new Vector2(25, 25);

    public void Visualize(ITraverser traverser)
    {
        var width = traverser.CountWidth();
        var height = traverser.CountHeight();
        size = parent.sizeDelta / new Vector2(width, height);
        foreach (var level in traverser.GenerateTreeNodes())
        {
            var pos = size * new Vector2(1, -level.Item1);
            CreateNode(pos, level.Item2.First());
        }
    }

    public void CreateNode(Vector2 position, INodeInfo node)
    {
        GameObject go = Instantiate(nodePrefab);
        go.GetComponent<Node>().SetText(node.Type.ToString(), node.Name);
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0, 0);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        rect.anchorMin = new Vector2(0.5f, 1);
        rect.anchorMax = new Vector2(0.5f, 1);
    }
}
