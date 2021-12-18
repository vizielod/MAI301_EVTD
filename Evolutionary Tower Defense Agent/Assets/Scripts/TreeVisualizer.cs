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
        Clean();

        var width = traverser.CountWidth();
        var height = traverser.CountHeight();
        size = parent.sizeDelta / new Vector2(width, height);
        foreach (var level in traverser.GenerateTreeNodes())
        {
            var nNodes = level.Item2.Count();
            var xSpacing = parent.sizeDelta.x / nNodes;
            int i = 0;
            foreach (var node in level.Item2)
            {
                var pos = new Vector2(xSpacing * i , size.y * -level.Item1);
                CreateNode(pos, node);
                i++;
            }
        }
    }

    private void Clean()
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateNode(Vector2 position, INodeInfo node)
    {
        GameObject go = Instantiate(nodePrefab);
        go.GetComponent<Node>().SetText(node.Type.ToString(), node.Name);
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }
}
