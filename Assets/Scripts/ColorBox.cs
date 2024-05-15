using UnityEngine;
using UnityEngine.UI;


public class ColorBox : MonoBehaviour
{
    private RawImage _rawImage;
    
    private Color Color { 
        get {return _rawImage.color;} 
        set {_rawImage.color = value;} 
    }

    // Need Awake to make sure properties are properly initialized
    void Awake()
    {
        _rawImage = GetComponent<RawImage>();
        Color = new Color(1, 1, 1, 1);
    }

    public void ChangeColor(Color newColor) {
        Color = newColor;
    }

    public Color GetColor () {
        return Color;
    }
}
