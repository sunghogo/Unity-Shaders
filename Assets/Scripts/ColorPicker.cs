using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Color _pickedColor;
    private RawImage _rawImage;
    
    // Start is called before the first frame update
    void Start()
    {
        _rawImage = GetComponent<RawImage>();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Convert click position to UV coordinates
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rawImage.rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
            return;

        Debug.Log(localCursor);
        // Normalize the coordinates
        Rect rect = _rawImage.rectTransform.rect;
        Vector2 normalizedPoint = Rect.PointToNormalized(rect, localCursor);

        // Get color from the shader using the UV coordinates
        Color color = GetColorFromUV(normalizedPoint);
        
        _pickedColor = color;
        Debug.Log("Picked Color: " + color);
    }

    private Color GetColorFromUV(Vector2 uv)
    {
        Texture2D tex = _rawImage.texture as Texture2D;
        if (tex != null)
        {
            Color color = tex.GetPixelBilinear(uv.x, uv.y);
            return color;
        }
        return Color.black;
    }
}
