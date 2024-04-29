using UnityEngine;
using UnityEngine.UI;

public class PropertyBox : MonoBehaviour
{
    private ColorBox _colorBox;

    // Start is called before the first frame update
    void Start()
    {
        _colorBox = FindObjectOfType<ColorBox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeColor(Color newColor) {
        _colorBox.ChangeColor(newColor);
    }
}
