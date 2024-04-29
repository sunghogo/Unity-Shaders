using UnityEngine;
using UnityEngine.UI;


public class ColorBox : MonoBehaviour
{
    private RawImage _rawImage;
    
    private Color Color { 
        get {return _rawImage.color;} 
        set {_rawImage.color = value;} 
    }

    // Start is called before the first frame update
    void Start()
    {
        _rawImage = GetComponent<RawImage>();
    }

    public void ChangeColor(Color newColor) {
        Color = newColor;
    }

    public Color GetColor () {
        return Color;
    }
}
