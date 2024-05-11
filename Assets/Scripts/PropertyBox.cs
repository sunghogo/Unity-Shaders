using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PropertyBox : MonoBehaviour, IPointerClickHandler
{
    private OpenPanel _openPanel;
    private ClosedPanel _closedPanel;
    private TextMeshProUGUI _propertyTmp;
    private ColorBox _colorBox;
    private ColorWheelPanel _colorWheelPanel;
    public float OpenedHeight;
    public float ClosedHeight = 35;
    public bool Opened = false;
    public bool Adjusted = false;

    // Start is called before the first frame update
    void Start()
    {
        _openPanel = GetComponentInChildren<OpenPanel>();
        _closedPanel = GetComponentInChildren<ClosedPanel>();
        _propertyTmp = GetComponentInChildren<TextMeshProUGUI>();
        _colorBox = GetComponentInChildren<ColorBox>();
        _colorWheelPanel = GetComponentInChildren<ColorWheelPanel>();
        OpenedHeight = GetOpenedHeight();

        _colorWheelPanel.Close(); // Need to cache and close here to properly reference it for clicker event
        Close();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if (clickedObject == _colorBox.gameObject) _colorWheelPanel.Toggle();
        else if (clickedObject == _propertyTmp || clickedObject == _openPanel.gameObject || clickedObject == _closedPanel.gameObject) {
            Toggle();
        }
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

    private void Open() {
        Opened = true;
        _openPanel.Open();
        _closedPanel.Close();
    }

    private void Close() {
        Opened = false;
        _openPanel.Close();
        _closedPanel.Open();
    }

    private void Toggle() {
        Opened = !Opened;
        _closedPanel.Toggle();
        _openPanel.Toggle();
    }

    private float GetOpenedHeight() {
        var openPanelHeight = GetComponentInChildren<OpenPanel>().GetComponent<RectTransform>().rect.height;
        return openPanelHeight > 100 ? openPanelHeight + 10 : openPanelHeight;
    }
}