using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PropertyBox : MonoBehaviour, IPointerClickHandler
{
    private OpenPanel _openPanel;
    private ClosedPanel _closedPanel;
    private TextMeshProUGUI _propertyTmp;
    private TextMeshProUGUI _colorTmp;
    private TextMeshProUGUI _sliderOneTmp;
    private TextMeshProUGUI _sliderTwoTmp;

    public string PropertyTitleText {
        get {return _propertyTmp.text;}
        set {_propertyTmp.text = value;}
    }

    public string ColorPropertyText {
        get {return _colorTmp.text;}
        set {_colorTmp.text = value;}
    }

    public string SliderOnePropertyText {
        get {return _sliderOneTmp.text;}
        set {_sliderOneTmp.text = value;}
    }

    public string SliderTwoPropertyText {
        get {return _sliderTwoTmp.text;}
        set {_sliderTwoTmp.text = value;}
    }

    private ColorBox _colorBox;
    private ColorWheelPanel _colorWheelPanel;

    public float OpenedHeight;
    public float ClosedHeight = 35;
    public bool Opened = false;
    public bool Adjusted = false;

    // Need Awake to make sure properties are properly initialized
    void Awake()
    {
        _openPanel = GetComponentInChildren<OpenPanel>();
        _closedPanel = GetComponentInChildren<ClosedPanel>();

        var tmps = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var tmp in tmps) {
            switch (tmp.name) {
                case "Property (TMP)":
                    _propertyTmp = tmp;
                    break;
                case "Color (TMP)":
                    _colorTmp = tmp;
                    break;
                case "Slider 1 (TMP)":
                    _sliderOneTmp = tmp;
                    break;
                case "Slider 2 (TMP)":
                    _sliderTwoTmp = tmp;
                    break;
            }
        }

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
        if (_sliderOneTmp is null && _sliderOneTmp is null) {
            return openPanelHeight - 10;
        } else if (_sliderOneTmp is not null && _sliderTwoTmp is null) {
            return openPanelHeight;
        } else {
            return openPanelHeight + 10;
        }
    }
}