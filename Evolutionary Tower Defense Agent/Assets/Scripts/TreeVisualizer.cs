using BehaviorTree;
using CodeMonkey.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TreeVisualizer : MonoBehaviour
{
    public GameObject nodePrefab;
    public RectTransform container;
    public NodeTreeInspector window;

    private Vector2 size = new Vector2(25, 25);
    private Dictionary<Guid, Node> NodeCache;

    private void Start()
    {
        NodeCache = new Dictionary<Guid, Node>();
    }

    public void Visualize(ITraverser traverser)
    {
        Clean();

        var width = traverser.CountWidth() + 1;
        var height = traverser.CountHeight();
        size = new Vector2(container.rect.width/width, container.rect.height/height);
        foreach (var level in traverser.GenerateTreeNodes())
        {
            var nNodes = level.Item2.Count();
            var xSpacing = container.rect.width / (nNodes + 1);
            int i = 1;
            var cache = new Dictionary<Guid, Node>();
            foreach (var node in level.Item2)
            {
                var pos = new Vector2(xSpacing * i , size.y * -level.Item1);
                var (key, value) = CreateNode(pos, node);
                cache.Add(key, value);
                i++;
            }
            NodeCache = cache;
        }

        window.Show();
    }

    private void Clean()
    {
        foreach (Transform child in container.transform)
            Destroy(child.gameObject);
    }

    public (Guid, Node) CreateNode(Vector2 position, INodeInfo node)
    {
        GameObject go = Instantiate(nodePrefab);
        var nodeComp = go.GetComponent<Node>();
        nodeComp.FeedNode(node);
        go.transform.SetParent(container, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        if (node.Parent.HasValue && NodeCache.ContainsKey(node.Parent.Value))
            LinkNodes(nodeComp.GetPos(), NodeCache[node.Parent.Value].GetPos());

        return (node.ID, nodeComp);
    }

    private void LinkNodes(Vector2 start, Vector2 end)
    {
        GameObject lineGO = new GameObject("verticalLine", typeof(Image));
        lineGO.transform.SetParent(container, false);
        lineGO.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        RectTransform rectTransform = lineGO.GetComponent<RectTransform>();
        float distance = Vector2.Distance(start, end);
        Vector2 dir = (end - start).normalized;
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.sizeDelta = new Vector2(distance, 1f);
        rectTransform.anchoredPosition = start + dir * distance * .5f - new Vector2(0 , size.y * 0.5f);
        rectTransform.rotation = Quaternion.Euler(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
}
