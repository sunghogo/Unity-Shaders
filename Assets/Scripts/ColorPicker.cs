using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ColorPicker : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Color _currentColor;
    private RawImage _rawImage;
    private PropertyBox _propertyBox;
    
    // Start is called before the first frame update
    void Start()
    {
        _rawImage = GetComponent<RawImage>();
        _propertyBox = FindObjectOfType<PropertyBox>();

        _currentColor =  new Color(1, 1, 1, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Convert click position to UV coordinates
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rawImage.rectTransform, eventData.position, eventData.pressEventCamera, out localCursor)) return;   

        // Normalize the coordinates
        Rect rect = _rawImage.rectTransform.rect;
        Vector2 normalizedPoint = Rect.PointToNormalized(rect, localCursor);

        // Get color from the shader using the UV coordinates
        _currentColor = GetColorFromUV(normalizedPoint);
        
        _propertyBox.ChangeColor(_currentColor);
    }

    private Color GetColorFromUV(Vector2 uv)
    {
        Texture2D tex = _rawImage.texture as Texture2D;
        if (tex != null)
        {
            Color color = tex.GetPixelBilinear(uv.x, uv.y);
            return (color.a == 0) ? _currentColor : color;
        }
        return _currentColor;
    }
}
