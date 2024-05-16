using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;

public class UiCanvas : MonoBehaviour

{
    public PropertyBox _propertyBoxZeroSliderPrefab;
    public PropertyBox _propertyBoxOneSliderPrefab;
    public PropertyBox _propertyBoxTwoSlidersPrefab;
    private Canvas _canvas;
    private Shaders _shaders;
    private TextMeshProUGUI _shaderTitleTmp;
    private Dictionary<string, PropertyBox> _propertyBoxes;
    private bool _shadersChanged; 
    private Vector3 _initialPropertyBoxPosition;
    [SerializeField] private Vector3 _propertyBoxPosition;
    private Vector3 _propertyBoxOffset;

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _shaders = FindObjectOfType<Shaders>();
        _shaderTitleTmp = GetComponentInChildren<TextMeshProUGUI>();

        _propertyBoxes = new Dictionary<string, PropertyBox>();
        UpdatePropertyBoxes();

        _shadersChanged = true;
        _initialPropertyBoxPosition = new Vector3(240, -240, 0);
        _propertyBoxPosition = _initialPropertyBoxPosition;
        _propertyBoxOffset = new Vector3(0, -70, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_shadersChanged) {
            DeactivatePropertyBoxes();
            ResetBoxPositions();
            GeneratePropertyBoxes();
            UpdatePropertyBoxes();
            _shadersChanged = false;
        }
        // UpdateCascadingPropertyBoxes();
    }

    private void GeneratePropertyBoxes() {
        // Property names are retrieved sequentially in the same order as in the shader
        foreach (var name in _shaders.TestMaterial.GetPropertyNames(MaterialPropertyType.Vector)) {
            if (name.Split('_').Length > 2) continue; // Ignore default Unity Shader properties with the same property title
            string propertyTitle = ParsePropertyTitle(name);
            if (!_propertyBoxes.ContainsKey(propertyTitle)) {
                switch (name) {
                    case "_materialColor":
                        GeneratePropertyBox(0, propertyTitle, _propertyBoxPosition);
                        break;
                    case "_ambientIntensity":
                        GeneratePropertyBox(1, propertyTitle, _propertyBoxPosition);
                        break;
                    case "_diffuseIntensity":
                        GeneratePropertyBox(1, propertyTitle, _propertyBoxPosition);
                        break;
                    case "_rimIntensity":
                        GeneratePropertyBox(2, propertyTitle, _propertyBoxPosition);
                        break;
                    case "_specularIntensity":
                        GeneratePropertyBox(2, propertyTitle, _propertyBoxPosition);
                        break;
                }
            } else {
                var box = _propertyBoxes[propertyTitle];
                ActivatePropertyBox(box);
                box.GetComponent<RectTransform>().anchoredPosition3D = _propertyBoxPosition;
                _propertyBoxPosition += _propertyBoxOffset;
            }
        };
    }

    private string ParsePropertyTitle(string name) {
        string splitText = name.Substring(1).SplitWords(' ').Split(' ')[0];
        return char.ToUpper(splitText[0]) + splitText.Substring(1);
    }

    private string ParsePropertyName(string name) {
        string splitText = name.Substring(1).SplitWords(' ');
        return char.ToUpper(splitText[0]) + splitText.Substring(1);
    }

    private void GeneratePropertyBox(int numSliders, string propertyTitle, Vector3 position) {
        GameObject instance;
        switch (numSliders) {
            case 0:
                instance = Instantiate(_propertyBoxZeroSliderPrefab.gameObject);
                break;
            case 1:
                instance = Instantiate(_propertyBoxOneSliderPrefab.gameObject);
                break;
            case 2:
                instance = Instantiate(_propertyBoxTwoSlidersPrefab.gameObject);
                break;
            default:
                return;
        }

        instance.transform.SetParent(_canvas.transform, false);
        instance.name = propertyTitle;

        var rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchoredPosition3D = position;

        PropertyBox propertyBox = instance.GetComponent<PropertyBox>();
        propertyBox.PropertyTitleText = propertyTitle;
        propertyBox.ColorPropertyText = $"{propertyTitle} Color";
        if (numSliders > 0 ) propertyBox.SliderOnePropertyText = $"{propertyTitle} Strength";
        if (numSliders > 1) propertyBox.SliderTwoPropertyText = $"{propertyTitle} Radius";

        _propertyBoxPosition += _propertyBoxOffset;
    }

    private void UpdatePropertyBoxes() {
        var propertyBoxes = GetComponentsInChildren<PropertyBox>();
        foreach (var box in propertyBoxes) {
            _propertyBoxes[box.name] = box;
        }
    }

    private void DeactivatePropertyBoxes() {
        foreach (var box in _propertyBoxes.Values) {
            box.gameObject.SetActive(false);
        }
    }
    
    private void ActivatePropertyBox(PropertyBox box) {
        box.gameObject.SetActive(true);
    }

    private void ResetBoxPositions() {
        _propertyBoxPosition = _initialPropertyBoxPosition;
    }

    // private void UpdateCascadingPropertyBoxes() {
    //     if (_propertyBoxes is null) return;
    //     for (int i = 0; i < _propertyBoxes.Length; i++) {
    //         var initialBox = _propertyBoxes[i];
    //         if (initialBox.Opened && !initialBox.Adjusted) {
    //             for (int j = i + 1; j < _propertyBoxes.Length; j++) {
    //                 var subsequentBox = _propertyBoxes[j];
    //                 subsequentBox.transform.Translate(0, -initialBox.OpenedHeight, 0);
    //             }
    //             initialBox.Adjusted = true;
    //         } else if (!initialBox.Opened && initialBox.Adjusted) {
    //             for (int j = i + 1; j < _propertyBoxes.Length; j++) {
    //                 var subsequentBox = _propertyBoxes[j];
    //                 subsequentBox.transform.Translate(0, initialBox.OpenedHeight, 0);
    //             }
    //             initialBox.Adjusted = false;
    //         }
    //     }
    // }

    public void ShadersChanged() {
        _shadersChanged = true;
    }
}
