using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PropertyBox : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Shaders _shaders;
    private OpenPanel _openPanel;
    private ClosedPanel _closedPanel;
    private TextMeshProUGUI _propertyTmp;
    private ColorBox _colorBox;
    private ColorWheelPanel _colorWheelPanel;
    

    // Start is called before the first frame update
    void Start()
    {
        _shaders = FindObjectOfType<Shaders>();
        _openPanel = GetComponentInChildren<OpenPanel>();
        _closedPanel = GetComponentInChildren<ClosedPanel>();
        _propertyTmp = GetComponentInChildren<TextMeshProUGUI>();
        _colorBox = GetComponentInChildren<ColorBox>();
        _colorWheelPanel = GetComponentInChildren<ColorWheelPanel>();
        
        _colorWheelPanel.Close();
        _openPanel.Close();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if (clickedObject == _colorBox.gameObject) _colorWheelPanel.Toggle();
        else if (clickedObject == _propertyTmp || clickedObject == _openPanel.gameObject || clickedObject == _closedPanel.gameObject) {
            _closedPanel.Toggle();
            _openPanel.Toggle();
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
}
