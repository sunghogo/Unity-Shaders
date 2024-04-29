using UnityEngine;
using UnityEngine.UI;

public class ColorMapGenerator : MonoBehaviour
{
    private RawImage _rawImage;
    private float _outerRadius;
    private float _innerRadius;

    void Start()
    {
        _rawImage = GetComponent<RawImage>();
        Texture2D colorWheel = GenerateColorWheel(_rawImage.rectTransform.rect.width, _rawImage.rectTransform.rect.height);
        _rawImage.texture = colorWheel;
        _rawImage.material = null;
    }

    Texture2D GenerateColorWheel(float width, float height)
    {
        Texture2D texture = new Texture2D((int)width, (int)height);
        Vector2 center = new Vector2(width / 2, height / 2);
        float radius = width / 2; // Assume width and height are the same
        _outerRadius = radius;
        _innerRadius = 0.8f * radius;
        texture.filterMode = FilterMode.Point; // Set filter mode to point to avoid blurring

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                float distance = Vector2.Distance(center, position);
                if (_innerRadius <= distance && distance <= _outerRadius) // Inside the circle
                {
                    // float angle = Mathf.Atan2(y - center.y, x - center.x) + Mathf.PI;
                    float angle = Mathf.Atan2(-(center.y - y), x - center.x);
                    if (angle < 0) angle += 2 * Mathf.PI; // Normalize angle to be between 0 and 2*PI
                    float hue = angle / (2 * Mathf.PI);
                    Color color = HSVToRGB(hue, 1.0f, 1.0f); // Full saturation and value
                    texture.SetPixel(x, y, color);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear); // Transparent outside the radius
                }
            }
        }
        texture.Apply();
        return texture;
    }

    public static Color HSVToRGB(float h, float s, float v)
    {
        if (s == 0)
        {
            return new Color(v, v, v); // Achromatic (grey)
        }
        var sector = Mathf.FloorToInt(h * 6);
        var fraction = h * 6 - sector;
        var p = v * (1 - s);
        var q = v * (1 - s * fraction);
        var t = v * (1 - s * (1 - fraction));
        switch (sector % 6)
        {
            case 0:
                return new Color(v, t, p);
            case 1:
                return new Color(q, v, p);
            case 2:
                return new Color(p, v, t);
            case 3:
                return new Color(p, q, v);
            case 4:
                return new Color(t, p, v);
            case 5:
                return new Color(v, p, q);
            default:
                return new Color(0, 0, 0); // Should never happen
        }
    }
}