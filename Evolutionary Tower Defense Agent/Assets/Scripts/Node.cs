using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public TextMeshProUGUI typeLabel;
    public TextMeshProUGUI nameLabel;

    public void SetText(string type, string name)
    {
        typeLabel.text = type;
        nameLabel.text = name;
    }

}
