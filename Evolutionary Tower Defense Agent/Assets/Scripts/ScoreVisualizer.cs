using TMPro;
using UnityEngine;

public class ScoreVisualizer : MonoBehaviour
{
    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetScore(float value)
    {
        text.text = value.ToString("F2");
    }

    public void SetScore(string value)
    {
        text.text = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
