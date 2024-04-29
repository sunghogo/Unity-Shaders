using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PropertyBox : MonoBehaviour, IPointerClickHandler
{
    private ColorBox _colorBox;
    private ColorWheelPanel _colorWheelPanel;

    // Start is called before the first frame update
    void Start()
    {
        _colorBox = GetComponentInChildren<ColorBox>();
        _colorWheelPanel = GetComponentInChildren<ColorWheelPanel>();

        _colorWheelPanel.Close();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if (clickedObject == _colorBox.gameObject) _colorWheelPanel.Toggle();
        else _colorWheelPanel.Close();

    }
    
    public void ChangeColor(Color newColor) {
        _colorBox.ChangeColor(newColor);
    }
    public void OpenColorWheelPanel() {
        _colorWheelPanel.Open();
    }
    public void CloseColorWheelPanel() {
        _colorWheelPanel.Close();
    }
}
