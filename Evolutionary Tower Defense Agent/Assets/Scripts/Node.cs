using BehaviorTree;
using System;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public TextMeshProUGUI typeLabel;
    public TextMeshProUGUI nameLabel;

    public Guid ID { get; private set; }

    public void FeedNode(INodeInfo node)
    {
        typeLabel.text = node.Type.ToString();
        nameLabel.text = node.Name;
        ID = node.ID;
    }

    public Vector2 GetPos() => GetComponent<RectTransform>().anchoredPosition;
}
